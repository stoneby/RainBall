using System.Collections;
using UnityEngine;

public class ShootingState : AbstractState
{
    public float EruptHeight;
    public float EruptTime;

    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        Utils.Shooter.GenerateBall();

        // need to update hitting ball index according to shoot ball shooting into it.
        var index = Utils.GameData.ShootBallList[Utils.GameData.ShootBallIndex] + Utils.GameData.ShootBallIndex;
        Utils.Shooter.HittingBall = Utils.BallManager.BallUpdaterList[index].gameObject;

        Debug.Log("Shooting to ball: " + index + ", hitting ball: " + Utils.Shooter.HittingBall.name);

        var targetPosition = Utils.Shooter.HittingBall.transform.position;
        var ball = Utils.Shooter.ShootBall.transform.GetChild(0).gameObject;

        iTween.MoveBy(ball, iTween.Hash("z", EruptHeight, "time", EruptTime / 2, "easeType", iTween.EaseType.easeOutQuad));
        iTween.MoveBy(ball,
                      iTween.Hash("z", -EruptHeight, "time", EruptTime / 2, "delay", EruptTime / 2, "easeType",
                                  iTween.EaseType.easeInCubic));
        iTween.MoveTo(Utils.Shooter.ShootBall,
                      iTween.Hash("position", targetPosition, "time", EruptTime, "easeType", iTween.EaseType.linear));

        yield return new WaitForSeconds(EruptTime);

        OnEnd();
    }
}
