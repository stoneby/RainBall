using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEffect : MonoBehaviour
{
    #region Individual Stuff

    public float JumpDuration;
    public Vector3 JumpAmount;
    public iTween.EaseType EaseType;

    #endregion

    #region Group Stuff

    public float ItemInterval;
    public List<GameObject> ItemList;

    #endregion

    public void Go()
    {
        if(!GoCheck())
        {
            Debug.LogWarning("Fail to pass the check process.");
            return;
        }

        StartCoroutine("DoJumpingEffect");
    }

    public void Stop()
    {
        StopCoroutine("DoJumpingEffect");
    }

    IEnumerator DoJumpingEffect()
    {
        foreach(var item in ItemList)
        {
            iTween.MoveAdd(item, iTween.Hash("time", JumpDuration / 2, "amount", JumpAmount, "easetype", EaseType));
            iTween.MoveAdd(item, iTween.Hash("delay", JumpDuration / 2, "time", JumpDuration / 2, "amount", -JumpAmount, "easetype", EaseType));

            yield return new WaitForSeconds(ItemInterval);
        }
    }

    private bool GoCheck()
    {
        if(ItemList == null)
        {
            Debug.LogWarning("ItemList should not be null, Go() before it has been set up.");
            return false;
        }
        return true;
    }
}
