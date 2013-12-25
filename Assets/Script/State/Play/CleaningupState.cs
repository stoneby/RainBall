using System.Collections;
using UnityEngine;

public class CleaningupState : AbstractState
{
    // Use this for initialization
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        if (Utils.GameSerializer.HasPlayed)
        {
            StartCoroutine(Disappear());
        }
        else
        {
            OnEnd();
        }
    }

    IEnumerator Disappear()
    {
        Utils.BallChainManager.BallUpdaterList.ForEach(ball => iTweenEvent.GetEvent(ball.gameObject, "Disappear").Play());
        Utils.BallChainManager.BallUpdaterList.ForEach(ball => iTweenEvent.GetEvent(ball.gameObject, "Fear").Play());

        var disappearEvent = iTweenEvent.GetEvent(Utils.TemplateBall, "Disappear");
        yield return new WaitForSeconds(float.Parse(disappearEvent.Values["time"].ToString()));
        
        OnEnd();
    }
}
