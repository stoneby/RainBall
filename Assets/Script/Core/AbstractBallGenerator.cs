using UnityEngine;

public abstract class AbstractBallGenerator : MonoBehaviour
{
    public int Index { get; set; }

    protected Vector3 StartLocation;

    public abstract BallUpdater Generator();

    public BallUpdater GenerateBallByColor(int color)
    {
        var currentBall =
            Instantiate(Utils.Settings.TemplateBallList[color], StartLocation,
                        Utils.Settings.TemplateBallList[color].transform.rotation) as
            GameObject;
        var ballUpdater = currentBall.AddComponent<BallUpdater>();
        ballUpdater.Color = color;
        currentBall.name = ballUpdater.Name;
        return ballUpdater;
    }

    void Start()
    {
        StartLocation = iTweenPath.GetPath(Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path)[0];
    }
}
