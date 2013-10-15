using UnityEditor;

[CustomEditor(typeof(BallManager))]
public class BallManagerEditor : Editor
{
    void OnEnable()
    {
        UpdateManager();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UpdateManager();
    }

    private void UpdateManager()
    {
        var manager = target as BallManager;
        manager.MoveDirection = (MoveDirection) EditorGUILayout.EnumPopup("Move Direction", manager.MoveDirection);
        manager.IntersectFactor = EditorGUILayout.Slider("Intersect Factor", manager.IntersectFactor, 0f, 1f);
        manager.DistanceFactor = EditorGUILayout.Slider("Distance Factor", manager.DistanceFactor, 0.8f, 1.2f);
    }
}
