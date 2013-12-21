using UnityEditor;

[CustomEditor(typeof(PathSampler))]
public class PathSamplerEditor : Editor
{
    private SerializedProperty sampleAllProperty;
    private SerializedProperty sampleListProperty;
    private SerializedProperty internalDurationProperty;
    private SerializedProperty parserProperty;

    private string sampleListCount;

    void OnEnable()
    {
        sampleAllProperty = serializedObject.FindProperty("SampleAll");
        internalDurationProperty = serializedObject.FindProperty("InternalDuration");
        parserProperty = serializedObject.FindProperty("Parser");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(internalDurationProperty);
        EditorGUILayout.PropertyField(sampleAllProperty);
        EditorGUILayout.PropertyField(parserProperty);

        if(!sampleAllProperty.boolValue)
        {
            DrawSampleList();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSampleList()
    {
        sampleListProperty = serializedObject.FindProperty("SampleList");

        EditorGUILayout.BeginVertical();
        do
        {
            if(sampleListProperty.propertyPath != "SampleList" &&
               !sampleListProperty.propertyPath.StartsWith("SampleList" + "."))
            {
                break;
            }
            EditorGUILayout.PropertyField(sampleListProperty);
        } while(sampleListProperty.NextVisible(true));
        EditorGUILayout.EndVertical();
    }
}
