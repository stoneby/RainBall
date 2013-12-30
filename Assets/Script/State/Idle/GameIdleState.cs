using System.Collections;
using UnityEngine;

public class GameIdleState : AbstractState
{
    public float JumpInterval;
    public float JumpHeight;
    public float JumpDuration;

    private bool entered;
	
	private bool gaffEnabled;
	
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        entered = true;

        Utils.Shooter.ShootBehaviour.ShootEndEnabled = true;

        //if (Utils.GameSerializer.HasPlayed)
        {
            CirclingBalls();
            JumpingBalls();
        }
    }

    private void CirclingBalls()
    {
        Debug.Log("Use to circling balls.");
    }

    private void JumpingBalls()
    {
        StartCoroutine("DoJumpingBalls");
    }

    IEnumerator DoJumpingBalls()
    {
        SetJumpData();

        foreach(var ballUpdater in Utils.BallChainManager.BallUpdaterList)
        {
            var ball = ballUpdater.GetComponent<BallController>().Ball;
            var jumpEvent = iTweenEvent.GetEvent(ball, "Jump");
            jumpEvent.Play();

            var fallEvent = iTweenEvent.GetEvent(ball, "Fall");
            fallEvent.Play();

            yield return new WaitForSeconds(JumpInterval);
        }
    }

    private void SetJumpData()
    {
        Utils.BallChainManager.BallUpdaterList.ForEach(ballUpdater =>
        {
            var ball =
                ballUpdater.GetComponent<BallController>().Ball;
            var jumpEvent = iTweenEvent.GetEvent(ball, "Jump");
            jumpEvent.Values["time"] = JumpDuration / 2;
            jumpEvent.Values["amount"] = new Vector3(0, 0, JumpHeight);

            var fallEvent = iTweenEvent.GetEvent(ball, "Fall");
            fallEvent.Values["time"] = JumpDuration / 2;
            fallEvent.Values["amount"] = new Vector3(0, 0, -JumpHeight);
            fallEvent.Values["delay"] = JumpDuration / 2;
        });
    }

    private void Exit()
    {
        entered = false;
		
		if(gaffEnabled)
		{
			gaffEnabled = false;
			Utils.GameDataManager.Shuffler.Gaff(20);
		}
		else
		{
        	Utils.GameDataManager.Next();
        	Utils.GameDataManager.Display();
		}

		StopAllCoroutines();

        OnEnd();
    }

    void Update()
    {
        if (!entered)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
		{
            Exit();
        }
		
		if(Input.GetKey(KeyCode.G))
		{
			gaffEnabled = true;
		}
    }
}
