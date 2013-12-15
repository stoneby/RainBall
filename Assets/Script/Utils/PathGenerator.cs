using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public GameObject PathParent;

    public TextAsset SampleFile;

    private readonly List<Vector3> positionList = new List<Vector3>();

    public bool GenerateTweenPath(TextAsset textAsset)
    {
        if (!ReadFromFile(textAsset))
        {
            return false;
        }

        ConfigureTweenPath(textAsset);
        return true;
    }

    private void ConfigureTweenPath(TextAsset textAsset)
    {
        var pathName = LevelManager.GetDetailPath(textAsset.name);

        var pathLength = iTween.PathLength(positionList.ToArray());
        var nodeCount = (int)(pathLength / Utils.BallGenerator.TemplateBallList[0].GetComponent<SphereCollider>().radius * 2) + 1;
        nodeCount = (nodeCount >= iTweenPath.MaxNodeCount) ? iTweenPath.MaxNodeCount : nodeCount;

        Debug.Log("Generate path: name - " + pathName + ", length - " + pathLength + ", detail node count - " + nodeCount);

        var pathObject = new GameObject(pathName);
        pathObject.transform.parent = PathParent.transform;
        pathObject.layer = PathParent.layer;
        var trimmedNodeList = Utils.TrimList(positionList, nodeCount);
        Utils.ConfigureTweenPath(pathObject, trimmedNodeList, pathName);
    }

    private bool ReadFromFile(TextAsset textAsset)
    {
        positionList.Clear();
        try
        {
            var lines = textAsset.text.Split('\n');
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
                positionList.Add(position);
            }
            Debug.Log("Read from file: " + SampleFile.name + ", position count = " + positionList.Count);
        }
        catch(Exception)
        {
            Debug.LogWarning("File: " + SampleFile.name + " is not a valid sample file.");
            return false;
        }
        return true;
    }
}
