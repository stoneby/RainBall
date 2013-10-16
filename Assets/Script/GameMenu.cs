using System;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public BallManager Manager;
    public BallGenerator Generator;
    public Shooter Shoot;
    public ShootStateMachine StateMachine;

    private bool showMenu = true;

    void OnGUI()
    {
        if (showMenu)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Game Menu");

            GUILayout.BeginArea(new Rect(Screen.width/2 - 50, Screen.height/2 - 50, 100, 100));
            if (GUILayout.Button("Start", GUILayout.Width(80), GUILayout.Height(30)))
            {
                showMenu = false;
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

    private void OnBorn(object sender, EventArgs args)
    {
        Debug.Log(GetType().Name + "Ball moving start.");
    }

    private void OnDie(object sender, EventArgs args)
    {
        Debug.Log(GetType().Name + "Ball moving complete. Shoot begins.");
        
        Shoot.enabled = true;
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
        Manager.Born += OnBorn;
        Manager.Die += OnDie;

        StateMachine.GoStart += OnShootStart;
        StateMachine.GoStop += OnShootStop;
    }
}
