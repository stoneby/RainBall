using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (DraganBallGenerator))]
public class DraganBallGeneratorEditor : Editor
{
    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
