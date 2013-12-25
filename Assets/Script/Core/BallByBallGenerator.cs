using System.Collections.Generic;
using UnityEngine;

public class BallByBallGenerator : MonoBehaviour
{
    public GameObject TemplateBall;
    public PathParser Parser;

    private List<Vector3> ballBallList;
    private float distance;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if(ballBallList == null)
        {
            ballBallList = new List<Vector3>();
        }
        var ballController = TemplateBall.GetComponent<BallController>();
        distance = ballController.Diameter;
    }

    public void Generate()
    {
        Parser.ReadFromFile(Parser.SampleFile);
        ballBallList.Clear();

        var nodeList = Parser.PositionList;
        var currentLoc = nodeList[0];
        do
        {
            ballBallList.Add(currentLoc);
            Debug.Log("Add location: " + currentLoc);

            var newPath = Utils.TrimPath(nodeList, currentLoc, MoveDirection.Forward, distance);
            if (newPath == null)
            {
                break;
            }
            var newLoc = newPath[newPath.Count - 1];
            currentLoc = newLoc;
        } while(true);
        Parser.PositionList.Clear();
        Parser.PositionList.AddRange(ballBallList);
        Parser.WriteToFile(Parser.SampleFile.name + "_BallByBall.txt");
    }
}
