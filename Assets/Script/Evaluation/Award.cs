using UnityEngine;

public class Award : MonoBehaviour
{
    public int Num;
    public int Color;
    public int Value;

    public string Name
    {
        get { return string.Format("Num_{0}_Color_{1}", Num, Color); }
    }

    public override string ToString()
    {
        return string.Format("Award number: {0}, Color: {1}, Value: {2}", Num, Color, Value);
    }
}
