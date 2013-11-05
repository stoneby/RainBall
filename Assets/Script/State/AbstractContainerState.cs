using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractContainerState : AbstractState
{
    public GameObject Parent;

    protected StateType CurrentState { get; set; }
    protected StateType InitState { get; set; }
    protected StateType EndState { get; set; }

    protected readonly Dictionary<StateType, IState> StatesDict = new Dictionary<StateType, IState>();

    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");
        Debug.Log(GetType().Name + ": Current sub state: " + CurrentState);

        if (CurrentState == InitState)
        {
            OnBegin();
        }

        StatesDict[CurrentState].Go();
    }

    protected abstract void OnStateEnd(object sender, EventArgs args);

    protected override void Start()
    {
        base.Start();

        if (Parent == null)
        {
            Debug.Log(GetType().Name + ": Leaf state.");
            return;
        }

        var states = Parent.GetComponents<AbstractState>();
        foreach (var state in states)
        {
            Debug.Log("adding sub state name: " + state.GetType().Name + ", state type: " + state.Type + ", of base: " + GetType().Name);
            StatesDict.Add(state.Type, state);
        }

        foreach (var state in StatesDict)
        {
            state.Value.End += OnStateEnd;
        }
    }
}
