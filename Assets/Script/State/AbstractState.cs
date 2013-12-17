using System;
using UnityEngine;

public abstract class AbstractState : MonoBehaviour, IState
{
    public event EventHandler<EventArgs> Begin;
    public event EventHandler<EventArgs> End;
    public bool Pass { get; set; }
    public StateType Type { get; set; }

    public abstract void Go();

    protected void OnBegin()
    {
        if (Begin != null)
        {
            Begin(this, new EventArgs());
        }
    }

    protected void OnEnd()
    {
        if (End != null)
        {
            End(this, new EventArgs());
        }
    }

    protected virtual void Awake()
    {
        Debug.Log("AbstractState name: " + GetType().Name);
        Type = (StateType)Enum.Parse(typeof(StateType), GetType().Name.Remove(GetType().Name.LastIndexOf("State")));
    }

    protected virtual void Start()
    {
    }
}
