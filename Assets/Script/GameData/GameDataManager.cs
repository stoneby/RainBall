using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public BallChainReader BallReader;
    public ShootLocationReader ShootReader;

    public Shuffler Shuffler;

    public List<int> CurrentBallChain
    {
        get { return BallReader.BallChainList[Shuffler.CurrentIndex]; }
    }

    public List<ShootPair> CurrentShootChain
    {
        get { return ShootReader.ShootLocationsList[Shuffler.CurrentIndex]; }
    }

    /// <summary>
    /// Shoot ball index of current game play, say 5 shoot in total
    /// </summary>
    public int ShootBallIndex { get; set; }

    public void Next()
    {
        Shuffler.Next();
    }

    void Start()
    {
        if(!Validate())
        {
            const string error = "Game data is not valid. Please double check the warning above.";
            Debug.LogError(error);
            throw new Exception(error);
        }

        Shuffler.TotalNum = BallReader.BallChainList.Count;
    }

    private bool Validate()
    {
        if(BallReader.IsValid && ShootReader.IsValid)
        {
            if(BallReader.BallChainList.Count != ShootReader.ShootLocationsList.Count)
            {
                Debug.LogWarning("Ball chain list count does not match ball shoot location list count.");
                return false;
            }
            return true;
        }
        return false;
    }

    public void Display()
    {
        Debug.Log("Current shoot ball list.");
        CurrentShootChain.ForEach(item => Debug.Log(" " + item));
    }
}
