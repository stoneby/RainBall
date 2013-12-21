using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public GameObject PathParent;

    public PathParser Parser;

    public bool GenerateTweenPath(TextAsset textAsset)
    {
        if (!Parser.ReadFromFile(textAsset))
        {
            return false;
        }

        ConfigureTweenPath(textAsset);
        return true;
    }

    private void ConfigureTweenPath(TextAsset textAsset)
    {
        var pathName = LevelManager.GetDetailPath(textAsset.name);

        var pathLength = iTween.PathLength(Parser.PositionList.ToArray());
        var ballController = Utils.BallGenerator.TemplateBallList[0].GetComponent<BallController>();
        var nodeCount = (int)(pathLength / ballController.Diameter);
        nodeCount = (nodeCount >= iTweenPath.MaxNodeCount) ? iTweenPath.MaxNodeCount : nodeCount;

        Debug.Log("Generate path: name - " + pathName + ", length - " + pathLength + ", detail node count - " + nodeCount);

        var pathObject = new GameObject(pathName);
        pathObject.transform.parent = PathParent.transform;
        pathObject.layer = PathParent.layer;
        var trimmedNodeList = Utils.TrimList(Parser.PositionList, nodeCount);
        Utils.ConfigureTweenPath(pathObject, trimmedNodeList, pathName);
        Parser.PositionList = trimmedNodeList;
        Parser.WriteToFile(pathName + ".txt");
        Debug.Log("Write to file: " + pathName + ".txt");
    }
}
