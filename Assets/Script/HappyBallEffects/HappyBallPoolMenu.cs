using UnityEngine;

public class HappyBallPoolMenu : MonoBehaviour
{
    public JumpingEffect Jump;
    public GameIdleState IdleState;

    void OnGUI()
    {
        if(GUILayout.Button("Jump Effect"))
        {
            Jump.ItemList = Utils.BallChainManager.ToList();
            Jump.Go();
        }

        if (GUILayout.Button("Gaff", GUILayout.Width(100), GUILayout.Height(100)))
        {
            IdleState.Gaffing();
        }

        if (GUILayout.Button("Play", GUILayout.Width(100), GUILayout.Height(100)))
        {
            IdleState.Exit();
        }
    }
}
