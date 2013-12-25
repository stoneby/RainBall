using System;

public enum StateType
{
    GameIdle,
    GamePlay,
    Cleaningup,
    Initializing,
    PreShooting,
    Shooting,
    Booming,
    PlayEnding,
    GameEvaluate,
    InitControll,
    EvaluateErase,
    Erasing,
    EvaluateEnding,
    WaitForUserFire,
    FinalEnding,
    StateMachineGamble,
    StateMachineShoot
}

/// <summary>
/// IState interface
/// </summary>
public interface IState
{
    event EventHandler<EventArgs> Begin;
    event EventHandler<EventArgs> End;
    bool Pass { get; set; }
    StateType Type { get; set; }

    void Go();
}
