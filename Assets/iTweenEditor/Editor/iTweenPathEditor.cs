//by Bob Berkebile : Pixelplacement : http://www.pixelplacement.com

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(iTweenPath))]
public class iTweenPathEditor : Editor
{
    iTweenPath _target;
    GUIStyle style = new GUIStyle();
    public static int count = 0;


    void OnEnable()
    {
        //i like bold handle labels since I'm getting old:
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        _target = (iTweenPath)target;

        //Debug.Log("OnEnable before: path - " + _target.PathName + ", initialzied: " + _target.Initialized);
        //lock in a default path name:
        if (!_target.Initialized)
        {
            _target.Initialized = true;
            _target.PathName = "New Path " + ++count;
            _target.InitialName = _target.PathName;
        }
        //Debug.Log("OnEnable after: path - " + _target.PathName + ", initialzied: " + _target.Initialized);
    }

    public override void OnInspectorGUI()
    {
        //path name:
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Path Name");
        _target.PathName = EditorGUILayout.TextField(_target.PathName);
        EditorGUILayout.EndHorizontal();

        if (_target.PathName == "")
        {
            _target.PathName = _target.InitialName;
        }

        //path color:
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Path Color");
        _target.PathColor = EditorGUILayout.ColorField(_target.PathColor);
        EditorGUILayout.EndHorizontal();

        //exploration segment count control:
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Node Count");
        _target.NodeCount = Mathf.Clamp(EditorGUILayout.IntSlider(_target.NodeCount, 0, iTweenPath.MaxNodeCount), 0,
                                        iTweenPath.MaxNodeCount);
        EditorGUILayout.EndHorizontal();

        _target.ToogleX = EditorGUILayout.BeginToggleGroup("Fix X", _target.ToogleX);
        _target.FixPositionX = EditorGUILayout.FloatField("X", _target.FixPositionX);
        EditorGUILayout.EndToggleGroup();

        _target.ToogleY = EditorGUILayout.BeginToggleGroup("Fix Y", _target.ToogleY);
        _target.FixPositionY = EditorGUILayout.FloatField("Y", _target.FixPositionY);
        EditorGUILayout.EndToggleGroup();

        _target.ToogleZ = EditorGUILayout.BeginToggleGroup("Fix Z", _target.ToogleZ);
        _target.FixPositionZ = EditorGUILayout.FloatField("Z", _target.FixPositionZ);
        EditorGUILayout.EndToggleGroup();

        //add node?
        if (_target.NodeCount > _target.Nodes.Count)
        {
            for (int i = 0; i < _target.NodeCount - _target.Nodes.Count; i++)
            {
                var added = new Vector3((_target.ToogleX) ? _target.FixPositionX : 0, (_target.ToogleY) ? _target.FixPositionY : 0,
                                        (_target.ToogleZ) ? _target.FixPositionZ : 0);
                _target.Nodes.Add(added);
            }
        }

        //remove node?
        if (_target.NodeCount < _target.Nodes.Count)
        {
            if (EditorUtility.DisplayDialog("Remove path node?",
                                            "Shortening the node list will permantently destory parts of your path. This operation cannot be undone.",
                                            "OK", "Cancel"))
            {
                int removeCount = _target.Nodes.Count - _target.NodeCount;
                _target.Nodes.RemoveRange(_target.Nodes.Count - removeCount, removeCount);
            }
            else
            {
                _target.NodeCount = _target.Nodes.Count;
            }
        }

        //node display:
        EditorGUI.indentLevel = 4;
        for (int i = 0; i < _target.Nodes.Count; i++)
        {
            _target.Nodes[i] = EditorGUILayout.Vector3Field("Node " + (i + 1), _target.Nodes[i]);
        }

        //update and redraw:
        if (GUI.changed)
        {
            EditorUtility.SetDirty(_target);
        }
    }

    void OnSceneGUI()
    {
        if (_target.enabled)
        { // dkoontz
            if (_target.Nodes.Count > 0)
            {
                //allow path adjustment undo:
                Undo.SetSnapshotTarget(_target, "Adjust iTween Path");

                //path begin and end labels:
                Handles.Label(_target.Nodes[0], "'" + _target.PathName + "' Begin", style);
                Handles.Label(_target.Nodes[_target.Nodes.Count - 1], "'" + _target.PathName + "' End", style);

                //node handle display:
                for (int i = 0; i < _target.Nodes.Count; i++)
                {
                    _target.Nodes[i] = Handles.PositionHandle(_target.Nodes[i], Quaternion.identity);
                }
            }
        } // dkoontz
    }
}