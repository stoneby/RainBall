using System;

public class ErasingState : IState
{
    public event EventHandler<EventArgs> End;
    public bool Pass { get; set; }

    public void Go()
    {
        throw new NotImplementedException();
    }
}
