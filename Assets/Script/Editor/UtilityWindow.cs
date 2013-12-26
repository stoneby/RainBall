using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UtilityWindow : EditorWindow
{
    private bool containEmptyLine;
    private string toggleText = "Contain empty lines";

    private Vector2 scrollPosition;

    private string assetPath;
    private readonly List<FileInfo> scriptList = new List<FileInfo>();
    private readonly Dictionary<string, int> scriptLinesDict = new Dictionary<string, int>();
    private readonly Dictionary<string, int> countingFileDict = new Dictionary<string, int>();
    private readonly List<string> supportedFileList = new List<string>();

    private readonly Dictionary<string, List<string>> fileGroupDict = new Dictionary<string, List<string>>
                                                                          {
                                                                              {
                                                                                  "Material", new List<string>
                                                                                                  {
                                                                                                      ".mat"
                                                                                                  }
                                                                              },
                                                                              {
                                                                                  "Model", new List<string>
                                                                                               {
                                                                                                   ".dds",
                                                                                                   ".fbx",
                                                                                               }
                                                                              },
                                                                              {
                                                                                  "Texture", new List<string>
                                                                                                 {
                                                                                                     ".jpg",
                                                                                                     ".png"
                                                                                                 }
                                                                              },
                                                                              {
                                                                                  "Script", new List<string>
                                                                                                {
                                                                                                    ".cs"
                                                                                                }
                                                                              },
                                                                              {
                                                                                  "Data", new List<string>
                                                                                              {
                                                                                                  ".txt"
                                                                                              }
                                                                              },
                                                                              {
                                                                                  "Prefabs", new List<string>
                                                                                                 {
                                                                                                     ".prefab"
                                                                                                 }
                                                                              },
                                                                              {
                                                                                  "Scene", new List<string>
                                                                                               {
                                                                                                   ".unity"
                                                                                               }
                                                                              }
                                                                          };

    [MenuItem("Tool/ProjectSummary")]
    static void Init()
    {
        GetWindow<UtilityWindow>();
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        assetPath = Application.dataPath;
        Debug.Log("Asset path: " + assetPath);
        foreach(var value in fileGroupDict.Values)
        {
            supportedFileList.AddRange(value);
        }
        Debug.Log("Supported file list: " + supportedFileList.Count);
        AnalyseSpecificFiles();

        AnalyseScriptFiles();
    }

    private void AnalyseScriptFiles()
    {
        foreach(var script in scriptList)
        {
            if(!scriptLinesDict.ContainsKey(script.Name))
            {
                scriptLinesDict.Add(script.Name, 0);
            }
            else
            {
                scriptLinesDict[script.Name] = 0;
            }

            var lines = File.ReadAllLines(script.FullName);
            foreach(var line in lines)
            {
                if(containEmptyLine && line == string.Empty)
                {
                    continue;
                }
                ++scriptLinesDict[script.Name];
            }
        }
    }

    private void AnalyseSpecificFiles()
    {
        foreach(var file in Directory.GetFiles(assetPath, "*", SearchOption.AllDirectories))
        {
            var fileInfor = new FileInfo(file);
            if(!supportedFileList.Contains(fileInfor.Extension))
            {
                continue;
            }
            if(!countingFileDict.ContainsKey(fileInfor.Extension))
            {
                countingFileDict.Add(fileInfor.Extension, 0);
            }
            countingFileDict[fileInfor.Extension]++;

            if(fileGroupDict["Script"].Contains(fileInfor.Extension))
            {
                scriptList.Add(fileInfor);
                Debug.Log("Adding script: " + fileInfor.FullName);
            }
        }
    }

    int CountTotalLine()
    {
        var result = 0;
        foreach(var pair in scriptLinesDict)
        {
            result += pair.Value;
        }
        return result;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Label("Project files summary:");

        GUILayout.BeginVertical();

        foreach(var pair in countingFileDict)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(pair.Key, GUILayout.MaxWidth(60f));
            GUILayout.Label("" + pair.Value, "button", GUILayout.MinWidth(60f));
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        GUILayout.Space(10f);

        GUILayout.Label("Script files summary:");

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(100f));
        foreach(var pair in scriptLinesDict)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(pair.Key, GUILayout.MaxWidth(100f));
            GUILayout.Label("" + pair.Value, "button", GUILayout.MinWidth(60f));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();

        GUILayout.Label("Total script lines: ", GUILayout.MaxWidth(160f));
        var totalLine = CountTotalLine();
        GUILayout.Label("" + totalLine, "button", GUILayout.MinWidth(60f));

        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        if(GUILayout.Button(toggleText))
        {
            containEmptyLine = !containEmptyLine;
            toggleText = (containEmptyLine) ? "Contain empty lines" : "Not contain empty lines";
            AnalyseScriptFiles();
        }

        GUILayout.EndVertical();
    }
}
