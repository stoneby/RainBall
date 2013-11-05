
using UnityEngine;

public class GameIdleState : AbstractState
{
    private bool entered;

    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        entered = true;

        if (Utils.GameSerializer.HasPlayed)
        {
            CirclingBalls();
        }
    }

    private void CirclingBalls()
    {
        Debug.Log("Use to circling balls.");
    }

    private void Exit()
    {
        entered = false;
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
    }
}
