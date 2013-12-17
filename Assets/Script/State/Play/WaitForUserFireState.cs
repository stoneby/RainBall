using System.Collections;
using UnityEngine;

public class WaitForUserFireState : AbstractState
{
    public GameObject BallPlane;

    private float rot;
    private float axisY;
    private Vector3 stackerPoint;

    public override void Go()
    {
        StartCoroutine("WaitForUserFire");
    }

    IEnumerator WaitForUserFire()
    {
        BallPlane.SetActive(true);

        do
        {
            if(Input.GetButton("Fire2"))
            {
                SetBallsPhysics(false);
                var direction = GetShootDirection();
                SetBallsPhysics(true);

                var ball = Utils.FindNearestByRay(stackerPoint, direction);
                if(ball != null)
                {
                    iTweenEvent.GetEvent(ball, "Fear").Play();
                }
            }
            else if(Input.GetButton("Fire1"))
            {
                rot = transform.rotation.eulerAngles.y;
                Debug.Log("euler angle is: " + rot);

                SetBallsPhysics(false);
                var direction = GetShootDirection();
                SetBallsPhysics(true);

                var ball = Utils.FindNearestByRay(stackerPoint, direction);
                Utils.Shooter.HitBall = ball.transform.parent.gameObject;

                Debug.Log("Hitting ball: " + ball);

                Exit();
                break;
            }
            yield return null;

        } while(true);
    }

    private void SetBallsPhysics(bool enable)
    {
        Utils.BallManager.BallUpdaterList.ForEach(ball =>
                                                      {
                                                          var layer = (enable)
                                                                          ? LayerMask.NameToLayer("Default")
                                                                          : LayerMask.NameToLayer("Ignore Raycast");
                                                          ball.gameObject.layer = layer;
                                                      });
    }

    private Vector3 GetShootDirection()
    {
        var direction = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.gameObject.name);
            Debug.Log(("hit piont: " + hit.point));
            direction = Vector3.Normalize(hit.point - stackerPoint);
        }
        return direction;
    }

    protected override void Awake()
    {
        base.Awake();

        var templateBall = Utils.BallGenerator.TemplateBallList[0];
        axisY = templateBall.transform.position.y;
        // make sure the y axis is the same as all balls in.
        stackerPoint = new Vector3(gameObject.transform.position.x, axisY, gameObject.transform.position.z);
    }

    private void Exit()
    {
        StopCoroutine("WaitForUserFire");

        OnEnd();
    }
}
