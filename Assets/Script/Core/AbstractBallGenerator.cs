using UnityEngine;

public abstract class AbstractBallGenerator : MonoBehaviour
{
    public int Index { get; set; }

    public Vector3 Position;

    public abstract BallUpdater Generator();

    public BallUpdater GenerateBallByColor(int color)
    {
        var currentBall =
            Instantiate(Utils.Settings.TemplateBallList[color], Position,
                        Utils.Settings.TemplateBallList[color].transform.rotation) as
            GameObject;
        var ballUpdater = currentBall.AddComponent<BallUpdater>();
        ballUpdater.Color = color;
        currentBall.name = ballUpdater.Name;
        return ballUpdater;
    }

    protected void Start()
    {
    }
}
