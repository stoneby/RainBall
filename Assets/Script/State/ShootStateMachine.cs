using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootStateMachine : MonoBehaviour
{
    public StateType CurrentState { get; set; }

    public event EventHandler<EventArgs> GoStart;
    public event EventHandler<EventArgs> GoStop;

    private readonly Dictionary<StateType, IState> statesDict = new Dictionary<StateType, IState>();

    public void Go()
    {
        Debug.Log("ShootStateMachine goes at current state: " + CurrentState);

        if (CurrentState == StateType.Booming)
        {
            OnGoStart();
        }

        statesDict[CurrentState].Go();
    }

    public void End()
    {
        Debug.Log("ShootStateMachine ends at current state: " + CurrentState);
        CurrentState = StateType.EvaluateEnding;
    }

    private void OnGoStart()
    {
        if (GoStart != null)
        {
            GoStart(this, new EventArgs());
        }
    }

    private void OnGoStop()
    {
        if (GoStop != null)
        {
            GoStop(this, new EventArgs());
        }
    }

    private void OnStateEnd(object sender, EventArgs args)
    {
        Debug.Log("ShootStateMachine ends at current state: " + CurrentState);

        if (CurrentState == StateType.EvaluateEnding)
        {
            OnGoStop();
            return;
        }

        switch (CurrentState)
        {
            case StateType.Booming:
                CurrentState = StateType.EvaluateErase;
                break;
            case StateType.EvaluateErase:
                CurrentState = statesDict[CurrentState].Pass ? StateType.Erasing : StateType.EvaluateEnding;
                break;
            case StateType.Erasing:
                CurrentState = StateType.EvaluateErase;
                break;
        }
        Go();
    }

    void Awake()
    {
        Reset();
    }

    void Start()
    {
        var states = gameObject.GetComponents<AbstractState>();
        foreach (var state in states)
        {
            Debug.Log("adding state name: " + state.GetType().Name + ", state type: " + state.Type);
            statesDict.Add(state.Type, state);
        }

        foreach (var state in statesDict)
        {
            state.Value.End += OnStateEnd;
        }
    }

    public void Reset()
    {
        CurrentState = StateType.Booming;
    }
}
