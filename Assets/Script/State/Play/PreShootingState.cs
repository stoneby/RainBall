using UnityEngine;

public class PreShootingState : AbstractState
{
    // Use this for initialization
    public override void Go()
    {
        Utils.Shooter.HitBall = Utils.Shooter.HitBallList[Utils.GameDataManager.ShootBallIndex];

        Debug.Log("Shooting to ball: " + Utils.GameDataManager.CurrentShoot.Location + ", hitting ball: " +
                  Utils.Shooter.HitBall.name);

        OnEnd();
    }
}
