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

    public List<GameObject> ShootBallList { get; set; }
    public GameObject ShootBall { get; set; }
    public GameObject HittingBall { get; set; }

    public Vector3 ShootDirection { get; private set; }

    public EventHandler<EventArgs> BoomingOccuringEvent;
    public EventHandler<EventArgs> BoomingEndingEvent;

    public bool Enabled { get; set; }

    private bool shooting;

    private bool canShoot = true;

    private float rot;

    private Vector3 stackerPoint;
	
	float lobHeight = 40;
	float lobTime = 0.7f;
	private GameObject ballWrapper;

    IEnumerator StartShoot()
    {
		Vector3[] nodesList = iTweenPath.GetPath("Level_1");
		int index = Random.Range(0,nodesList.Length);
		Vector3 targetPosition = nodesList[index];
		Vector3 direction = Vector3.Normalize(targetPosition - transform.position);
		
		iTween.MoveBy(ShootBall, iTween.Hash("y", lobHeight, "time", lobTime/2, "easeType", iTween.EaseType.easeOutQuad));
		iTween.MoveBy(ShootBall, iTween.Hash("y", -lobHeight, "time", lobTime/2, "delay", lobTime/2, "easeType", iTween.EaseType.easeInCubic));     
		iTween.MoveTo(ballWrapper, iTween.Hash("position", targetPosition, "time", lobTime, "easeType", iTween.EaseType.linear));
		
		var currentBall = Utils.FindNearestBySphear(targetPosition, Utils.BallManager.Diameter / 2, direction);
        if (currentBall != null)
        {
            HittingBall = currentBall;

            Utils.ShootStateMachine.Reset();
            Utils.ShootStateMachine.Go();

            StopCoroutine("StartShoot");
        }
		
        yield return new WaitForSeconds(LifeTime);

        Utils.ShootStateMachine.End();

        shooting = false;
        canShoot = true;

        Reset(true);
    }

    private void Reset(bool destroy)
    {
        if (destroy)
        {
            Destroy(ShootBall);
        }
        GenerateBall();
    }

    void Start()
    {
        stackerPoint = new Vector3(gameObject.transform.position.x, KeyBall.transform.position.y,
                                   gameObject.transform.position.z);
        GenerateBall();
    }

    public void GenerateBall()
    {
		GameObject shooter = GameObject.Find("Shooter");
        ballWrapper = Instantiate(KeyBall, shooter.transform.position, Quaternion.identity) as GameObject;
		ShootBall = ballWrapper.transform.Find("TemplateKeyBall").gameObject;
		ShootBall.transform.position=shooter.transform.position;
        var index = Random.Range(0, Utils.Settings.ColorList.Count);
        ShootBall.renderer.material.color = Utils.Settings.ColorList[index];
        ShootBall.layer = LayerMask.NameToLayer("Ignore Raycast");

        var shootBallUpdater = ShootBall.AddComponent<BallUpdater>();
        shootBallUpdater.Color = index;
    }

    private void OnShootStart(object sender, EventArgs args)
    {
        shooting = false;
    }

    private void OnShootStop(object sender, EventArgs args)
    {
        canShoot = true;
        Reset(false);
    }

    void Awake()
    {
        Utils.ShootStateMachine.GoStart += OnShootStart;
        Utils.ShootStateMachine.GoStop += OnShootStop;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Enabled)
        {
            return;
        }

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

            ShootDirection = direction;
            StartCoroutine("StartShoot");
        }

        if (shooting)
        {
            var deta = ShootDirection * Time.deltaTime * Speed;
            ShootBall.transform.position += deta;

            Debug.DrawRay(ShootBall.transform.position, direction * Utils.BallManager.Diameter / 2, Color.red);

            var shooterPoint = new Vector3(ShootBall.transform.position.x, Utils.Shooter.KeyBall.transform.position.y,
                                           ShootBall.transform.position.z);
            var currentBall = Utils.FindNearestBySphear(shooterPoint, Utils.BallManager.Diameter / 2, Utils.Shooter.ShootDirection);
            if (currentBall != null)
            {
                HittingBall = currentBall;

                Utils.ShootStateMachine.Reset();
                Utils.ShootStateMachine.Go();

                StopCoroutine("StartShoot");
            }
        }
    }
}
