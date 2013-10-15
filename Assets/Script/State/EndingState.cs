using UnityEngine;

public class EndingState : AbstractState
{
    public override void Go()
    {
        var ballList = Utils.BallManager.BallUpdaterList;
        if (ballList.Count == 0)
        {
            Debug.Log("We meet super big celebration hit, all balls are gone.");
        }

        OnEnd();
    }
}
