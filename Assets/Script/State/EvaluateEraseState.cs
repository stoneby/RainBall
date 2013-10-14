using System;

public class EvaluateEraseState : IState
{
    public event EventHandler<EventArgs> End;
    public bool Pass { get; set; }

    public void Go()
    {
        throw new NotImplementedException();
    }
}
