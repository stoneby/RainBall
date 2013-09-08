using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorTest))]
public class EditorTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var editorTest = target as EditorTest;

        editorTest.LookAtPoint = EditorGUILayout.Vector3Field("Look At You", editorTest.LookAtPoint);
        editorTest.Blend = EditorGUILayout.Slider("Blender Value", editorTest.Blend, 0f, 1f);
        editorTest.Switch = EditorGUILayout.Toggle("Switch Opener", editorTest.Switch);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
