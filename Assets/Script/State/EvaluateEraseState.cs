
using UnityEngine;

public class EvaluateEraseState : AbstractState
{
    public const int BonusMinCounter = 3;

    public override void Go()
    {
        var ballList = Utils.BallManager.BallUpdaterList;
        if (ballList.Count == 0)
        {
            Debug.Log(GetType().Name + ", ball list is empty, you erase all balls, dude.");

            Pass = false;
            OnEnd();
            return;
        }

        var shootBall = Utils.Shooter.ShootBall;
        if (shootBall == null)
        {
            Debug.LogError(GetType().Name + ", shoot ball should not be null.");

            Pass = false;
            OnEnd();
            return;
        }

        var currentBallUpdater = shootBall.GetComponent<BallUpdater>();
        var counter = 1;
        var index = currentBallUpdater.Index;
        for (var i = index - 1; i >= 0; --i)
        {
            Debug.Log("checking ball: " + ballList[i].name + ", index:" + i);
            if (ballList[i].Color != currentBallUpdater.Color)
            {
                break;
            }
            ++counter;
        }
        var startIndex = index - counter + 1;
        for (var i = index + 1; i < ballList.Count; ++i)
        {
            Debug.Log("checking ball: " + ballList[i].name + ", index:" + i);
            if (ballList[i].Color != currentBallUpdater.Color)
            {
                break;
            }
            ++counter;
        }

        if (counter >= BonusMinCounter)
        {
            Pass = true;

            Utils.Evaluator.Check(currentBallUpdater.Color, counter, startIndex);
            // reset shoot ball to ball located on start index for checking loop.
            Utils.Shooter.ShootBall = (startIndex != 0)
                                          ? ballList[startIndex - 1].gameObject
                                          : (startIndex + counter != ballList.Count - 1)
                                                ? ballList[startIndex + counter].gameObject
                                                : null;
        }
        else
        {
            Pass = false;
        }

        Debug.Log(GetType().Name + ", counter is : " + counter + ", passed: " + Pass);

        OnEnd();
    }
}
