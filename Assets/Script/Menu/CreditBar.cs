using UnityEngine;
using System.Collections;

public class CreditBar : MonoBehaviour
{
	public Texture2D img;
	public GUIStyle numberStyle;
	public float NumberChangeTime = 0.8f;
	public int BetNum = 50;
	public int CreditNum = 1500;
	public int PaidNum {get;set;}
	private GUIStyle blankStyle = new GUIStyle (); //an "empty" style to avoid any of Unity's default padding, margin and background defaults
	private Rect container = new Rect (110, Screen.height - 96, 1417, 96);
	private Rect betRect = new Rect (310, 30, 70, 30);
	private Rect creditRect = new Rect (760, 30, 210, 30);
	private Rect paidRect = new Rect (1050, 30, 200, 30);
	
	void OnGUI ()
	{
		GUI.BeginGroup (container, img, blankStyle);
		GUI.Label (betRect, BetNum.ToString (), numberStyle);
		GUI.Label (creditRect, CreditNum.ToString (), numberStyle);
		GUI.Label (paidRect, PaidNum.ToString (), numberStyle);
		GUI.EndGroup ();
		
		// Test
//		if (GUI.Button (new Rect (10, Screen.height / 2 - 20, 80, 40), "Increase")) {
//			PaidChange (15);
//			CreditChange (15);
//		}
//		
//		if (GUI.Button (new Rect (10, Screen.height / 2 +25, 80, 40), "Decrease")) {
//			PaidChange (-15);
//			CreditChange (-15);
//		}
	}
	
	public void PaidChange (int i)
	{
		iTween.StopByName("PainChange");
		iTween.ValueTo (gameObject, 
			iTween.Hash ("name", "PainChange", "time", NumberChangeTime, "from", PaidNum, "to", PaidNum + i, "onupdate", "ApplyPaidChange"));
	}
	
	public void CreditChange (int i)
	{
		iTween.StopByName("CreditChange");
		iTween.ValueTo (gameObject, 
			iTween.Hash ("name", "CreditChange", "time", NumberChangeTime, "from", CreditNum, "to", CreditNum + i, "onupdate", "ApplyCreditChange"));
	}
	
	void ApplyPaidChange (int i)
	{
		PaidNum = i;
	}
	
	void ApplyCreditChange(int i)
	{
		CreditNum = i;
	}
}
