using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public StateType CurrentState { get; set; }

    public StateType InitState { get; set; }

    private readonly Dictionary<StateType, IState> statesDict = new Dictionary<StateType, IState>();

    public void Go()
    {
        Debug.Log("GameStateMachine goes at current state: " + CurrentState);
        statesDict[CurrentState].Go();
    }

    private void OnStateEnd(object sender, EventArgs args)
    {
        Debug.Log("GameStateMachine ends at current state: " + CurrentState);

        switch (CurrentState)
        {
            case StateType.GameIdle:
                CurrentState = StateType.GamePlay;
                break;
            case StateType.GamePlay:
                CurrentState = StateType.GameEvaluate;
                break;
            case StateType.GameEvaluate:
                CurrentState = StateType.GameIdle;
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
        CurrentState = InitState;
    }
}
