using UnityEditor;

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

        EditorGUIUtility.LookLikeControls();

        DrawDefaultInspector();

        EditorGUIUtility.LookLikeInspector();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
