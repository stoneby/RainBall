using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public GameObject Parent;

    public GameObject KeyBall;

    public List<GameObject> TemplateBallList;

    public int Size;

    void Awake()
    {
        if(TemplateBallList == null || TemplateBallList.Count != Utils.Settings.TotalColorCount)
        {
            Debug.LogError("Please double check whether template ball list has been attached or " +
                           "the count is not equal to total color count according to globel game settings.");

            return;
        }

        // sphear collider, which x and z equals diameter in our case.
        Utils.BallManager.Diameter = TemplateBallList[0].collider.bounds.size.x;
    }

    void Start()
    {
        Generate();
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

        var startLocation = iTweenPath.GetPath(Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path)[0];

        var ballList = Utils.BallManager.BallUpdaterList;
        ballList.Clear();
        for (var i = 0; i < Size; ++i)
        {
            var index = Random.Range(0, Utils.Settings.TotalColorCount);
            var currentBall = Instantiate(TemplateBallList[index], startLocation, Quaternion.identity) as GameObject;
            currentBall.transform.parent = Parent.transform;

            var ballUpdater = currentBall.AddComponent<BallUpdater>();
            ballUpdater.Index = i;
            ballUpdater.Color = index;
            currentBall.name = ballUpdater.Name;
            ballList.Add(ballUpdater);

            Debug.Log("Current ball: " + currentBall.name + " adding to dragan ball manager.");
        }

        var size = ballList.Count;
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
