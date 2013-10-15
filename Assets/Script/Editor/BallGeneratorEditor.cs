using UnityEditor;

[CustomEditor(typeof (BallGenerator))]
public class BallGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
