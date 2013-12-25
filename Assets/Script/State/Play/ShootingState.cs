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
        Utils.Shooter.GenerateBall();
        Utils.Shooter.Shoot();
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
