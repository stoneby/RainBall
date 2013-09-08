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

    /// <summary>
    /// Intersection point number between distance in one diameter
    /// </summary>
    /// <remarks>Used to re-generate ball's tracking point list</remarks>
    public int BoomedIntersectionNum = 10;

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
    }

    private void BoomingOccuring()
    {
        if (BoomingOccuringEvent != null)
        {
            BoomingOccuringEvent(this, new EventArgs());
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

        Reset(false);

        if (BoomingEndingEvent != null)
        {
            BoomingEndingEvent(this, new EventArgs());
        }
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
                var shootBallUpdater = shootBall.AddComponent<BallUpdater>();
                shootBallUpdater.CopySettings(currentBallUpdater);
                shootBallUpdater.Index = currentBallUpdater.Index;

                // Not the last ball, backward last ball to one ball space.
                if (nextBall)
                {
                    var nextBallUpdater = nextBall.GetComponent<BallUpdater>();
                    nextBallUpdater.LastBall = shootBall;
                    var ballList = Utils.BallManager.BallUpdaterList;
                    ballList.Insert(nextBallUpdater.Index, shootBallUpdater);

                    Debug.Log("Insert ball " + shootBallUpdater.name + ", to index: " + nextBallUpdater.Index);

                    // handle the last ball.
                    var lastBall = ballList[ballList.Count - 1];
                    var targetPositon = TrimToTargetPosition(lastBall.TrackingTail);
                    iTween.MoveTo(lastBall.gameObject, new Hashtable
                                                           {
                                                               {"position", targetPositon},
                                                               {"time", BoomedDuration},
                                                               {"easetype", iTween.EaseType.linear}
                                                           });
                    ++lastBall.Index;
                    // handle balls at shoot ball till the last, excluded.
                    for (var index = shootBallUpdater.Index + 1; index < ballList.Count - 1; ++index)
                    {
                        var firstBall = ballList[index];
                        var secondBall = ballList[index + 1];

                        //Debug.Log("First ball: " + firstBall.name + ", Second ball: " + secondBall.name);

                        // handle first ball's tracking tail list.
                        firstBall.TrackingTail.Clear();
                        var p1 = secondBall.transform.position;
                        var p2 = (secondBall.NextBall == null) ? targetPositon : secondBall.NextBall.transform.position;
                        var intersectionList = Utils.GetIntersectionList(p1, p2, BoomedIntersectionNum);
                        foreach (var position in intersectionList)
                        {
                            firstBall.TrackingTail.AddLast(position);
                        }

                        //Debug.Log("First ball's tracking list: " + firstBall.TrackingTail.Count + ", Second Ball's tracking list:" + secondBall.TrackingTail.Count);

                        // move first ball to second ball's location.
                        iTween.MoveTo(firstBall.gameObject, new Hashtable
                            {
                                {"position", secondBall.transform.position},
                                {"time", BoomedDuration},
                                {"easetype", iTween.EaseType.linear}
                            });

                        // increase index
                        ++ballList[index].Index;
                    }
                }
                // insert at the last ball case.
                else
                {
                    var targetPositon = TrimToTargetPosition(currentBallUpdater.TrackingTail);
                    var p1 = currentBall.transform.position;
                    var p2 = targetPositon;
                    // handle shoot ball's tracking list.
                    foreach (var position in currentBallUpdater.TrackingTail)
                    {
                        shootBallUpdater.TrackingTail.AddLast(position);
                    }
                    // handle shoot ball's index.
                    shootBallUpdater.Index = currentBallUpdater.Index + 1;
                    // handle shoot ball movement.
                    iTween.MoveTo(shootBall, new Hashtable
                                                 {
                                                     {"position", targetPositon},
                                                     {"time", BoomedDuration},
                                                     {"easetype", iTween.EaseType.linear}
                                                 });
                    Utils.BallManager.BallUpdaterList.Add(shootBallUpdater);
                    
                    // handle current ball's tracking list.
                    currentBallUpdater.TrackingTail.Clear();
                    var intersectionList = Utils.GetIntersectionList(p1, p2, BoomedIntersectionNum);
                    foreach (var position in intersectionList)
                    {
                        currentBallUpdater.TrackingTail.AddLast(position);
                    }
                }

                currentBallUpdater.NextBall = shootBall;
                shootBallUpdater.NextBall = nextBall;
                shootBallUpdater.LastBall = currentBall;
                shootBall.transform.parent = currentBall.transform.parent;
                shootBall.layer = currentBall.layer;
                shootBall.name = "Shooter" + "_" + shootBallUpdater.Index;

                BoomingEnding();

                StopCoroutine("StartShoot");
            }
        }
    }

    private Vector3 TrimToTargetPosition(LinkedList<Vector3> trackingList)
    {
        var startPosition = trackingList.Last.Value;
        Vector3 endPosition, preEndPosition;
        do
        {
            endPosition = trackingList.Last.Value;
            float distance = Mathf.Abs(Vector3.Distance(startPosition, endPosition));

            if (distance >= Utils.BallManager.Diameter)
            {
                break;
            }

            trackingList.RemoveLast();

            preEndPosition = endPosition;
            trackingList.RemoveLast();
        } while (true);

        var result = endPosition - startPosition;
        result.Normalize();
        result *= Utils.BallManager.Diameter;
        result += startPosition;
        trackingList.AddLast(result);
        return result;
    }
}
