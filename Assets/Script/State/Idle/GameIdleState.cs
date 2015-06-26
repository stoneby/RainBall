using UnityEngine;

public class GameIdleState : AbstractState
{
    public JumpingEffect Jump;

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

            Jump.ItemList = Utils.BallChainManager.ToList();
            Jump.Go();
        }
    }

    private void CirclingBalls()
    {
        Debug.Log("Use to circling balls.");
    }

    public void Exit()
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

    public void Gaffing()
    {
        gaffEnabled = true;
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
		    Gaffing();
		}
    }
}
