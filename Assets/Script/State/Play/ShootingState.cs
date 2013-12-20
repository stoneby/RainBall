using System;
using UnityEngine;

public class ShootingState : AbstractState
{
    public ParticleSystem Burning;

    private ParticleSystem burningInstance;

    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        Shoot();
    }

    private void Shoot()
    {
        // need to update hitting ball index according to shoot ball shooting into it.
        var index = Utils.GameDataManager.CurrentShootChain[Utils.GameDataManager.ShootBallIndex].Location + Utils.GameDataManager.ShootBallIndex;
        Utils.Shooter.HitBall = Utils.BallManager.BallUpdaterList[index].gameObject;

        Debug.Log("Shooting to ball: " + index + ", hitting ball: " + Utils.Shooter.HitBall.name);

        Utils.Shooter.GenerateBall();
        Utils.Shooter.Shoot();
        var shootBallController = Utils.Shooter.ShootBall.GetComponent<BallController>();
        shootBallController.StartBurning();
    }

    private void OnEndShoot(object sender, EventArgs args)
    {
        OnEnd();
    }

    protected override void Awake()
    {
        base.Awake();

        Utils.Shooter.ShootBehaviour.ShootEnd += OnEndShoot;
    }
}
