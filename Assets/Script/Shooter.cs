using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
    public GameObject KeyBall;

    public float Speed = 1f;

    public float LifeTime = 5f;

    public float BoomedDuration = 1f;

    public float BoomedWaitDuration = 1f;

    public List<GameObject> ShootBalls { get; private set; }

    public EventHandler<EventArgs> BoomingOccuringEvent;
    public EventHandler<EventArgs> BoomingEndingEvent;

    private bool shooting;

    private bool canShoot = true;

    private float rot;

    private Vector3 shootDirection;

    private Vector3 stackerPoint;

    IEnumerator StartShoot()
    {
        yield return new WaitForSeconds(LifeTime);

        shooting = false;
        canShoot = true;

        Reset(true);
    }

    private void Reset(bool destroy)
    {
        if (destroy)
        {
            Destroy(ShootBalls[0]);
        }
        ShootBalls.RemoveAt(0);
        GenerateBall();
    }

    void Start()
    {
        ShootBalls = new List<GameObject>();
        stackerPoint = new Vector3(gameObject.transform.position.x, KeyBall.transform.position.y,
                                   gameObject.transform.position.z);

        GenerateBall();
    }

    private void GenerateBall()
    {
        var shootBall =
            Instantiate(KeyBall, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
                        new Quaternion()) as GameObject;
        var index = Random.Range(0, Utils.Settings.ColorList.Count);
        shootBall.renderer.material.color = Utils.Settings.ColorList[index];
        shootBall.transform.parent = transform.parent;
        shootBall.layer = LayerMask.NameToLayer("Ignore Raycast");
        ShootBalls.Add(shootBall);

        var shootBallUpdater = shootBall.AddComponent<BallUpdater>();
        shootBallUpdater.Color = index;
    }

    private void BoomingOccuring()
    {
        if (BoomingOccuringEvent != null)
        {
            BoomingOccuringEvent(ShootBalls[0], new EventArgs());
        }
    }

    private void BoomingEnding()
    {
        shooting = false;

        StartCoroutine(DoBoomingEnding());
    }

    IEnumerator DoBoomingEnding()
    {
        yield return new WaitForSeconds(BoomedDuration + BoomedWaitDuration);

        canShoot = true;

        if (BoomingEndingEvent != null)
        {
            BoomingEndingEvent(ShootBalls[0], new EventArgs());
        }

        Reset(false);
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

            shootDirection = direction;
            StartCoroutine("StartShoot");
        }

        if (shooting)
        {
            var shootBall = ShootBalls[0];

            var deta = shootDirection * Time.deltaTime * Speed;
            shootBall.transform.position += deta;

            var shooterPoint = new Vector3(shootBall.transform.position.x, KeyBall.transform.position.y,
                                           shootBall.transform.position.z);
            Debug.DrawRay(shootBall.transform.position, direction * Utils.BallManager.Diameter / 2, Color.red);
            // Collision occurs.
            var currentBall = Utils.FindNearestBySphear(shooterPoint, Utils.BallManager.Diameter / 2, direction);
            if (currentBall != null)
            {
                Debug.Log("Current hitting ball: " + currentBall.name);

                BoomingOccuring();

                // [TODO] Correct the hit point. (may overlap this time two balls)
                // [TODO] Check shoot ball join before or after current ball.
                // [FIXME] Shoot ball join after the current ball.
                var currentBallUpdater = currentBall.GetComponent<BallUpdater>();
                var nextBall = currentBallUpdater.NextBall;
                // Break link between current ball and next ball.
                currentBallUpdater.NextBall = null;

                // handle shoot ball.
                var shootBallUpdater = shootBall.GetComponent<BallUpdater>();
                shootBallUpdater.Set(currentBallUpdater);
                shootBallUpdater.Index = currentBallUpdater.Index;
                shootBall.transform.parent = currentBall.transform.parent;
                shootBall.layer = currentBall.layer;

                var ballList = Utils.BallManager.BallUpdaterList;
                // update ball list.
                ballList.Insert((nextBall == null) ? ballList.Count : (nextBall.GetComponent<BallUpdater>().Index), shootBallUpdater);

                // update following balls' index.
                for (var index = currentBallUpdater.Index + 1; index < ballList.Count; ++index)
                {
                    ++ballList[index].Index;
                    ballList[index].name = ballList[index].Name;
                }

                Debug.Log("Insert ball " + shootBallUpdater.name + ", to index: " + shootBallUpdater.Index);

                // get last ball's final position.
                Utils.BallManager.MoveDirection = MoveDirection.Backward;
                var lastBall = (nextBall == null) ? ballList[ballList.Count - 2] : ballList[ballList.Count - 1];
                var finalBallNodeList = Utils.TrimPath(Utils.LevelManager.GetPathNodes(), lastBall.transform.position,
                                                       MoveDirection.Backward, Utils.BallManager.Diameter);
                var finalBallPosition = finalBallNodeList[finalBallNodeList.Count - 1];

                for (var i = shootBallUpdater.Index; i < ballList.Count - 1; ++i)
                {
                    Utils.MoveDirectly(ballList[i].gameObject, ballList[i + 1].transform.position, Utils.Settings.InsertSpeed);
                }
                Utils.MoveDirectly(ballList[ballList.Count - 1].gameObject, finalBallPosition, Utils.Settings.InsertSpeed);

                // update next ball's linkage.
                if (nextBall)
                {
                    var nextBallUpdater = nextBall.GetComponent<BallUpdater>();
                    nextBallUpdater.LastBall = null;
                }

                BoomingEnding();

                StopCoroutine("StartShoot");
            }
        }
    }
}
