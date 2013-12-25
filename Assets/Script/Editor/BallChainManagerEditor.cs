using UnityEditor;

[CustomEditor(typeof(BallChainManager))]
public class BallChainManagerEditor : Editor
{
    private BallChainManager chainManager;

    void OnEnable()
    {
        chainManager = target as BallChainManager;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UpdateManager();
    }

    private void UpdateManager()
    {
        chainManager.MoveDirection = (MoveDirection) EditorGUILayout.EnumPopup("Move Direction", chainManager.MoveDirection);
        chainManager.IntersectFactor = EditorGUILayout.Slider("Intersect Factor", chainManager.IntersectFactor, 0f, 1f);
        chainManager.DistanceFactor = EditorGUILayout.Slider("Distance Factor", chainManager.DistanceFactor, 0.8f, 1.2f);
    }
}
