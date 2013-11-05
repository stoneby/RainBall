using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public TextAsset Text;

    public List<int> ShootBallList { get { return allSet[Index]; } }

    public int ShootBallIndex { get; set; }

    private int Index { get; set; }
    private List<List<int>> allSet; 

    public void Next()
    {
        ++Index;
        Index = Index >= allSet.Count ? 0 : Index;
    }

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Index = -1;

        allSet = new List<List<int>>();
        var lines = Text.text.Split('\n');
        foreach (var line in lines)
        {
            var tokens = line.Trim().Split(' ');
            var list = tokens.Select(token => int.Parse(token)).ToList();
            allSet.Add(list);
        }
    }
}
