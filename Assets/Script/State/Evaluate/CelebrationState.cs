using System;
using System.Collections;
using UnityEngine;

public class CelebrationState : AbstractState
{
	public MagicShow Magic;
	public int CelebrationPaidNum = 5000;
	
	public float Duration = 1f;
	
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");
		
		if (Utils.CreditBar.PaidNum > CelebrationPaidNum)
		{
			Magic.ShowTime();
			StartCoroutine("OnShootEnd");
			Magic.ShowEnd += OnShootEnd;		
		}
		else
		{
			OnEnd();
		}
    }
	
	IEnumerator OnShootEnd()
	{
		Debug.LogWarning("-------------- On shoot end.");
		yield return new WaitForSeconds(Duration);
		Magic.Clean();
		OnEnd();
	}
	
	private void OnShootEnd(object sender, EventArgs args)
	{
		Debug.LogWarning("-------------- On shoot end.");
		
		Magic.ShowEnd -= OnShootEnd;
		
		OnEnd();
	}
}
