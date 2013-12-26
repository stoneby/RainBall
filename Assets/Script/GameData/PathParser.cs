using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathParser : MonoBehaviour
{
    public TextAsset SampleFile;

    public List<Vector3> PositionList { get; set; }

    public bool ReadFromFile(TextAsset textAsset)
    {
        if (PositionList == null)
        {
            PositionList = new List<Vector3>();
        }
        PositionList.Clear();
        try
        {
            var lines = textAsset.text.Split(new [] {'\r', '\n'});
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var thinLine = line.Trim().Substring(1, line.Length - 2);
                var tokens = thinLine.Split(',');
                var position = new Vector3(float.Parse(tokens[0].Trim()),
                                           float.Parse(tokens[1].Trim()), float.Parse(tokens[2].Trim()));
                PositionList.Add(position);
            }
            Debug.Log("Read from file: " + SampleFile.name + ", position count = " + PositionList.Count);
        }
        catch (Exception e)
        {
            Debug.LogWarning("File: " + SampleFile.name + " is not a valid sample file.\n" + e.StackTrace);
            return false;
        }
        return true;
    }

    public void WriteToFile(string fileName)
    {
        var text = string.Empty;
        PositionList.ForEach(position =>
        {
            text = string.Format("{0}\n{1}", text, position);
        });
        var path = string.Format("{0}/{1}", Application.dataPath, fileName);
        //File.WriteAllText(path, text);
        Debug.Log("Path parser write to file: " + path);
    }
}
