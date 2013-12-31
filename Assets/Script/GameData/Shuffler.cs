using System;
using System.Collections.Generic;
using UnityEngine;

public class Shuffler : MonoBehaviour
{
    public bool Debugging;

    public int TotalNum
    {
        get
        {
            return totalNum;
        }
        set
        {
            if(totalNum != value)
            {
                initialized = true;
                totalNum = value;
                Utils.InitRandomList(randomList, totalNum);
            }
        }
    }

    public int CurrentIndex
    {
        get
        {
            if(!initialized)
            {
                Debug.LogError("Please initialize shuffler first, by setting TotalNum.");
                throw new Exception("Please initialize shuffler first, by setting TotalNum.");
            }
            return randomList[index];
        }
    }

    private int index = -1;

    private bool initialized;
    private int totalNum;
    private readonly List<int> randomList = new List<int>();
	
	public int Gaff(int gaffIndex)
	{
		var oldIndex = index;
		index = gaffIndex;
		return oldIndex;
	}
	
    public void Next()
    {
        if(index >= randomList.Count)
        {
            index = 0;
        }

        if(index == 0)
        {
            if(!Debugging)
            {
                Shuffle();
            }
        }
        ++index;
		
		if(index >= randomList.Count)
        {
            index = 0;
        }
		
        Debug.Log("++++++++++++ Index of shuffler: " + index);
    }

    private void Shuffle()
    {
        if(!initialized)
        {
            Debug.LogError("Please initialize shuffler first, by setting TotalNum.");
            return;
        }

        Utils.Shuffle(randomList);
    }
}
