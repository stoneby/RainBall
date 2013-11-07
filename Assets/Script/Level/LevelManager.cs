using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> LevelList;

    public int CurrentLevel { get; set; }

    public void NextLevel()
    {
        CurrentLevel = (CurrentLevel == LevelList.Count - 1) ? 0 : (CurrentLevel + 1);
    }

    public string GetDetailPath()
    {
        var currentPath = LevelList[CurrentLevel].Path;
        return GetDetailPath(currentPath);
    }

    public static string GetDetailPath(string path)
    {
        return string.Format("{0}_Detail", path);
    }

    public List<Vector3> GetPathNodes()
    {
        var currentPath = LevelList[CurrentLevel].Path;
        return GetPathNodes(currentPath);
    }

    public static List<Vector3> GetPathNodes(string path)
    {
        var result = new List<Vector3>();
        var detailPath = GetDetailPath(path);
        var pathNodes = iTweenPath.GetPath(detailPath);
        result.AddRange(pathNodes);
        return result;
    }

    void Awake()
    {
        if (LevelList == null || LevelList.Count == 0)
        {
            Debug.LogError("LevelList could not be null, please configure it first.");
            return;
        }
        CurrentLevel = 0;
    }
}
