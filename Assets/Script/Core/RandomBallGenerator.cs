using UnityEngine;

public class RandomBallGenerator : AbstractBallGenerator
{
    public override BallUpdater Generator()
    {
        var color = Random.Range(0, Utils.Settings.TotalColorCount);
        return GenerateBallByColor(color);
    }
}
