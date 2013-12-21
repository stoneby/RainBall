
public class RandomBallShow : MagicShow
{
    public override void ShowTime()
    {
        GenerateBalls();
    }

    private void GenerateBalls()
    {
        Parsers.ForEach(parser => parser.ReadFromFile(parser.SampleFile));

        Parsers.ForEach(parser => parser.PositionList.ForEach(ball =>
            {
                GenerateBall(parser.transform, ball);
            }));
    }
}
