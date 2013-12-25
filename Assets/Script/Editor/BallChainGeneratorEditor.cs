using UnityEditor;

[CustomEditor(typeof (BallChainGenerator))]
public class BallChainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
