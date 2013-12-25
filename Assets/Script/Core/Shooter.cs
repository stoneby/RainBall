using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    /// <summary>
    /// Shoot ball list
    /// </summary>
    /// <remarks>
    ///     Be very careful that when hit ball is set, we will add to hit ball list automatically,
    ///     which should be the same number as shoot ball list.
    ///     User take care of this!!!!
    /// </remarks>    
    public List<GameObject> ShootBallList { get; set; }

    /// <summary>
    /// Hit ball list
    /// </summary>
    /// <remarks>
    ///     Be very careful that when hit ball is set, we will add to hit ball list automatically,
    ///     which should be the same number as shoot ball list.
    ///     User take care of this!!!!
    /// </remarks>    
    public List<GameObject> HitBallList { get; set; }

	public GameObject ShootBall { get; set; }

    /// <summary>
    /// Hit ball to set
    /// </summary>
    public GameObject HitBall { get; set; }

    public AbstractShootBehaviour ShootBehaviour;

    public AbstractBallGenerator BallGenerator;

    public GameObject KeyBall;

    public void Shoot()
    {
        ShootBehaviour.Shoot(ShootBall, HitBall);    
    }

    public void GenerateBall()
    {
        var shootBallUpdater = BallGenerator.Generator();
        ShootBall = shootBallUpdater.gameObject;
        ShootBall.transform.parent = transform;
        ShootBall.transform.position = transform.position;
        ShootBall.layer = LayerMask.NameToLayer("Ignore Raycast");

        ShootBallList.Add(ShootBall);
    }

    private void OnMoveStart(object sender, BallMoveArgs args)
    {
        if(args.IsLevelMoving)
        {
            if(ShootBallList.Count != HitBallList.Count)
            {
                Debug.LogWarning("Warning! Shoot ball list count: " + ShootBallList.Count +
                                 ", is not equals to hit ball list count: " + HitBallList.Count);
            }
            ShootBallList.Clear();
            HitBallList.Clear();
        }
    }

    void Start()
    {
        ShootBallList = new List<GameObject>();
        HitBallList = new List<GameObject>();
        Utils.BallChainManager.StartMoving += OnMoveStart;
    }
}
