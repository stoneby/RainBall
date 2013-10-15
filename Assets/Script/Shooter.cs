using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
    public GameObject KeyBall;

    public float Speed = 1f;

    public float LifeTime = 5f;

    public GameObject ShootBall { get; set; }
    public GameObject HittingBall { get; private set; }

    public Vector3 ShootDirection { get; private set; }

    public EventHandler<EventArgs> BoomingOccuringEvent;
    public EventHandler<EventArgs> BoomingEndingEvent;

    private bool shooting;

    private bool canShoot = true;

    private float rot;

    private Vector3 stackerPoint;

    IEnumerator StartShoot()
    {
        yield return new WaitForSeconds(LifeTime);

        Utils.ShootStateMachine.End();

        shooting = false;
        canShoot = true;

        Reset(true);
    }

    private void Reset(bool destroy)
    {
        if (destroy)
        {
            Destroy(ShootBall);
        }
        GenerateBall();
    }

    void Start()
    {
        stackerPoint = new Vector3(gameObject.transform.position.x, KeyBall.transform.position.y,
                                   gameObject.transform.position.z);
        GenerateBall();
    }

    private void GenerateBall()
    {
        ShootBall =
            Instantiate(KeyBall, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
                        new Quaternion()) as GameObject;
        var index = Random.Range(0, Utils.Settings.ColorList.Count);
        ShootBall.renderer.material.color = Utils.Settings.ColorList[index];
        ShootBall.transform.parent = transform.parent;
        ShootBall.layer = LayerMask.NameToLayer("Ignore Raycast");

        var shootBallUpdater = ShootBall.AddComponent<BallUpdater>();
        shootBallUpdater.Color = index;
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
        var mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                                       (Camera.main.transform.position.y - transform.position.y)));
        var direction = Vector3.Normalize(mouseWorldPosition - transform.position);
        transform.rotation = Quaternion.FromToRotation(Vector3.left, direction);

        if (Input.GetButton("Fire2"))
        {
            var ball = Utils.FindNearestByRay(stackerPoint, direction);
            if (ball != null)
            {
                iTweenEvent.GetEvent(ball, "Fear").Play();
            }
        }

        if (canShoot && !shooting && Input.GetButton("Fire1"))
        {
            shooting = true;
            canShoot = false;

            rot = transform.rotation.eulerAngles.y;
            Debug.Log("euler angle is: " + rot);

            ShootDirection = direction;
            StartCoroutine("StartShoot");
        }

        if (shooting)
        {
            var deta = ShootDirection * Time.deltaTime * Speed;
            ShootBall.transform.position += deta;

            Debug.DrawRay(ShootBall.transform.position, direction * Utils.BallManager.Diameter / 2, Color.red);

            var shooterPoint = new Vector3(ShootBall.transform.position.x, Utils.Shooter.KeyBall.transform.position.y,
                                           ShootBall.transform.position.z);
            var currentBall = Utils.FindNearestBySphear(shooterPoint, Utils.BallManager.Diameter / 2, Utils.Shooter.ShootDirection);
            if (currentBall != null)
            {
                HittingBall = currentBall;

                Utils.ShootStateMachine.Reset();
                Utils.ShootStateMachine.Go();

                StopCoroutine("StartShoot");
            }
        }
    }
}
