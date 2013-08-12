using System.Collections.Generic;
using UnityEngine;

public class GUIMenu : MonoBehaviour
{
    public List<GameObject> Balls;

    public GameObject Puzzle; 

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
    }
}
