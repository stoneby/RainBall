using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathGenerator))]
public class PathGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var pathGenerator = target as PathGenerator;

        if (GUILayout.Button("Generate Tween Path"))
        {
            if (!ErrorChecking())
            {
                return;
            }

            if (!pathGenerator.GenerateTweenPath(pathGenerator.SampleFile))
            {
                EditorUtility.DisplayDialog("Error", "Make sure objects are set correctly and Sample File is valid, please do re-selection.", "Gocha");
            }
        }

        if (GUILayout.Button("Generate All Paths"))
        {
            if (!ErrorChecking())
            {
                return;
            }

            var pathLocation = Application.dataPath;
            foreach (var file in Directory.GetFiles(pathLocation))
            {
                var fileInfor = new FileInfo(file);
                if (!fileInfor.Extension.Contains("txt"))
                {
                    continue;
                }
                var fileName = fileInfor.Name;
                var textAsset = Resources.LoadAssetAtPath("Assets/" + fileName, typeof(TextAsset)) as TextAsset;
                pathGenerator.GenerateTweenPath(textAsset);
            }
        }
    }

    private bool ErrorChecking()
    {
        var pathGenerator = target as PathGenerator;
        if (pathGenerator.PathParent == null || pathGenerator.SampleFile == null)
        {
            EditorUtility.DisplayDialog("Error", "Make sure objects are set correctly.", "Gocha");
            return false;
        }
        return true;
    }
}
