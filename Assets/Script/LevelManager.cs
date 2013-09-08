using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	void Awake ()
	{
		if (LevelList == null || LevelList.Count == 0)
		{
			Debug.LogError("LevelList could not be null, please configure it first.");
			return;
		}
		CurrentLevel = 0;
	}
}
