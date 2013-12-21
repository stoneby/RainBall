using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public GameObject Parent;
    public List<GameObject> TemplateBallList;

    void Awake()
    {
        if(TemplateBallList == null || TemplateBallList.Count != Utils.Settings.TotalColorCount)
        {
            Debug.LogError("Please double check whether template ball list has been attached or " +
                           "the count is not equal to total color count according to globel game settings.");

            return;
        }

        // sphear collider, which x and z equals diameter in our case.
        var ballController = Utils.BallGenerator.TemplateBallList[0].GetComponent<BallController>();
        Utils.BallManager.Diameter = ballController.Diameter;
    }

    void Start()
    {
    }

    public void Cleanup()
    {
        for (var i = 0; i < transform.childCount; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void Generate()
    {
        Cleanup();

        GenerateBalls();
    }

    private void GenerateBalls()
    {
        var startLocation = iTweenPath.GetPath(Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path)[0];
        var size = Utils.Settings.TotalBallCount;

        var ballList = Utils.BallManager.BallUpdaterList;
        ballList.Clear();
        for (var i = 0; i < size; ++i)
        {
            var index = Utils.GameDataManager.CurrentBallChain[i];
            var currentBall =
                Instantiate(TemplateBallList[index], startLocation, TemplateBallList[index].transform.rotation) as
                GameObject;
            currentBall.transform.parent = Parent.transform;

            var ballUpdater = currentBall.AddComponent<BallUpdater>();
            ballUpdater.Index = i;
            ballUpdater.Color = index;
            currentBall.name = ballUpdater.Name;
            ballList.Add(ballUpdater);

            //Debug.Log("Current ball: " + currentBall.name + " adding to dragan ball manager.");
        }

        // handle link relationship between balls.
        for (var i = 0; i < size; ++i)
        {
            if (i == 0)
            {
                ballList[i].NextBall = ballList[i + 1].gameObject;
                ballList[i].LastBall = null;
            }
            else if (i == ballList.Count - 1)
            {
                ballList[i].NextBall = null;
                ballList[i].LastBall = ballList[i - 1].gameObject;
            }
            else
            {
                ballList[i].NextBall = ballList[i + 1].gameObject;
                ballList[i].LastBall = ballList[i - 1].gameObject;
            }
        }
    }
}
