using System.Collections;
using UnityEngine;

public class PlayEndingState : AbstractState
{
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        var ballList = Utils.BallManager.BallUpdaterList;
        if (ballList.Count == 0)
        {
            Debug.Log("We meet super big celebration hit, all balls are gone.");
        }

        StartCoroutine(Debugging());
        //OnEnd();
    }

    IEnumerator Debugging()
    {
        yield return null;
        OnEnd();
    }
}
