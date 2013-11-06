using System.Collections;
using UnityEngine;

public class ShootingState : AbstractState
{
    public float Speed;

    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        Utils.Shooter.GenerateBall();

        Utils.GameData.Next();
        var index = Utils.GameData.ShootBallList[Utils.GameData.ShootBallIndex];
        Utils.Shooter.HittingBall = Utils.BallManager.BallUpdaterList[index].gameObject;
        
        Debug.Log("Shooting to ball: " + index + ", hitting ball: " + Utils.Shooter.HittingBall.name);

        var targetPosition = Utils.Shooter.HittingBall.transform.position;
        var shooterPosition = new Vector3(Utils.Shooter.ShootBall.transform.position.x,
                                          Utils.Shooter.HittingBall.transform.position.y,
                                          Utils.Shooter.ShootBall.transform.position.z);
        var direction = Vector3.Normalize(targetPosition - shooterPosition);
        do
        {
            var distance = Mathf.Abs(Vector3.Distance(shooterPosition, targetPosition));
            if (distance <= Utils.BallManager.Diameter)
            {
                break;
            }

            var deta = direction * Time.deltaTime * Speed;
            Utils.Shooter.ShootBall.transform.position += deta;
            shooterPosition = new Vector3(Utils.Shooter.ShootBall.transform.position.x,
                                          Utils.Shooter.HittingBall.transform.position.y,
                                          Utils.Shooter.ShootBall.transform.position.z);
            yield return null;
        } while (true);

        OnEnd();
    }
}
