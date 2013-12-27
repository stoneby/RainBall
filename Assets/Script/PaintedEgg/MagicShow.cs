using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicShow : MonoBehaviour
{
    public GameObject TemplateBall;
    public List<PathParser> Parsers;

    public GameObject BallsParent;
	
	public event EventHandler<EventArgs> ShowEnd;
	
    public abstract void ShowTime();

    public GameObject GenerateBall(Transform parent, Vector3 position)
    {
        var templateBallList = Utils.Settings.TemplateBallList;
        var index = UnityEngine.Random.Range(0, templateBallList.Count);
        var ball =
            Instantiate(templateBallList[index], position, templateBallList[index].transform.rotation) as GameObject;
        ball.transform.parent = parent.transform;
        return ball;
    }
	
    public void Clean()
    {
        for (var i = 0; i < BallsParent.transform.childCount; ++i)
        {
            Destroy(BallsParent.transform.GetChild(i).gameObject);
        }
    }
	
	public void EndShow()
	{
		if(ShowEnd != null)
		{
			ShowEnd(this, new EventArgs());
		}
	}

    void Awake()
    {
        BallsParent = transform.parent.parent.FindChild("Balls").gameObject;
    }
}
