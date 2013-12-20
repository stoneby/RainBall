using System.Collections.Generic;
using UnityEngine;

public class BallByBallGenerator : MonoBehaviour
{
    public string InputPath;

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
        var ballController = Utils.BallGenerator.TemplateBallList[0].GetComponent<BallController>();
        distance = ballController.Ball.GetComponent<SphereCollider>().radius * 2;
    }

    public void Generate()
    {
        ballBallList.Clear();

        var nodeList = iTweenPath.GetPath(InputPath);
        var currentLoc = nodeList[0];
        var lastLoc = nodeList[nodeList.Length - 1];
        var counter = 0;
        do
        {
            if(Vector3.Distance(currentLoc, lastLoc) < 0.1f || counter > 100)
            {
                break;
            }
            ballBallList.Add(currentLoc);
            Debug.Log("Add location: " + currentLoc);

            var newPath = Utils.TrimPath(nodeList, currentLoc, MoveDirection.Forward, distance);
            var newLoc = newPath[newPath.Count - 1];
            currentLoc = newLoc;
            counter++;
        } while(true);
    }
}
