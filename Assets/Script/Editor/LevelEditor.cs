using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor {

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		var level = target as Level;
		var path = level.GetComponent<iTweenPath>().PathName;
		level.Path = EditorGUILayout.TextField("Path", path);
	}
}
