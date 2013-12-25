
public class ShootBallGenerator : AbstractBallGenerator {

	// Use this for initialization
    public override BallUpdater Generator()
    {
        var color = Utils.GameDataManager.CurrentShootChain[Utils.GameDataManager.ShootBallIndex].Color;
        return GenerateBallByColor(color);
    }
}
