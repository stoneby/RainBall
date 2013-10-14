using System;
using System.Collections.Generic;

public enum StateType
{
    EvaluateBoom,
    Booming,
    EvaluateErase,
    Erasing,
    Ending
}

public class StateMachine
{
    public StateType CurrentState { get; set; }

    private readonly Dictionary<StateType, IState> states = new Dictionary<StateType, IState>(); 

    public StateMachine()
    {
        CurrentState = StateType.EvaluateBoom;

        states.Add(StateType.EvaluateBoom, new EvaluateBoomState());
        states.Add(StateType.Booming, new BoomingState());
        states.Add(StateType.EvaluateErase, new EvaluateEraseState());
        states.Add(StateType.Erasing, new ErasingState());
        states.Add(StateType.Ending, new EndingState());

        foreach (var state in states)
        {
            state.Value.End += OnStateEnd;
        }
    }

    public void Go()
    {
        states[CurrentState].Go();
    }

    private void OnStateEnd(object sender, EventArgs args)
    {
        if (CurrentState == StateType.Ending)
        {
            return;
        }
        
        switch (CurrentState)
        {
            case StateType.EvaluateBoom:
                CurrentState = states[CurrentState].Pass ? StateType.Booming : StateType.Ending;
                break;
            case StateType.Booming:
                CurrentState = StateType.EvaluateErase;
                break;
            case StateType.EvaluateErase:
                CurrentState = states[CurrentState].Pass ? StateType.Erasing : StateType.Ending;
                break;
            case StateType.Erasing:
                CurrentState = StateType.EvaluateErase;
                break;
        }
        Go();
    }
}
