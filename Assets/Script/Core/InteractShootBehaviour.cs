using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class InteractShootBehaviour : AbstractShootBehaviour
{
    public float Speed = 1f;

    public float LifeTime = 5f;

    private Vector3 ShootDirection { get; set; }

    public EventHandler<EventArgs> BoomingOccuringEvent;
    public EventHandler<EventArgs> BoomingEndingEvent;

    public bool Enabled { get; set; }

    private bool shooting;
    private bool canShoot = true;
    private float rot;
    private float axisY;
    private Vector3 stackerPoint;

    private GameObject shootBall;
    private GameObject hitBall;

    private IEnumerator StartShoot()
    {
        var nodesList = iTweenPath.GetPath("Level_1");
        var index = Random.Range(0, nodesList.Length);
        var targetPosition = nodesList[index];
        var direction = Vector3.Normalize(targetPosition - transform.position);

        var currentBall = Utils.FindNearestBySphear(targetPosition, Utils.BallManager.Diameter / 2, direction);
        if(currentBall != null)
        {
            hitBall = currentBall;

            Utils.ShootStateMachine.Reset();
            Utils.ShootStateMachine.Go();

            StopCoroutine("StartShoot");
        }

        yield return new WaitForSeconds(LifeTime);

        Utils.ShootStateMachine.End();

        shooting = false;
        canShoot = true;

        Reset(true);
    }

    private void Reset(bool destroy)
    {
        if(destroy)
        {
            Destroy(shootBall);
        }
        Utils.Shooter.GenerateBall();
    }

    private void Start()
    {
        var templateBall = Utils.BallGenerator.TemplateBallList[0];
        axisY = templateBall.transform.position.y;
        // make sure the y axis is the same as all balls in.
        stackerPoint = new Vector3(gameObject.transform.position.x, axisY, gameObject.transform.position.z);
    }

    private void OnShootStart(object sender, EventArgs args)
    {
        shooting = false;
    }

    private void OnShootStop(object sender, EventArgs args)
    {
        canShoot = true;
        Reset(false);
    }

    void Awake()
    {
        Utils.ShootStateMachine.GoStart += OnShootStart;
        Utils.ShootStateMachine.GoStop += OnShootStop;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Enabled)
        {
            return;
        }

        var mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                                       (Camera.main.transform.position.y - transform.position.y)));
        var direction = Vector3.Normalize(mouseWorldPosition - transform.position);
        transform.rotation = Quaternion.FromToRotation(Vector3.left, direction);

        if(Input.GetButton("Fire2"))
        {
            var ball = Utils.FindNearestByRay(stackerPoint, direction);
            if(ball != null)
            {
                iTweenEvent.GetEvent(ball, "Fear").Play();
            }
        }

        if(canShoot && !shooting && Input.GetButton("Fire1"))
        {
            shooting = true;
            canShoot = false;

            rot = transform.rotation.eulerAngles.y;
            Debug.Log("euler angle is: " + rot);

            ShootDirection = direction;
            StartCoroutine("StartShoot");
        }

        if(shooting)
        {
            var deta = ShootDirection * Time.deltaTime * Speed;
            shootBall.transform.position += deta;

            Debug.DrawRay(shootBall.transform.position, direction * Utils.BallManager.Diameter / 2, Color.red);

            var shooterPoint = new Vector3(shootBall.transform.position.x, axisY, shootBall.transform.position.z);
            var currentBall = Utils.FindNearestBySphear(shooterPoint, Utils.BallManager.Diameter / 2, ShootDirection);
            if(currentBall != null)
            {
                hitBall = currentBall;

                Utils.ShootStateMachine.Reset();
                Utils.ShootStateMachine.Go();

                StopCoroutine("StartShoot");
            }
        }
    }

    public override void Shoot(GameObject shootObject, GameObject hitObject)
    {
        shootBall = shootObject;
        hitBall = hitObject;


    }
}
