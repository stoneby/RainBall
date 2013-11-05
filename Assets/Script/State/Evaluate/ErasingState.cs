
using System.Collections;
using UnityEngine;

public class ErasingState : AbstractState
{
    public float MovingDuration;

    private Vector3 endLocation;

    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        Erase();

        Move();
    }

    private void Erase()
    {
        var ballList = Utils.BallManager.BallUpdaterList;
        var awardInfor = Utils.Evaluator.CurrentAwardInfor;

        var endBall = ballList[awardInfor.StartIndex];

        Debug.Log("Ending ball: " + endBall.name);

        endLocation = endBall.transform.position;

        for (var i = awardInfor.StartIndex; i < awardInfor.StartIndex + awardInfor.Award.Num; ++i)
        {
            var ball = ballList[i].gameObject;
            Destroy(ball);
        }

        ballList.RemoveRange(awardInfor.StartIndex, awardInfor.Award.Num);

        Debug.Log("Erasing ball number of " + awardInfor.Award.Num + ", starting with index of " + awardInfor.StartIndex);

        for (var i = 0; i < ballList.Count; ++i)
        {
            ballList[i].Index = i;
            ballList[i].name = ballList[i].Name;
        }

        if (ballList.Count == 0)
        {
            return;
        }
        // build relationship after erasing.
        if (awardInfor.StartIndex == ballList.Count)
        {
            ballList[awardInfor.StartIndex - 1].NextBall = null;
        }
        else if (awardInfor.StartIndex == 0)
        {
            ballList[awardInfor.StartIndex].LastBall = null;
        }
        else
        {
            ballList[awardInfor.StartIndex - 1].NextBall = ballList[awardInfor.StartIndex].gameObject;
            ballList[awardInfor.StartIndex].LastBall = ballList[awardInfor.StartIndex - 1].gameObject;
        }
    }

    private void Move()
    {
        var awardInfor = Utils.Evaluator.CurrentAwardInfor;
        if (awardInfor.StartIndex >= Utils.BallManager.BallUpdaterList.Count)
        {
            OnEnd();
            return;
        }

        Utils.BallManager.MoveDirection = MoveDirection.Forward;

        var moveObject = Utils.BallManager.BallUpdaterList[awardInfor.StartIndex].gameObject;
        Utils.BallManager.InsertTrackingTail(awardInfor.StartIndex, Utils.BallManager.MoveDirection);
        Utils.ConfigureTweenPath(moveObject, moveObject.transform.position, endLocation);
        Utils.MovePartical(moveObject, Utils.Settings.BoomMovingSpeed);

        Debug.Log("Moving ball index of " + awardInfor.StartIndex);

        MoveComplete();
    }

    private void MoveComplete()
    {
        StartCoroutine(DoMoveComplete());
    }

    IEnumerator DoMoveComplete()
    {
        yield return new WaitForSeconds(MovingDuration);

        OnEnd();
    }
}
