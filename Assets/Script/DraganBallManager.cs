using System;
using System.Collections.Generic;
using UnityEngine;

public class DraganBallManager : MonoBehaviour
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
    /// Move direction type
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

        OnStop();

        if (OnDying != null)
        {
            OnDying(this, new EventArgs());
        }
    }

    void OnBoomingStart(object sender, EventArgs args)
    {
        Debug.Log("On Booming Start");

        iTween.Pause(KeyBall);
        Running = true;
    }

    void OnBoomingComplete(object sender, EventArgs args)
    {
        Debug.Log("On Booming Complete");

        var shooter = sender as Shooter;
        var shootBallUpdater = shooter.ShootBalls[0].GetComponent<BallUpdater>();
        var currentBallUpdater = BallUpdaterList[shootBallUpdater.Index - 1];
        currentBallUpdater.NextBall = shootBallUpdater.gameObject;
        shootBallUpdater.LastBall = BallUpdaterList[shootBallUpdater.Index - 1].gameObject;
        GameObject nextBall = null;
        if (shootBallUpdater.Index + 1 < BallUpdaterList.Count)
        {
            nextBall = BallUpdaterList[shootBallUpdater.Index + 1].gameObject;
            var nextballUpdater = nextBall.GetComponent<BallUpdater>();
            nextballUpdater.LastBall = shootBallUpdater.gameObject;
        }
        shootBallUpdater.NextBall = nextBall;

        iTween.Resume(KeyBall);
        Running = false;
    }

    private void OnStart()
    {
        Running = true;
        Set();
    }

    private void OnStop()
    {
        Running = false;

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
        Utils.Shooter.BoomingOccuringEvent += OnBoomingStart;
        Utils.Shooter.BoomingEndingEvent += OnBoomingComplete;
    }

    /// <summary>
    /// After all updates complete get called
    /// </summary>
    /// <remarks>Since leading ball's itween move in update, computing in late update, or leading ball move forward one frame</remarks>
    void LateUpdate()
    {
        if (!Running)
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
