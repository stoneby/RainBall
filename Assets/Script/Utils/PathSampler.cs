using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathSampler : MonoBehaviour
{
    public float InternalDuration = 3f;

    public List<string> SampleList;

    public bool SampleAll;

    public PathParser Parser;

    private bool sampling;

    private readonly List<Vector3> sampleList = new List<Vector3>();

    private iTweenEvent sampleEvent;

    private readonly Dictionary<string, float> durationList = new Dictionary<string, float>();

    public void Sample()
    {
        StartCoroutine(DoSample());
    }

    IEnumerator DoSample()
    {
        foreach (var path in iTweenPath.Paths)
        {
            if(!SampleAll && !SampleList.Contains(path.Key))
            {
                Debug.Log("Passing through itween path: " + path.Key);
                continue;
            }
            Setup(path.Key);
            Sample(path.Key);
            yield return new WaitForSeconds(durationList[path.Key]);
            yield return new WaitForSeconds(InternalDuration);
        }
    }

    public void Sample(string path)
    {
        var sampleEvent = iTweenEvent.GetEvent(gameObject, "Sample");
        sampleEvent.Values["path"] = path;
        sampleEvent.Play();
    }

    private void InitializeDuration()
    {
        foreach (var path in iTweenPath.Paths)
        {
            if(!SampleAll && !SampleList.Contains(path.Key))
            {
                continue;
            }
            var pathLength = iTween.PathLength(iTweenPath.GetPath(path.Key));
            var speed = (float)sampleEvent.Values["speed"];
            var duration = pathLength / speed;
            durationList.Add(path.Key, duration);
            Debug.Log("Path name: " + path.Key + ", length: " + pathLength + ", duration computed: " + duration);
        }
    }

    void OnStart()
    {
        sampling = true;
        sampleList.Clear();
    }

    void OnStop()
    {
        sampling = false;

        var sampleFile = string.Format("{0}.txt", sampleEvent.Values["path"] as string);
        sampleFile = sampleFile.ToUpper();
        Parser.PositionList = sampleList;
        Parser.WriteToFile(sampleFile);
        Debug.Log("Sample file save to: " + sampleFile);
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Application path: " + Application.dataPath);

        PreHandle();

        sampleEvent = iTweenEvent.GetEvent(gameObject, "Sample");
        InitializeDuration();
    }

    private void PreHandle()
    {
        for(var i = 0; i < SampleList.Count; ++i)
        {
            SampleList[i] = SampleList[i].ToLower();
        }
    }

    private void Setup(string path)
    {
        var startPoint = iTweenPath.GetPath(path)[0];
        Debug.Log("Itween path start position: " + startPoint);
        transform.position = startPoint;

        sampleEvent.Values["path"] = path;
    }

    void FixedUpdate()
    {
        if (sampling)
        {
            sampleList.Add(transform.position);
        }
    }
}
