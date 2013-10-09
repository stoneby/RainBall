using System;
using System.Collections.Generic;
using UnityEngine;

public class DraganBallManager : MonoBehaviour
{
    public GameObject CommanderBall;

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

    void MoveStart(string leaderName)
    {
        CommanderBall = transform.FindChild(leaderName).gameObject;
        if (CommanderBall == null)
        {
            Debug.LogWarning("Please make sure you add leader name to onstartparams correctly itween.");
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
        CommanderBall = transform.FindChild(leaderName).gameObject;
        if (CommanderBall == null)
        {
            Debug.LogWarning("Please make sure you add leader name to onstartparams correctly itween.");
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

        iTween.Pause(CommanderBall);
        Running = false;
    }

    void OnBoomingComplete(object sender, EventArgs args)
    {
        Debug.Log("On Booming Complete");

        iTween.Resume(CommanderBall);
        Running = true;
    }

    private void OnStart()
    {
        Running = true;

        var ballUpdater = CommanderBall.GetComponent<BallUpdater>();
        GameObject brotherBall;
        do
        {
            brotherBall = ballUpdater.MoveDirection == MoveDirection.Forward
                                  ? ballUpdater.NextBall
                                  : ballUpdater.LastBall;
            
            Debug.Log("Brother ball - " + brotherBall.name);

            var brotherBallUpdater = brotherBall.GetComponent<BallUpdater>();
            brotherBallUpdater.CopySettings(ballUpdater);
            brotherBallUpdater.MoveDirection = ballUpdater.MoveDirection;
        } while (brotherBall != null);
    }

    private void OnStop()
    {
        Running = false;

        BallUpdaterList.ForEach(
            ball => Debug.Log("TrackingTail list cout: " + ball.TrackingTail.Count + ", gameobject: " + ball.name));
    }

    void Awake()
    {
        BallUpdaterList = new List<BallUpdater>();
        Diameter = CommanderBall.GetComponent<SphereCollider>().radius * 2;
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
        //if (!Running)
        //{
        //    return;
        //}

        //var ballUpdater = CommanderBall.GetComponent<BallUpdater>();
        //GameObject brotherBall;
        //do
        //{
        //    brotherBall = ballUpdater.MoveDirection == MoveDirection.Forward
        //                      ? ballUpdater.NextBall
        //                      : ballUpdater.LastBall;

        //    //Debug.Log("LateUpdate(): Brother ball - " + brotherBall.name);

        //    var brotherBallUpdater = brotherBall.GetComponent<BallUpdater>();
        //    brotherBallUpdater.UpdateBrotherBall();
        //} while (brotherBall != null);
    }
}
