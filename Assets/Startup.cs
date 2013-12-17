using System.Collections;
using UnityEngine;

public class Startup : MonoBehaviour
{
    IEnumerator Start()
    {
        // wait one frame for state machine get ready.
        yield return null;

        Utils.GameStateMachine.Go();
    }
}
