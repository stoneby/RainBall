using System;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public BallManager Manager;
    public BallGenerator Generator;
    public Shooter Shoot;
    public ShootStateMachine StateMachine;

    private bool zumaPlay;

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
                zumaPlay = false;

                Utils.GameStateMachine.Go();
            }
            if (GUILayout.Button("Zuma", GUILayout.Width(80), GUILayout.Height(30)))
            {
                showMenu = false;
                zumaPlay = true;

                Generator.Generate();

                Manager.MoveDirection = MoveDirection.Forward;
                Utils.MoveLevel(Manager.BallUpdaterList[0].gameObject, Utils.Settings.MoveSpeed);
            }
            if (GUILayout.Button("Exit", GUILayout.Width(80), GUILayout.Height(30)))
            {
                Application.Quit();
            }
            GUILayout.EndArea();

            GUI.EndGroup();
        }
    }

    private void OnStartMoving(object sender, EventArgs args)
    {
        Debug.Log(GetType().Name + "Ball moving start.");
    }

    private void OnStopMoving(object sender, EventArgs args)
    {
        Debug.Log(GetType().Name + "Ball moving complete. Shoot begins.");
        
        Shoot.enabled = zumaPlay;
    }

    private void OnShootStart(object sender, EventArgs args)
    {
        Debug.Log(GetType().Name + " Shoot boom start.");
    }

    private void OnShootStop(object sender, EventArgs args)
    {
        Debug.Log(GetType().Name + " Shoot process complete.");

        if (Manager.BallUpdaterList.Count == 0)
        {
            Debug.Log(GetType().Name + " You win this game.");

            showMenu = true;
            Shoot.enabled = false;
        }
    }

    void Start()
    {
        Manager.StartMoving += OnStartMoving;
        Manager.StopMoving += OnStopMoving;

        StateMachine.GoStart += OnShootStart;
        StateMachine.GoStop += OnShootStop;
    }
}
