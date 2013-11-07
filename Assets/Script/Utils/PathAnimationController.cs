using System;
using UnityEngine;

public class PathAnimationController : MonoBehaviour
{
    public event EventHandler<EventArgs> AnimationStarted;
    public event EventHandler<EventArgs> AnimationStopped;

    public void SetSpeed(float speed)
    {
        animation["Path"].speed = speed;
    }

    void AnimationStart()
    {
        if (AnimationStarted != null)
        {
            AnimationStarted(this, new EventArgs());
        }
    }

    void AnimationStop()
    {
        if (AnimationStopped != null)
        {
            AnimationStopped(this, new EventArgs());
        }
    }
}
