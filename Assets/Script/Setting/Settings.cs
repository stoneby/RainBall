using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    #region Speed Settings

    /// <summary>
    /// Speed that for the whole move procedure
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// Speed of shooting ball
    /// </summary>
    public float ShootSpeed;
    /// <summary>
    /// Speed for insert a shooter ball
    /// </summary>
    public float InsertSpeed;
    /// <summary>
    /// Speed for moving after booming
    /// </summary>
    public float BoomMovingSpeed;

    #endregion

    #region Count Settings

    /// <summary>
    /// Total ball count
    /// </summary>
    public int TotalBallCount;
    /// <summary>
    /// Total shoot ball count
    /// </summary>
    public int TotalShootBallCount;
    /// <summary>
    /// Total ball color count
    /// </summary>
    public int TotalColorCount;

    #endregion

    public List<Color> ColorList;

    public List<GameObject> TemplateBallList;

    void Awake()
    {
        if(TemplateBallList == null || TemplateBallList.Count != Utils.Settings.TotalColorCount)
        {
            Debug.LogError("Please double check whether template ball list has been attached or " +
                           "the count is not equal to total color count according to globel game settings.");

        }
    }

    void Start()
    {
        // sphear collider, which x and z equals diameter in our case.
        var ballController = TemplateBallList[0].GetComponent<BallController>();
        Utils.BallChainManager.Diameter = ballController.Diameter;
    }
}
