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
        ballBallList = Utils.MakePathEqually(Parser.PositionList, distance);

        Parser.PositionList.Clear();
        Parser.PositionList.AddRange(ballBallList);
        Parser.WriteToFile(Parser.SampleFile.name + "_BallByBall.txt");
    }
}
