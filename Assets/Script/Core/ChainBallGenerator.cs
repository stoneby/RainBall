
public class ChainBallGenerator : AbstractBallGenerator
{
    public override BallUpdater Generator()
    {
        var color = Utils.GameDataManager.CurrentBallChain[Index];
        return GenerateBallByColor(color);
    }
}
