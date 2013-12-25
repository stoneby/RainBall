using UnityEngine;

public class InitControllState : AbstractState
{
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        Utils.Shooter.ShootBall = Utils.Shooter.ShootBallList[Utils.GameDataManager.ShootBallIndex];

        ++Utils.GameDataManager.ShootBallIndex;

        OnEnd();
    }
}
