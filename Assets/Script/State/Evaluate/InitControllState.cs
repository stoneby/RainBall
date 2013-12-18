using UnityEngine;

public class InitControllState : AbstractState
{
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        // update actual shoot ball index due to ball has been shooted into the original list.
        var shootBallIndex = Utils.GameDataManager.CurrentShootChain[Utils.GameDataManager.ShootBallIndex].Location + Utils.GameDataManager.ShootBallIndex;
        if (shootBallIndex < Utils.BallManager.BallUpdaterList.Count)
        {
            Utils.Shooter.ShootBall = Utils.BallManager.BallUpdaterList[shootBallIndex].gameObject;
        }
        ++Utils.GameDataManager.ShootBallIndex;

        OnEnd();
    }
}
