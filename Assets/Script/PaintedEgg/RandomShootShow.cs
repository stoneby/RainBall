using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShootShow : MagicShow
{
    public Shooter Shooter;
    public float TimeInterval;
    public bool Shuffle;

    private List<Vector3> positionList = new List<Vector3>();
    private List<int> randomList = new List<int>();

    public override void ShowTime()
    {
        Clean();

        positionList.Clear();
        Parsers.ForEach(parser => parser.ReadFromFile(parser.SampleFile));
        Parsers.ForEach(parser => positionList.AddRange(parser.PositionList));

        Utils.InitRandomList(randomList, positionList.Count);
        if (Shuffle)
        {
            Utils.Shuffle(randomList);
        }

        StartCoroutine("DoShowTime");
    }

    IEnumerator DoShowTime()
    {
        yield return null;
		
        Shooter.ShootBehaviour.ShootEndEnabled = false;

        for (var i = 0; i < positionList.Count; ++i)
        {
            Shooter.ShootBall = GenerateBall(BallsParent.transform, Shooter.transform.position);
            Shooter.HitBall = GenerateBall(BallsParent.transform, positionList[randomList[i]]);
            Shooter.HitBall.SetActive(false);
            Shooter.Shoot();
            yield return new WaitForSeconds(TimeInterval);
        }
    }
}
