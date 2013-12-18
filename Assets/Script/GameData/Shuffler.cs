using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
                Generate();
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
    private List<int> randomList;

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

        Debug.Log("++++++++++++ Index of shuffler: " + index);
    }

    private void Shuffle()
    {
        if(!initialized)
        {
            Debug.LogError("Please initialize shuffler first, by setting TotalNum.");
            return;
        }

        for(var i = 0; i < randomList.Count; ++i)
        {
            var index = Random.Range(0, randomList.Count);
            if(i == index)
            {
                continue;
            }
            Swap(randomList, i, index);
        }
    }

    private static void Swap(List<int> list, int start, int end)
    {
        var temp = list[start];
        list[start] = list[end];
        list[end] = temp;
    }

    private void Generate()
    {
        randomList = new List<int>();
        for(var i = 0; i < totalNum; ++i)
        {
            randomList.Add(i);
        }
    }

    void Start()
    {
    }
}
