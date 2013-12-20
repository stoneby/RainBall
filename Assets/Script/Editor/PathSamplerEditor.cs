using UnityEditor;

[CustomEditor(typeof(PathSampler))]
public class PathSamplerEditor : Editor
{
    private SerializedProperty sampleAllProperty;
    private SerializedProperty sampleListProperty;
    private SerializedProperty internalDurationProperty;

    private string sampleListCount;

    void OnEnable()
    {
        sampleAllProperty = serializedObject.FindProperty("SampleAll");
        internalDurationProperty = serializedObject.FindProperty("InternalDuration");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(internalDurationProperty);
        EditorGUILayout.PropertyField(sampleAllProperty);

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
