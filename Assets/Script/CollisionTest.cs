using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    // Use this for initialization
    void OnCollisionEnter()
    {
        Debug.Log("OnCollisionEnter");
    }

    void OnCollisionStay()
    {
        Debug.Log("OnCollisionStay");
    }

    void OnCollisionExit()
    {
        Debug.Log("OnCollisionExit");
    }
}
