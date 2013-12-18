using UnityEngine;

public class PaintedEggController : MonoBehaviour
{
    public GameObject TemplateBall;

    void OnGUI()
    {
        if (GUILayout.Button("Go"))
        {
            Go();
        }
    }

    private void Go()
    {
        Move("I");
        Move("G");
        Move("T");
    }

    private void Move(string path)
    {
        var iBall = Instantiate(TemplateBall) as GameObject;
        var moveEvent = iTweenEvent.GetEvent(iBall, "Move");
        moveEvent.Values["path"] = path;
        moveEvent.Values.Remove("onstart");
        moveEvent.Values.Remove("oncomplete");
        moveEvent.Play();
    }
}
