
public class RandomShootShow : MagicShow
{
    public Shooter Shooter;

    public override void ShowTime()
    {
        Parsers.ForEach(parser => parser.ReadFromFile(parser.SampleFile));
        Parsers.ForEach(parser => parser.PositionList.ForEach(position =>
            {
                Shooter.ShootBall = GenerateBall(Shooter.transform, Shooter.transform.position);
                Shooter.HitBall = GenerateBall(parser.transform, position);
                Shooter.HitBall.SetActive(false);
                Shooter.Shoot();
            }));
    }


}
