using System;
using System.Collections.Generic;
using UnityEngine;

public class BallUpdater : MonoBehaviour
{
    public GameObject BrotherBall;

    public string Name;

    public float Factor { get; set; }
    
    public float DiaMeter { get; set; }

    public float Theta { get; set; }

    public bool Running { get; set; }

    private Queue<Vector3> trackingTail;

    void Born(object sender, EventArgs args)
    {
        Running = true;
        trackingTail = new Queue<Vector3>();
    }

    void Die(object sender, EventArgs args)
    {
        Running = false;
        trackingTail.Clear();
    }

    private void UpdateBrotherBall()
    {
        if (trackingTail.Count == 0)
        {
            trackingTail.Enqueue(transform.position);
            return;
        }

        var begin = trackingTail.Peek();
        var end = transform.position;
        var distance = Vector3.Distance(begin, end);
        //Debug.Log("name: " + name + ", distance: " + distance + ", begin: " + begin + ", end: " + end + ", tracking list count: " + trackingTail.Count);

        if (distance <= Theta)
        {
            //Debug.Log("name: " + name + "Too small update, pass by this vector.");
            return;
        }

        trackingTail.Enqueue(end);

        while (distance >= DiaMeter)
        {
            begin = trackingTail.Dequeue();
            //Debug.Log("name: " + name + ", dequeue point: " + begin);
            distance = Vector3.Distance(trackingTail.Peek(), end);
        }

        //Debug.Log("name: " + name + ", distance adjust: " + distance);

        var final = (begin - end);
        final.Normalize();
        final.Scale(new Vector3(DiaMeter * Factor, 1, DiaMeter * Factor));
        BrotherBall.transform.position = final + end;
    }

    void Start()
    {
        DraganBallManager.Instance.BallUpdaterList.Add(this);

        DraganBallManager.Instance.OnBorn += Born;
        DraganBallManager.Instance.OnDying += Die;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Running)
        {
            return;
        }

        UpdateBrotherBall();
    }
}
