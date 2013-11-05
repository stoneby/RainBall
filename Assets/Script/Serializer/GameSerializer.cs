using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSerializer : MonoBehaviour
{
    public List<GameObject> SerializeObjects;

    public bool HasPlayed
    {
        get { return File.Exists(GameDataPath); }
    }

    public const string GameDataFile = "GameData.txt";

    private readonly List<ISerializer> serializeList = new List<ISerializer>();
    public List<ISerializer> SerializeList { get { return serializeList; } }

    public string GameDataPath { get { return string.Format("{0}/{1}", Application.dataPath, GameDataFile); } }

    public void Serialize()
    {
        foreach (var serializer in SerializeList)
        {
            serializer.Serialize();
        }
    }

    public void Deserialize()
    {
        foreach (var serializer in SerializeList)
        {
            serializer.Deserialize();
        }
    }

    void Start()
    {
        Deserialize();
    }
}
