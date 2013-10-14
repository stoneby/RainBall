using System;
using UnityEngine;

public class BoomingState : IState
{
    public event EventHandler<EventArgs> End;
    public bool Pass { get; set; }

    public void Go()
    {
        var shooter = Utils.Shooter;
        var hittingBall = shooter.HittingBall;
        if (hittingBall == null)
        {
            return;
        }

        Debug.Log("Current hitting ball: " + hittingBall.name);
        
        //BoomingOccuring();

        // [TODO] Correct the hit point. (may overlap this time two balls)
        // [TODO] Check shoot ball join before or after current ball.
        // [FIXME] Shoot ball join after the current ball.
        var currentBallUpdater = hittingBall.GetComponent<BallUpdater>();
        var nextBall = currentBallUpdater.NextBall;
        // Break link between current ball and next ball.
        currentBallUpdater.NextBall = null;

        // handle shoot ball.
        var shootBall = shooter.ShootBall;
        var shooterBallUpdater = shootBall.GetComponent<BallUpdater>();
        shooterBallUpdater.Set(currentBallUpdater);
        shooterBallUpdater.Index = currentBallUpdater.Index;
        shootBall.transform.parent = hittingBall.transform.parent;
        shootBall.layer = hittingBall.layer;

        var ballList = Utils.BallManager.BallUpdaterList;
        // update ball list.
        ballList.Insert((nextBall == null) ? ballList.Count : (nextBall.GetComponent<BallUpdater>().Index), shooterBallUpdater);

        // update following balls' index.
        for (var index = currentBallUpdater.Index + 1; index < ballList.Count; ++index)
        {
            ++ballList[index].Index;
            ballList[index].name = ballList[index].Name;
        }

        Debug.Log("Insert ball " + shooterBallUpdater.name + ", to index: " + shooterBallUpdater.Index);

        // get last ball's final position.
        Utils.BallManager.MoveDirection = MoveDirection.Backward;
        var lastBall = (nextBall == null) ? ballList[ballList.Count - 2] : ballList[ballList.Count - 1];
        var finalBallNodeList = Utils.TrimPath(Utils.LevelManager.GetPathNodes(), lastBall.transform.position,
                                               MoveDirection.Backward, Utils.BallManager.Diameter);
        var finalBallPosition = finalBallNodeList[finalBallNodeList.Count - 1];

        for (var i = shooterBallUpdater.Index; i < ballList.Count - 1; ++i)
        {
            Utils.MoveDirectly(ballList[i].gameObject, ballList[i + 1].transform.position, Utils.Settings.InsertSpeed);
        }
        Utils.MoveDirectly(ballList[ballList.Count - 1].gameObject, finalBallPosition, Utils.Settings.InsertSpeed);

        // update next ball's linkage.
        if (nextBall)
        {
            var nextBallUpdater = nextBall.GetComponent<BallUpdater>();
            nextBallUpdater.LastBall = null;
        }

        End(this, new EventArgs());
    }
}
