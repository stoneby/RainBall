using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootLocationReader : MonoBehaviour
{
    public TextAsset Text;

    public List<List<ShootPair>> ShootLocationsList;

    public bool IsValid
    {
        get { return isShootCountValid && isColorCountValid; }
    }

    private bool isShootCountValid = true;
    private bool isColorCountValid = true;

    void Awake()
    {
        Read();
    }

    private void Read()
    {
        ShootLocationsList = new List<List<ShootPair>>();
        var lines = Text.text.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach(var line in lines)
        {
            var tokens = line.Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if(tokens.Length == 0)
            {
                continue;
            }

            var shootLocations = new List<ShootPair>();
            foreach(var token in tokens)
            {
                var pair = token.Substring(1, token.Length - 2)
                                .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                if(pair.Length != 2)
                {
                    continue;
                }

                var location = int.Parse(pair[0]);
                var colorIndex = int.Parse(pair[1]);
                if(colorIndex < 0 || colorIndex > Utils.Settings.TotalColorCount)
                {
                    Debug.Log("Total ball color count is: " + Utils.Settings.TotalColorCount + ", but we got color: " +
                              colorIndex + " from file: " + Text.name);
                    isColorCountValid = false;
                }

                shootLocations.Add(new ShootPair { Location = location, Color = colorIndex });
            }

            if(Utils.Settings.TotalShootBallCount != shootLocations.Count)
            {
                Debug.LogWarning("Total ball count from setting: " + Utils.Settings.TotalBallCount +
                                 ", but we got: " + shootLocations.Count + " from file: " + Text.name);
                isShootCountValid = false;
            }

            ShootLocationsList.Add(shootLocations);
        }
    }
}
