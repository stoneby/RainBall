using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject Core;
    private const float LobHeight = 20;
    private const float LobTime = .5f;
    public Vector3 TargetPosition;

    void Start()
    {
        iTween.MoveBy(Core, iTween.Hash("y", LobHeight, "time", LobTime / 2, "easeType", iTween.EaseType.easeOutQuad));
        iTween.MoveBy(Core, iTween.Hash("y", -LobHeight, "time", LobTime / 2, "delay", LobTime / 2, "easeType", iTween.EaseType.easeInCubic));
        iTween.MoveTo(gameObject, iTween.Hash("position", TargetPosition, "time", LobTime, "easeType", iTween.EaseType.linear));
        iTween.FadeTo(gameObject, iTween.Hash("delay", 1, "time", .5, "alpha", 0, "onComplete", "CleanUp"));
    }

    void CleanUp()
    {
        Destroy(gameObject);
    }
}

