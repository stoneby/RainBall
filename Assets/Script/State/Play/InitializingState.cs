using System.Collections;
using UnityEngine;

public class InitializingState : AbstractState
{
    // Use this for initialization
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        Utils.Shooter.ShootBehaviour.ShootEndEnabled = true;

        StartCoroutine("DoGo");
    }

    IEnumerator DoGo()
    {
        Utils.BallChainManager.StopMoving += OnStopMoving;

        Utils.BallChainGenerator.Generate();

        // wait one frame to destroy original balls.
        yield return null;

        var leaderBall = Utils.BallChainManager.BallUpdaterList[0];
        Utils.MoveLevel(leaderBall.gameObject, Utils.Settings.MoveSpeed);

		// Decrease Credit number and clear up Paid number in Menu
        Utils.CreditBar.Bet();
	}

    private void OnStopMoving(object sender, BallMoveArgs args)
    {
        Utils.BallChainManager.StopMoving -= OnStopMoving;

        if(args.IsLevelMoving)
        {
            OnEnd();
        }
    }
}
