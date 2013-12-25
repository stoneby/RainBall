using System;

[Serializable]
public class ShootPair : IComparable<ShootPair>
{
    public int Location;
    public int Color;

    public override string ToString()
    {
        return string.Format("Shoot pair: location-" + Location + ", color-" + Color);
    }

    public int CompareTo(ShootPair other)
    {
        return Location - other.Location;
    }
}
