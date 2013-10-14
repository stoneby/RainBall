using System;
using UnityEngine;

public class EvaluateBoomState : IState
{
    public event EventHandler<EventArgs> End;
    public bool Pass { get; set; }

    public void Go()
    {
        var shooterBall = Utils.Shooter.ShootBall;
        // Collision occurs.
        var shooterPoint = new Vector3(shooterBall.transform.position.x, Utils.Shooter.KeyBall.transform.position.y,
                               shooterBall.transform.position.z);
        var currentBall = Utils.FindNearestBySphear(shooterPoint, Utils.BallManager.Diameter / 2, Utils.Shooter.ShootDirection);
        Pass = (currentBall != null);

        End(this, new EventArgs());
    }
}
