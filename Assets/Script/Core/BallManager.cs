using System;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public List<BallUpdater> BallUpdaterList { get; set; }

    public float Diameter { get; set; }

    public bool Running { get; set; }

    /// <summary>
    /// Intersect factor that between 0 and 1
    /// </summary>
    /// <remarks>To intersect lease far point to most near point to current ball position</remarks>
    [HideInInspector]
    public float IntersectFactor;

    /// <summary>
    /// Distance factor that between 0 and 1
    /// </summary>
    /// <remarks>To adjust to current ball position, ideal to 1, distance is the same as ball's diameter</remarks>
    [HideInInspector]
    public float DistanceFactor;

    /// <summary>
    /// MovePartical direction type
    /// </summary>
    [HideInInspector]
    public MoveDirection MoveDirection;

    public event EventHandler<BallMoveArgs> StartMoving;
    public event EventHandler<BallMoveArgs> StopMoving;

    private const float Theta = 0.1f;

    public GameObject HeadBall { get; private set; }

    public void Set()
    {
        foreach (var ballUpdater in BallUpdaterList)
        {
            ballUpdater.DiaMeter = Diameter;
            ballUpdater.DistanceFactor = DistanceFactor;
            ballUpdater.IntersectFactor = IntersectFactor;
            ballUpdater.Theta = Theta;
            ballUpdater.MoveDirection = MoveDirection;
        }
    }

    void MoveStart(string leaderName)
    {
        //HeadBall = transform.FindChild(leaderName).gameObject;
        HeadBall = transform.GetChild(0).gameObject;
        if (HeadBall == null)
        {
            Debug.LogWarning(string.Format("Please make sure you add leader name {0} to onstartparams correctly itween.", leaderName));
            return;
        }

        Debug.Log("MoveStart, lead ball: " + HeadBall.name + ", count: " + transform.childCount + ", frame: " + Time.frameCount);

        OnStart();

        if (StartMoving != null)
        {
            var path = iTweenEvent.GetEvent(HeadBall, "Move").Values["path"];
            var levelMoving = path.Equals(Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path);
            Debug.LogWarning("actual path: " + path + ", level Path: " + Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path);
            StartMoving(this,
                        new BallMoveArgs
                            {
                                LeaderName = leaderName,
                                IsLevelMoving = levelMoving
                            });
        }
    }

    void MoveComplete(string leaderName)
    {
        //HeadBall = transform.FindChild(leaderName).gameObject;
        HeadBall = transform.GetChild(0).gameObject;
        if (HeadBall == null)
        {
            Debug.LogWarning(string.Format("Please make sure you add leader name {0} to oncompleteparams correctly itween.", leaderName));
            return;
        }

        Debug.Log("MoveComplete, lead ball: " + HeadBall.name);

        OnStop();

        if (StopMoving != null)
        {
            var path = iTweenEvent.GetEvent(HeadBall, "Move").Values["path"];
            var levelMoving = path.Equals(Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path);
            StopMoving(this, new BallMoveArgs
                {
                    LeaderName = leaderName,
                    IsLevelMoving = levelMoving
                });
        }
    }

    void OnShootStart(object sender, EventArgs args)
    {
        Debug.Log("On Shoot Start, key ball: " + HeadBall.name);
         
        iTween.Pause(HeadBall);
    }

    void OnShootStop(object sender, EventArgs args)
    {
        Debug.Log("On Shoot Complete, key ball: " + HeadBall.name);

        iTween.Resume(HeadBall);
    }

    private void OnStart()
    {
        HeadBall.GetComponent<BallUpdater>().Running = true;
        Set();
    }

    private void OnStop()
    {
        HeadBall.GetComponent<BallUpdater>().Running = false;
        HeadBall = BallUpdaterList[0].gameObject;

        //BallUpdaterList.ForEach(
        //    ball => Debug.Log("TrackingTail list cout: " + ball.TrackingTail.Count + ", gameobject: " + ball.name));
        
        Debug.Log("Clear up tracking tail list.");
        BallUpdaterList.ForEach(ball => ball.TrackingTail.Clear());
    }

    void Awake()
    {
        BallUpdaterList = new List<BallUpdater>();
    }

    void Start()
    {
        Utils.ShootStateMachine.GoStart += OnShootStart;
        Utils.ShootStateMachine.GoStop += OnShootStop;
    }

    /// <summary>
    /// After all updates complete get called
    /// </summary>
    /// <remarks>Since leading ball's itween move in update, computing in late update, or leading ball move forward one frame</remarks>
    void LateUpdate()
    {
        HeadBall = transform.GetChild(0).gameObject;
        if (HeadBall == null || HeadBall.GetComponent<BallUpdater>() == null || !HeadBall.GetComponent<BallUpdater>().Running)
        {
            return;
        }

        var currentBall = HeadBall;
        while (currentBall != null)
        {
            var currentBallUpdater = currentBall.GetComponent<BallUpdater>();
            currentBallUpdater.UpdateBrotherBall();

            currentBall = currentBallUpdater.MoveDirection == MoveDirection.Forward
                              ? currentBallUpdater.NextBall
                              : currentBallUpdater.LastBall;
        }
    }

    public void InsertTrackingTail(int startIndex, MoveDirection moveDirection)
    {
        if (moveDirection == MoveDirection.Forward)
        {
            for (var i = startIndex; i < BallUpdaterList.Count - 1; ++i)
            {
                BallUpdaterList[i].TrackingTail.Clear();
                var intersectionList = Utils.GetIntersectionList(BallUpdaterList[i].transform.position, BallUpdaterList[i + 1].transform.position, 10);
                foreach (var node in intersectionList)
                {
                    BallUpdaterList[i].TrackingTail.AddFirst(node);
                }
            }
        }
        else if (moveDirection == MoveDirection.Backward)
        {
            for (var i = startIndex; i > 0; --i)
            {
                BallUpdaterList[i].TrackingTail.Clear();
                var intersectionList = Utils.GetIntersectionList(BallUpdaterList[i].transform.position, BallUpdaterList[i + 1].transform.position, 10);
                foreach (var node in intersectionList)
                {
                    BallUpdaterList[i].TrackingTail.AddLast(node);
                }
            }
        }
    }
}
