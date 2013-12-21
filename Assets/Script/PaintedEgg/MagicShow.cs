using System.Collections.Generic;
using UnityEngine;

public abstract class MagicShow : MonoBehaviour
{
    public GameObject TemplateBall;
    public List<PathParser> Parsers;

    public abstract void ShowTime();

    public GameObject GenerateBall(Transform parent, Vector3 position)
    {
        var templateBallList = Utils.BallGenerator.TemplateBallList;
        var index = Random.Range(0, templateBallList.Count);
        var ball =
            Instantiate(templateBallList[index], position, templateBallList[index].transform.rotation) as GameObject;
        ball.transform.parent = parent.transform;
        return ball;
    }
}
