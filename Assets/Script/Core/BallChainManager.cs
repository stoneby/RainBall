using System;
using System.Collections.Generic;
using UnityEngine;

public class BallChainManager : MonoBehaviour
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

    private GameObject headBall;

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

    void MoveStart(string leadBallName)
    {
        Debug.LogWarning(leadBallName);
        headBall = transform.FindChild(leadBallName).gameObject;

        Debug.Log("MoveStart, lead ball: " + headBall.name + ", count: " + transform.childCount + ", frame: " + Time.frameCount);

        OnStart();

        if (StartMoving != null)
        {
            var path = iTweenEvent.GetEvent(headBall, "Move").Values["path"];
            var levelMoving = path.Equals(Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path);
            Debug.LogWarning("actual path: " + path + ", level Path: " +
                             Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path + "\n Is level moving: " +
                             levelMoving);
            StartMoving(this,
                        new BallMoveArgs
                            {
                                LeaderName = leadBallName,
                                IsLevelMoving = levelMoving
                            });
        }
    }

    void MoveComplete(string leadBallName)
    {
        headBall = transform.FindChild(leadBallName).gameObject;

        Debug.Log("MoveComplete, lead ball: " + headBall.name);

        OnStop();

        if (StopMoving != null)
        {
            var path = iTweenEvent.GetEvent(headBall, "Move").Values["path"];
            var levelMoving = path.Equals(Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path);
            Debug.LogWarning("actual path: " + path + ", level Path: " +
                             Utils.LevelManager.LevelList[Utils.LevelManager.CurrentLevel].Path + "\n Is level moving: " +
                             levelMoving);
            StopMoving(this, new BallMoveArgs
                {
                    LeaderName = leadBallName,
                    IsLevelMoving = levelMoving
                });
        }
    }

    private void OnStart()
    {
        headBall.GetComponent<BallUpdater>().Running = true;
        Set();
    }

    private void OnStop()
    {
        headBall.GetComponent<BallUpdater>().Running = false;
        
        Debug.Log("Clear up tracking tail list.");
        BallUpdaterList.ForEach(ball => ball.TrackingTail.Clear());
    }

    void Awake()
    {
        BallUpdaterList = new List<BallUpdater>();
    }

    /// <summary>
    /// After all updates complete get called
    /// </summary>
    /// <remarks>Since leading ball's itween move in update, computing in late update, or leading ball move forward one frame</remarks>
    void LateUpdate()
    {
        if (headBall == null || headBall.GetComponent<BallUpdater>() == null || !headBall.GetComponent<BallUpdater>().Running)
        {
            return;
        }

        var currentBall = headBall;
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
