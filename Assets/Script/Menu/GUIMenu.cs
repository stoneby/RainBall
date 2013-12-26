using System.Collections;
using UnityEngine;

public class GUIMenu : MonoBehaviour
{
    public BallChainManager ChainManager;
    public BallChainGenerator ChainGenerator;
    public MoveDirection MoveDirection;
    public Transform BeginLocation;
    public Transform EndLocation;

    private GameObject leadBall;

    void OnGUI()
    {
        if (GUILayout.Button("ITween path"))
        {
            leadBall = Utils.BallChainManager.BallUpdaterList[0].gameObject;
            ChainManager.MoveDirection = MoveDirection.Forward;
            Utils.MoveLevel(leadBall, Utils.Settings.MoveSpeed);
        }

        if (GUILayout.Button("Pause Itween"))
        {
            iTween.Pause(leadBall);
        }

        if (GUILayout.Button("Resume Itween"))
        {
            iTween.Resume(leadBall);
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
            ChainManager.BallUpdaterList.ForEach(ball => iTweenEvent.GetEvent(ball.gameObject, "Fear").Play());
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
            Utils.BallChainManager.MoveDirection = MoveDirection.Forward;

            var leadingBall = Utils.BallChainManager.BallUpdaterList[0];
            MoveFreely(leadingBall);
        }

        if (GUILayout.Button("Moving Backward"))
        {
            Utils.BallChainManager.MoveDirection = MoveDirection.Backward;

            var leadingBall = Utils.BallChainManager.BallUpdaterList[Utils.BallChainManager.BallUpdaterList.Count - 1];
            MoveFreely(leadingBall);
        }

        if(GUILayout.Button("Snooze"))
        {
            Utils.BallChainManager.BallUpdaterList.ForEach(ball =>
                                                               {
                                                                   var tween = iTweenEvent.GetEvent(ball.gameObject,
                                                                                                    "Fear");
                                                                   tween.Play();
                                                               });
        }

        if(GUILayout.Button("Cycling"))
        {
            StartCycling();
        }

        if(GUILayout.Button("Stop Cycling"))
        {
            StopCycling();
        }
    }

    void StartCycling()
    {
        Utils.BallChainManager.StopMoving += OnStopMoving;

        var headBall = Utils.BallChainManager.BallUpdaterList[0].gameObject;
        var move = iTweenEvent.GetEvent(headBall, "Move");
        move.Play();
    }

    void StopCycling()
    {
        Utils.BallChainManager.StopMoving -= OnStopMoving;
    }

    private void OnStopMoving(object sender, BallMoveArgs args)
    {
        var headBall = Utils.BallChainManager.BallUpdaterList[0].gameObject;
        var move = iTweenEvent.GetEvent(headBall, "Move");
        move.Play();
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

        Utils.MovePartical(leadingBall.gameObject, Utils.Settings.MoveSpeed);
    }

    IEnumerator Generate()
    {
        ChainManager.BallUpdaterList.Clear();
        yield return new WaitForSeconds(1f);
        ChainGenerator.Generate();
    }

    void Start()
    {
        Utils.LevelManager.CurrentLevel = 0;
    }
}
