using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public GameObject Parent;

    public GameObject KeyBall;

    public int Size;

    void Start()
    {
        Generate();
    }

    private void DestroyChildren()
    {
        for (var i = 0; i < transform.childCount; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void Generate()
    {
        DestroyChildren();

        var startLocation = iTweenPath.GetPath(Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path)[0];

        var ballList = Utils.BallManager.BallUpdaterList;
        ballList.Clear();
        for (var i = 0; i < Size; ++i)
        {
            var currentBall = Instantiate(KeyBall, startLocation, Quaternion.identity) as GameObject;
            currentBall.transform.parent = Parent.transform;
            var index = Random.Range(0, Utils.Settings.ColorList.Count);
            currentBall.renderer.material.color = Utils.Settings.ColorList[index];

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
