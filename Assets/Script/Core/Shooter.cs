using System;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public List<GameObject> ShootBallList { get; set; }

	public GameObject ShootBall { get; set; }
    public GameObject HitBall { get; set; }

    public AbstractShootBehaviour ShootBehaviour;

    public GameObject KeyBall;

    public void Shoot()
    {
        ShootBehaviour.Shoot(ShootBall, HitBall);    
    }

    public void GenerateBall()
    {
        var index = Utils.GameDataManager.CurrentShootChain[Utils.GameDataManager.ShootBallIndex].Color;
        var templateBall = Utils.BallGenerator.TemplateBallList[index];
        ShootBall = Instantiate(templateBall, transform.position, templateBall.transform.rotation) as GameObject;
        ShootBall.transform.parent = transform;
        ShootBall.transform.position = transform.position;
        ShootBall.layer = LayerMask.NameToLayer("Ignore Raycast");

        var shootBallUpdater = ShootBall.AddComponent<BallUpdater>();
        shootBallUpdater.Color = index;

        ShootBallList.Add(ShootBall);
    }

    private void OnMoveStart(object sender, BallMoveArgs args)
    {
        if(args.IsLevelMoving)
        {
            ShootBallList.Clear();
        }
    }

    void Start()
    {
        ShootBallList = new List<GameObject>();
        Utils.BallManager.StartMoving += OnMoveStart;
    }
}
