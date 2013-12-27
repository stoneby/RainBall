using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallChainReader : MonoBehaviour
{
    public TextAsset Text;

    public List<List<int>> BallChainList;

    public bool IsValid
    {
        get { return isBallCountValid && isColorCountValid; }
    }

    private bool isBallCountValid = true;
    private bool isColorCountValid = true;

    void Awake()
    {
        Read();
    }

    private void Read()
    {
        BallChainList = new List<List<int>>();
        var lines = Text.text.Trim().Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
        foreach(var line in lines)
        {
            var tokens = line.Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if(tokens.Length == 0)
            {
                continue;
            }
            var ballChain = new List<int>();
            foreach(var token in tokens)
            {
                var colorIndex = int.Parse(token);
                if(colorIndex < 0 || colorIndex >= Utils.Settings.TotalColorCount)
                {
                    Debug.LogWarning("Total ball color count is: " + Utils.Settings.TotalColorCount + ", but we got color: " +
                              colorIndex + " from file: " + Text.name);
                    isColorCountValid = false;
                }
                ballChain.Add(colorIndex);                
            }

            if(Utils.Settings.TotalBallCount != ballChain.Count)
            {
                Debug.LogWarning("Total ball count from setting: " + Utils.Settings.TotalBallCount +
                                 ", but we got: " + ballChain.Count + " from file: " + Text.name);
                isBallCountValid = false;
            }

            BallChainList.Add(ballChain);
        }
    }
}
