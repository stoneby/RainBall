using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Evaluator : MonoBehaviour
{
    public List<Award> Awards;

    public AwardInformation CurrentAwardInfor { get; private set; }

    private static readonly Dictionary<int, int> AmountDict = new Dictionary<int, int>
        {
            {3, 500},
            {4, 800},
            {5, 1200},
            {6, 1800},
            {7, 2400},
            {8, 3000}
        };

    public void Check(int color, int number, int index)
    {
        CurrentAwardInfor.Award = null;
        foreach (var award in Awards.Where(award => award.Color == color && award.Num == number))
        {
            CurrentAwardInfor.Award = award;
            CurrentAwardInfor.StartIndex = index;
        }

		// Display result in Menu
		if (CurrentAwardInfor.Award != null)
		{
            Utils.CreditBar.UpdateMeter(CurrentAwardInfor.Award.Value);
		}
		else
        {
            throw new NotSupportedException(
                string.Format("Not support award with color: {0} and number: {1}, please double check.", color, number));
        }
        Debug.Log("Evaluation check, current award: " + CurrentAwardInfor.Award + ", start index: " + index);
    }

    private void Init()
    {
        Awards = new List<Award>();
        for (var i = 0; i < Utils.Settings.TotalColorCount; ++i)
        {
            foreach (var pair in AmountDict)
            {
                var award = gameObject.AddComponent<Award>();
                award.Color = i;
                award.Num = pair.Key;
                award.Value = pair.Value;
                Awards.Add(award);
            }
        }

        CurrentAwardInfor = new AwardInformation();
    }

    void Awake()
    {
        Init();
    }
}
