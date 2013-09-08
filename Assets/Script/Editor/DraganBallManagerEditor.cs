using UnityEditor;

[CustomEditor(typeof(DraganBallManager))]
public class DraganBallManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var manager = target as DraganBallManager;
        manager.MoveDirection = (MoveDirection)EditorGUILayout.EnumPopup("Move Direction", manager.MoveDirection);
        manager.IntersectFactor = EditorGUILayout.Slider("Intersect Factor", manager.IntersectFactor, 0f, 1f);
        manager.DistanceFactor = EditorGUILayout.Slider("Distance Factor", manager.DistanceFactor, 0.8f, 1.2f);
    }
}
