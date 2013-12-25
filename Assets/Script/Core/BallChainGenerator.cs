using UnityEngine;

public class BallChainGenerator : MonoBehaviour
{
    public GameObject Parent;
    public AbstractBallGenerator BallGenerator;

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
        var size = Utils.Settings.TotalBallCount;

        var ballList = Utils.BallChainManager.BallUpdaterList;
        ballList.Clear();
        for (var i = 0; i < size; ++i)
        {
            BallGenerator.Index = i;
            var ballUpdater = BallGenerator.Generator();
            ballUpdater.Index = i;
            ballUpdater.transform.parent = Parent.transform;
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
