using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuaternionTest))]
[CanEditMultipleObjects]
public class QuaternionTestEditor : Editor
{
    private SerializedProperty speedProp;
    private SerializedProperty angleProp;

    void OnEnable()
    {
        // Setup the SerializedProperties
        speedProp = serializedObject.FindProperty("Speed");
        angleProp = serializedObject.FindProperty("Angle");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var quaternionTest = target as QuaternionTest;

        EditorGUIUtility.LookLikeControls();

        //EditorGUILayout.Slider(speedProp, 0f, 1f, new GUIContent("Set Speed"));

        //if (!angleProp.hasMultipleDifferentValues)
        {
            //EditorGUILayout.Slider(angleProp, 0f, 1f, "Set Angle");
        }

        DrawDefaultInspector();

        EditorGUIUtility.LookLikeInspector();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
