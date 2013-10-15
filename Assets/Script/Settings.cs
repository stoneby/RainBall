using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public List<Color> ColorList;

    /// <summary>
    /// Speed that for the whole move procedure
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// Speed for insert a shooter ball
    /// </summary>
    public float InsertSpeed;
    /// <summary>
    /// Speed for moving after booming
    /// </summary>
    public float BoomMovingSpeed;
}
