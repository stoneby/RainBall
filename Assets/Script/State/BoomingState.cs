using System.Collections;
using UnityEngine;

public class BoomingState : AbstractState
{
    public float BoomedDuration = 1f;

    public override void Go()
    {
        var shooter = Utils.Shooter;
        var hittingBall = shooter.HittingBall;
        if (hittingBall == null)
        {
            Debug.LogError(GetType().Name + ", hitting ball should not be null.");

            OnEnd();
            return;
        }

        Debug.Log("Current hitting ball: " + hittingBall.name);
        
        // [TODO] Correct the hit point. (may overlap this time two balls)
        // [TODO] Check shoot ball join before or after current ball.
        // [FIXME] Shoot ball join after the current ball.
        var currentBallUpdater = hittingBall.GetComponent<BallUpdater>();
        var nextBall = currentBallUpdater.NextBall;
        // handle shoot ball.
        var shootBall = shooter.ShootBall;
        var shootBallUpdater = shootBall.GetComponent<BallUpdater>();
        shootBallUpdater.Set(currentBallUpdater);
        shootBallUpdater.Index = currentBallUpdater.Index;
        shootBall.transform.parent = hittingBall.transform.parent;
        shootBall.layer = hittingBall.layer;

        var ballList = Utils.BallManager.BallUpdaterList;
        // update ball list.
        ballList.Insert((nextBall == null) ? ballList.Count : (nextBall.GetComponent<BallUpdater>().Index), shootBallUpdater);

        // update following balls' index.
        for (var index = currentBallUpdater.Index + 1; index < ballList.Count; ++index)
        {
            ++ballList[index].Index;
            ballList[index].name = ballList[index].Name;
        }

        Debug.Log("Insert ball " + shootBallUpdater.name + ", to index: " + shootBallUpdater.Index);

        // get last ball's final position.
        var lastBall = (nextBall == null) ? ballList[ballList.Count - 2] : ballList[ballList.Count - 1];
        var finalBallNodeList = Utils.TrimPath(Utils.LevelManager.GetPathNodes(), lastBall.transform.position,
                                               MoveDirection.Backward, Utils.BallManager.Diameter);
        var finalBallPosition = finalBallNodeList[finalBallNodeList.Count - 1];

        for (var i = shootBallUpdater.Index; i < ballList.Count - 1; ++i)
        {
            Utils.MoveDirectly(ballList[i].gameObject, ballList[i + 1].transform.position, Utils.Settings.InsertSpeed);
        }
        Utils.MoveDirectly(ballList[ballList.Count - 1].gameObject, finalBallPosition, Utils.Settings.InsertSpeed);

        // update balls' linkage.
        currentBallUpdater.NextBall = shootBall;
        shootBallUpdater.LastBall = currentBallUpdater.gameObject;
        if (nextBall)
        {
            var nextBallUpdater = nextBall.GetComponent<BallUpdater>();
            nextBallUpdater.LastBall = shootBall;
            shootBallUpdater.NextBall = nextBall;
        }

        BoomingEnding();
    }

    private void BoomingEnding()
    {
        StartCoroutine(DoBoomingEnding());
    }

    IEnumerator DoBoomingEnding()
    {
        yield return new WaitForSeconds(BoomedDuration);

        OnEnd();
    }
}
