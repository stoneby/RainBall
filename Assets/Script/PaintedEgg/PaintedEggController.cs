using UnityEngine;

public class PaintedEggController : MonoBehaviour
{
    public MagicShow Magic;
    
    void OnGUI()
    {
        if (GUILayout.Button("Go"))
        {
            Go();
        }
    }

    private void Go()
    {
        Magic.ShowTime();
    }
}
