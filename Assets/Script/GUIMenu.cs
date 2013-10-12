using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMenu : MonoBehaviour
{
    public GameObject Puzzle;

    public DraganBallManager Manager;

    public DraganBallGenerator Generator;

    public GameObject Shooter;

    public bool UseTrimNodeCount;

    public int TrimNodeCount;

    public string Path;

    public float LeadingBallSpeed;

    public MoveDirection MoveDirection;

    public Transform BeginLocation;
    public Transform EndLocation;

    void OnGUI()
    {
        if (GUILayout.Button("ITween path"))
        {
            Utils.BallManager.MoveDirection = MoveDirection.Forward;
            Utils.MoveLevel(Utils.BallManager.BallUpdaterList[0].gameObject);
        }

        if (GUILayout.Button("Pause Itween"))
        {
            iTween.Pause(Manager.KeyBall);
        }

        if (GUILayout.Button("Resume Itween"))
        {
            iTween.Resume(Manager.KeyBall);
        }

        if (GUILayout.Button("Sample All Paths"))
        {
            Utils.PathSampler.Sample();
        }

        if (GUILayout.Button("Generate Balls"))
        {
            StartCoroutine(Generate());
        }

        if (GUILayout.Button("Shaking Fears"))
        {
            Manager.BallUpdaterList.ForEach(ball => iTweenEvent.GetEvent(ball.gameObject, "Fear").Play());
        }

        if (GUILayout.Button("Generate Path By One Position"))
        {
            BallUpdater ballUpdater;

            if (!CheckGeneratePath(out ballUpdater))
            {
                return;
            }
            Utils.ConfigureTweenPath(ballUpdater.gameObject, MoveDirection);
        }

        if (GUILayout.Button("Generate Path Between Two Position"))
        {
            if (BeginLocation == null || EndLocation == null)
            {
                Debug.LogError("Please do make sure BeginLocatin and EndLocation have all been set.");
                return;
            }
            Utils.ConfigureTweenPath(BeginLocation.gameObject, BeginLocation.position, EndLocation.position);
        }

        if (GUILayout.Button("Moving Forward"))
        {
            Utils.BallManager.MoveDirection = MoveDirection.Forward;
            
            var leadingBall = Utils.BallManager.BallUpdaterList[0];
            MoveFreely(leadingBall);
        }

        if (GUILayout.Button("Moving Backward"))
        {
            Utils.BallManager.MoveDirection = MoveDirection.Backward;
            
            var leadingBall = Utils.BallManager.BallUpdaterList[Utils.BallManager.BallUpdaterList.Count - 1];
            MoveFreely(leadingBall);
        }
    }

    private static bool CheckGeneratePath(out BallUpdater ballUpdater)
    {
        var selectedObject = Utils.SelectionDetector.SelectedObject;
        if (selectedObject == null)
        {
            Debug.Log("Please selection a game object first.");
            ballUpdater = null;
            return false;
        }

        ballUpdater = selectedObject.GetComponent<BallUpdater>();
        if (ballUpdater == null)
        {
            Debug.Log("Please select a ball updater.");
            return false;
        }

        var path = selectedObject.GetComponent<iTweenPath>();
        if (path != null)
        {
            Destroy(path);
        }

        Debug.Log("Selection object: " + selectedObject.name);
        return true;
    }

    private void MoveFreely(BallUpdater leadingBall)
    {
        var path = leadingBall.GetComponent<iTweenPath>();
        if (path == null)
        {
            Debug.Log("Leading ball: " + leadingBall.name + " requires itween path, please generate it first.");
            return;
        }

        Utils.Move(leadingBall.gameObject);
    }

    IEnumerator Generate()
    {
        Manager.BallUpdaterList.Clear();
        var startPoint = iTweenPath.GetPath(Path)[0];
        iTween.MoveTo(Manager.KeyBall, startPoint, 1f);
        yield return new WaitForSeconds(1f);
        Generator.Generate();
    }

    void Start()
    {
        Utils.LevelManager.CurrentLevel = 0;
    }
}
