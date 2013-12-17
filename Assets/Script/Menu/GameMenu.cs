using System;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private bool showMenu = true;

    void OnGUI()
    {
        if (showMenu)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Game Menu");

            GUILayout.BeginArea(new Rect(Screen.width/2 - 50, Screen.height/2 - 50, 100, 100));
            if (GUILayout.Button("Gamble", GUILayout.Width(80), GUILayout.Height(30)))
            {
                showMenu = false;

                Utils.GameStateMachine.Go();
            }
            if (GUILayout.Button("Zuma", GUILayout.Width(80), GUILayout.Height(30)))
            {
                showMenu = false;

                Utils.ShootStateMachine.Go();
            }
            if (GUILayout.Button("Exit", GUILayout.Width(80), GUILayout.Height(30)))
            {
                Application.Quit();
            }
            GUILayout.EndArea();

            GUI.EndGroup();
        }
    }
}
