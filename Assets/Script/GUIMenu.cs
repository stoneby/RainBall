using System.Collections.Generic;
using UnityEngine;

public class GUIMenu : MonoBehaviour
{
    public List<GameObject> Balls;

    public GameObject Puzzle;

    public DraganBallManager Manager;

    public DraganBallGenerator Generator;

    void OnGUI()
    {
        if (GUILayout.Button("Start"))
        {
            Balls.ForEach(ball => iTweenEvent.GetEvent(ball, "Move").Play());
        }

        if (GUILayout.Button("Puzzle"))
        {
            iTweenEvent.GetEvent(Puzzle, "Move").Play();
        }

        if (GUILayout.Button("Animation"))
        {
            Manager.Run();
        }

        GUILayout.Label("Speed: ");

        float speed;
        if (float.TryParse(GUILayout.TextField("1", 5), out speed))
        {
            PathAnimationController.Instance.SetSpeed(speed);
        }

        if (GUILayout.Button("Generate Balls"))
        {
            Generator.Generate();   
        }
    }
}
