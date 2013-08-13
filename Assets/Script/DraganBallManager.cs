using System;
using System.Collections.Generic;
using UnityEngine;

public class DraganBallManager : MonoBehaviour
{
    public GameObject CommandBall;

    public List<BallUpdater> BallUpdaterList { get; set; }

    public float Factor;

    public event EventHandler<EventArgs> OnBorn;
    public event EventHandler<EventArgs> OnDying;

    public static DraganBallManager Instance;

    private float diameter;

    private const float Theta = 0f;

    public void Run()
    {
        CommandBall.animation.Play("Path");
    }

    private void SetDiameter()
    {
        diameter = CommandBall.GetComponent<SphereCollider>().radius * 2;

        BallUpdaterList.ForEach(ball => ball.DiaMeter = diameter);
        BallUpdaterList.ForEach(ball => ball.Theta = Theta);
    }

    private void AnimationStarted(object sender, EventArgs args)
    {
        SetDiameter();

        BallUpdaterList.ForEach(ball => ball.Factor = Factor);
        BallUpdaterList.ForEach(ball => ball.Running = true);
        
        if (OnBorn != null)
        {
            OnBorn(this, new EventArgs());
        }
    }

    private void AnimationStopped(object sender, EventArgs args)
    {
        SetDiameter();

        BallUpdaterList.ForEach(ball => ball.Running = false);

        if (OnDying != null)
        {
            OnDying(this, new EventArgs());
        }
    }

    void MoveStart()
    {
        SetDiameter();

        BallUpdaterList.ForEach(ball => ball.Factor = Factor);
        BallUpdaterList.ForEach(ball => ball.Running = true);

        if (OnBorn != null)
        {
            OnBorn(this, new EventArgs());
        }
    }

    void MoveComplete()
    {
        SetDiameter();

        BallUpdaterList.ForEach(ball => ball.Running = false);

        if (OnDying != null)
        {
            OnDying(this, new EventArgs());
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError("Singleton Dude.!");
        }
        BallUpdaterList = new List<BallUpdater>();
    }

    void Start()
    {
        PathAnimationController.Instance.AnimationStarted += AnimationStarted;
        PathAnimationController.Instance.AnimationStopped += AnimationStopped;
    }
}
