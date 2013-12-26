using UnityEngine;

public class PaintedEggController : MonoBehaviour
{
    public MagicShow Magic;
    
    void OnGUI()
    {
        if (GUILayout.Button("Paint Egg Show"))
        {
            Go();
        }
    }

    private void Go()
    {
        Magic.ShowTime();
    }
}
