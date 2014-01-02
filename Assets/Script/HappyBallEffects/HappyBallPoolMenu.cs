using UnityEngine;

public class HappyBallPoolMenu : MonoBehaviour
{
    public JumpingEffect Jump;

    void OnGUI()
    {
        if(GUILayout.Button("Jump Effect"))
        {
            if(Jump.ItemList == null)
            {
                Jump.ItemList = Utils.BallChainManager.ToList();
            }
            Jump.Go();
        }
    }
}
