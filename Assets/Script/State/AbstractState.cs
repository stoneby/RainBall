using System;
using UnityEngine;

public abstract class AbstractState : MonoBehaviour, IState
{
    public event EventHandler<EventArgs> End;
    public bool Pass { get; set; }
    public StateType Type { get; set; }

    public abstract void Go();

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
        Type = (StateType)Enum.Parse(typeof(StateType), GetType().Name.Replace("State", string.Empty));
    }
}
