using System;

public class EndingState : IState
{
    public event EventHandler<EventArgs> End;
    public bool Pass { get; set; }

    public void Go()
    {
        End(this, new EventArgs());
    }
}
