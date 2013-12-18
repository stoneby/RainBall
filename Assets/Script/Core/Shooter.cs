using UnityEngine;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
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
    }
}
