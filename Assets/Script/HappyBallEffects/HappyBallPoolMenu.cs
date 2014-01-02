using UnityEngine;

public class HappyBallPoolMenu : MonoBehaviour
{
    public JumpingEffect Jump;

    void OnGUI()
    {
        if(GUILayout.Button("Jump Effect"))
        {
            Jump.ItemList = Utils.BallChainManager.ToList();
            Jump.Go();
        }
    }
}
