using System;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public GameObject KeyBall;

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

    public event EventHandler<EventArgs> OnBorn;
    public event EventHandler<EventArgs> OnDying;

    private const float Theta = 0.1f;

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
        KeyBall = transform.FindChild(leaderName).gameObject;
        if (KeyBall == null)
        {
            Debug.LogWarning(string.Format("Please make sure you add leader name {0} to onstartparams correctly itween.", leaderName));
            return;
        }

        Debug.Log("MoveStart, lead ball: " + KeyBall.name);

        OnStart();

        if (OnBorn != null)
        {
            OnBorn(this, new EventArgs());
        }
    }

    void MoveComplete(string leaderName)
    {
        KeyBall = transform.FindChild(leaderName).gameObject;
        if (KeyBall == null)
        {
            Debug.LogWarning(string.Format("Please make sure you add leader name {0} to oncompleteparams correctly itween.", leaderName));
            return;
        }

        Debug.Log("MoveComplete, lead ball: " + KeyBall.name);

        OnStop();

        if (OnDying != null)
        {
            OnDying(this, new EventArgs());
        }
    }

    void OnShootStart(object sender, EventArgs args)
    {
        Debug.Log("On Shoot Start, key ball: " + KeyBall.name);
         
        iTween.Pause(KeyBall);
    }

    void OnShootStop(object sender, EventArgs args)
    {
        Debug.Log("On Shoot Complete, key ball: " + KeyBall.name);

        iTween.Resume(KeyBall);
    }

    private void OnStart()
    {
        KeyBall.GetComponent<BallUpdater>().Running = true;
        Set();
    }

    private void OnStop()
    {
        KeyBall.GetComponent<BallUpdater>().Running = false;
        KeyBall = BallUpdaterList[0].gameObject;

        BallUpdaterList.ForEach(
            ball => Debug.Log("TrackingTail list cout: " + ball.TrackingTail.Count + ", gameobject: " + ball.name));
        
        Debug.Log("Clear up tracking tail list.");
        BallUpdaterList.ForEach(ball => ball.TrackingTail.Clear());
    }

    void Awake()
    {
        BallUpdaterList = new List<BallUpdater>();
        Diameter = KeyBall.GetComponent<SphereCollider>().radius * 2;
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
        if (KeyBall == null || KeyBall.GetComponent<BallUpdater>() == null || !KeyBall.GetComponent<BallUpdater>().Running)
        {
            return;
        }

        var currentBall = KeyBall;
        while (currentBall != null)
        {
            var currentBallUpdater = currentBall.GetComponent<BallUpdater>();
            currentBallUpdater.UpdateBrotherBall();

            currentBall = currentBallUpdater.MoveDirection == MoveDirection.Forward
                              ? currentBallUpdater.NextBall
                              : currentBallUpdater.LastBall;
        }
    }
}
