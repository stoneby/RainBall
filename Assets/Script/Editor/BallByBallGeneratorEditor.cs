using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BallByBallGenerator))]
public class BallByBallGeneratorEditor : Editor
{
    private BallByBallGenerator generator;

    void OnEnable()
    {
        generator = target as BallByBallGenerator;
        generator.Init();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Generator"))
        {
            generator.Generate();
        }
    }
}
