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
        Utils.BallGenerator.Generate();

        // wait one frame to destroy original balls.
        yield return null;

        var leaderBall = Utils.BallManager.BallUpdaterList[0];
        Utils.MoveLevel(leaderBall.gameObject, Utils.Settings.MoveSpeed);

		// Decrease Credit number and clear up Paid number in Menu
        Utils.CreditBar.Bet();
	}

    private void OnStopMoving(object sender, BallMoveArgs args)
    {
        if (args.IsLevelMoving)
        {
            OnEnd();
        }
    }

    protected override void Start()
    {
        base.Start();

        Utils.BallManager.StopMoving += OnStopMoving;
    }
}
