using System;

public enum StateType
{
    Booming,
    EvaluateErase,
    Erasing,
    Ending
}

/// <summary>
/// IState interface
/// </summary>
public interface IState
{
    event EventHandler<EventArgs> End;
    bool Pass { get; set; }
    StateType Type { get; set; }

    void Go();
}
