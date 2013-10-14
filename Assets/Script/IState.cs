using System;

/// <summary>
/// IState interface
/// </summary>
public interface IState
{
    event EventHandler<EventArgs> End;
    bool Pass { get; set; }

    void Go();
}
