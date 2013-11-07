using UnityEngine;
using System.Collections;

public class QuaternionTest : MonoBehaviour
{
    #region Lerp / Slerp / Angle

    public Transform Begin;
    public Transform End;
    
    public float Speed;

    #endregion

    #region Look Rotation / FromToRotation

    public Vector3 LookForward;
    public Vector3 LookUpWard;

    #endregion

    #region Angle Axis

    public float Angle;

    public Vector3 Axis;

    #endregion

    #region Euler

    public Vector3 EulerAxis;

    #endregion

    #region Operator*

    public Vector3 MultipleDirection;

    #endregion

    void OnGUI()
    {
        if (GUILayout.Button("Slerp"))
        {
            StartCoroutine(DoSlerp());
        }

        if (GUILayout.Button("Lerp"))
        {
            StartCoroutine(DoLerp());
        }

        // Z axis looking at LookForward
        if (GUILayout.Button("LookRotation(forward)"))
        {
            transform.rotation = Quaternion.LookRotation(LookForward);
        }

        if (GUILayout.Button("LookRotation(forward, upward"))
        {
            transform.rotation = Quaternion.LookRotation(LookForward, LookUpWard);
        }

        if (GUILayout.Button("FromToRotation"))
        {
            transform.rotation = Quaternion.FromToRotation(LookForward, LookUpWard);
        }

        if (GUILayout.Button("Angle"))
        {
            var angle = Quaternion.Angle(Begin.rotation, End.rotation);
            Debug.Log("Angle between begin: " + Begin.rotation + " and end: " + End.rotation + ", is: " + angle);
        }

        if (GUILayout.Button("AngleAxis"))
        {
            transform.rotation = Quaternion.AngleAxis(Angle, Axis);
        }

        if (GUILayout.Button("Euler"))
        {
            transform.rotation = Quaternion.Euler(EulerAxis);
        }

        if (GUILayout.Button("operator*"))
        {
            StartCoroutine(DoMultiple());
        }
    }

    /// <summary>
    /// Slerp
    /// </summary>
    /// <remarks>Move smoothly between [0, 180), clockwise or not.</remarks>
    IEnumerator DoSlerp()
    {
        float slerpValue = 0f;
        do
        {
            slerpValue += Time.deltaTime * Speed;
            transform.rotation = Quaternion.Slerp(Begin.rotation, End.rotation, slerpValue);
            yield return null;
        } while (slerpValue <= 1f);
    }

    /// <summary>
    /// Lerp
    /// </summary>
    /// <remarks>Move smoothly between [0, 180), clockwise or not.</remarks>
    IEnumerator DoLerp()
    {
        float slerpValue = 0f;
        do
        {
            slerpValue += Time.deltaTime * Speed;
            transform.rotation = Quaternion.Lerp(Begin.rotation, End.rotation, slerpValue);
            yield return null;
        } while (slerpValue <= 1f);
    }

    IEnumerator DoMultiple()
    {
        int counter = 0;
        do
        {
            var absoluteDirection = transform.rotation * MultipleDirection;
            transform.position += absoluteDirection * Time.deltaTime;
            yield return null;
        } while (++counter >= 100);
    }

    // Use this for initialization
    void Start()
    {

    }
}
