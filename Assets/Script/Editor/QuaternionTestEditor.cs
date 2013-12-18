using UnityEditor;

[CustomEditor(typeof(QuaternionTest))]
[CanEditMultipleObjects]
public class QuaternionTestEditor : Editor
{
    void OnEnable()
    {
        // Setup the SerializedProperties
        serializedObject.FindProperty("Speed");
        serializedObject.FindProperty("Angle");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUIUtility.LookLikeControls();

        DrawDefaultInspector();

        EditorGUIUtility.LookLikeInspector();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
