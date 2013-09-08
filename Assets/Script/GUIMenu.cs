using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMenu : MonoBehaviour
{
    public GameObject LeaderBall;

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
            var moveEvent = iTweenEvent.GetEvent(LeaderBall, "Move");
            moveEvent.Values["onstartparams"] = LeaderBall.name;
            moveEvent.Values["oncompleteparams"] = LeaderBall.name;
            moveEvent.Play();
        }

        if (GUILayout.Button("Pause Itween"))
        {
            iTween.Pause(Manager.CommanderBall);
        }

        if (GUILayout.Button("Resume Itween"))
        {
            iTween.Resume(Manager.CommanderBall);
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

        if (GUILayout.Button("Generate Path By Tracking Tail"))
        {
            BallUpdater ballUpdater;

            if (!CheckGeneratePath(out ballUpdater))
            {
                return;
            }

            var nodeList = new List<Vector3>();
            nodeList.AddRange(ballUpdater.TrackingTail);
            var pathLength = Utils.PathLength(nodeList);
            TrimNodeCount = (UseTrimNodeCount) ? TrimNodeCount : ((int)(pathLength / Manager.Diameter) + 1);
            TrimNodeCount = (TrimNodeCount >= iTweenPath.MaxNodeCount) ? iTweenPath.MaxNodeCount : TrimNodeCount;

            Debug.Log("Path length: " + pathLength + ", Trim node count: " + TrimNodeCount);

            var trimmedNodeList = Utils.TrimList(nodeList, TrimNodeCount);
            trimmedNodeList.Reverse();
            Utils.ConfigureTweenPath(ballUpdater.gameObject, trimmedNodeList, "LeadingPath");
        }

        if (GUILayout.Button("Generate Path By Original Path"))
        {
            BallUpdater ballUpdater;

            if (!CheckGeneratePath(out ballUpdater))
            {
                return;
            }
			
			Utils.LevelManager.CurrentLevel = 0;
			var detailPath = Utils.LevelManager.GetDetailPath();
            var pathNodes = new List<Vector3>();
            pathNodes.AddRange(iTweenPath.GetPath(detailPath));
            var nodeList = Utils.TrimPath(pathNodes, ballUpdater.gameObject.transform.position, MoveDirection);
            Utils.ConfigureTweenPath(ballUpdater.gameObject, nodeList, "LeadingPath");
        }
		
		if (GUILayout.Button("Generate Path Between Two Position"))
		{
			if (BeginLocation == null || EndLocation == null)
			{
				Debug.LogError("Please do make sure BeginLocatin and EndLocation have all been set.");
				return;
			}

			Utils.LevelManager.CurrentLevel = 0;
			var detailPath = Utils.LevelManager.GetDetailPath();
            var pathNodes = new List<Vector3>();
            pathNodes.AddRange(iTweenPath.GetPath(detailPath));
            var nodeList = Utils.TrimPath(pathNodes, BeginLocation.position, EndLocation.position);
			var ballUpdater = BeginLocation.GetComponent<BallUpdater>();
			Utils.ConfigureTweenPath(ballUpdater.gameObject, nodeList, "LeadingPath_" + ballUpdater.name);	
		}

        if (GUILayout.Button("Moving Forward"))
        {
            var leadingBall = Utils.BallManager.BallUpdaterList[0];
            var endingBall = Utils.BallManager.BallUpdaterList[Utils.BallManager.BallUpdaterList.Count - 1];

            MoveFreely(endingBall, leadingBall);
        }

        if (GUILayout.Button("Moving Backward"))
        {
            var leadingBall = Utils.BallManager.BallUpdaterList[Utils.BallManager.BallUpdaterList.Count - 1];
            var endingBall = Utils.BallManager.BallUpdaterList[0];

            MoveFreely(endingBall, leadingBall);
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

        Debug.Log("Selection object: " + selectedObject.name + ", ball updater: " + ballUpdater.TrackingTail.Count);
        return true;
    }

    private void MoveFreely(BallUpdater endingBall, BallUpdater leadingBall)
    {
        endingBall.TrackingTail.Clear();

        var path = leadingBall.GetComponent<iTweenPath>();
        if (path == null)
        {
            Debug.Log("Leading ball: " + leadingBall.name + " requires itween path, please generate it first.");
            return;
        }

        var moveEvent = iTweenEvent.GetEvent(leadingBall.gameObject, "Move");
        moveEvent.Values["onstartparams"] = LeaderBall.name;
        moveEvent.Values["oncompleteparams"] = LeaderBall.name;
        moveEvent.Play();

        //iTween.MoveTo(leadingBall.gameObject, new Hashtable
        //    {
        //        {"path", path.Nodes.ToArray()},
        //        //{"position", path.Nodes[path.NodeCount - 1]},
        //        {"speed", LeadingBallSpeed},
        //        {"easetype", iTween.EaseType.linear}
        //    });
    }

    IEnumerator Generate()
    {
        Manager.BallUpdaterList.Clear();
        var startPoint = iTweenPath.GetPath(Path)[0];
        iTween.MoveTo(Manager.CommanderBall, startPoint, 1f);
        yield return new WaitForSeconds(1f);
        Generator.Generate();
    }
}
