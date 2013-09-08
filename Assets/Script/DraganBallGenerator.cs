using UnityEngine;

public class DraganBallGenerator : MonoBehaviour
{
    public GameObject CommanderBall;

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
            var child = transform.GetChild(i);
            if (child.gameObject == CommanderBall)
            {
                Destroy(CommanderBall.GetComponent<BallUpdater>());
            }
            else
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public void Generate()
    {
        DestroyChildren();

        var path = iTweenEvent.GetEvent(CommanderBall, "Move").Values["path"] as string;
        CommanderBall.transform.position = iTweenPath.GetPath(path)[0];

        for (var i = 0; i < Size; ++i)
        {
            GameObject currentBall = null;
            if (i == 0)
            {
                currentBall = CommanderBall;
            }
            else
            {
                currentBall = Instantiate(KeyBall, CommanderBall.transform.position, CommanderBall.transform.rotation) as GameObject;
                currentBall.transform.parent = CommanderBall.transform.parent;
                //newBall.transform.position = CommanderBall.transform.position;
                var index = Random.Range(0, Utils.Settings.ColorList.Count);
                currentBall.renderer.material.color = Utils.Settings.ColorList[index];
                currentBall.name = "Ball_" + (i + 1) + "_Color_" + index;
            }

            var ballUpdater = currentBall.AddComponent<BallUpdater>();
            ballUpdater.Index = i;
            Utils.BallManager.BallUpdaterList.Add(ballUpdater);

            Debug.Log("Current ball: " + currentBall.name + " adding to dragan ball manager.");
        }

        var manager = Utils.BallManager.BallUpdaterList;
        var size = manager.Count;
        // handle link relationship between balls.
        for (var i = 0; i < size; ++i)
        {
            if (i == 0)
            {
                manager[i].NextBall = manager[i + 1].gameObject;
            }
            else if (i == Utils.BallManager.BallUpdaterList.Count - 1)
            {
                manager[i].LastBall = manager[i - 1].gameObject;
            }
            else
            {
                manager[i].NextBall = manager[i + 1].gameObject;
                manager[i].LastBall = manager[i - 1].gameObject;
            }
        }
    }
}
