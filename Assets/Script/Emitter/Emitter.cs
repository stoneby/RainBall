using UnityEngine;

public class Emitter : MonoBehaviour
{
    public Transform Target;
    public GameObject Bomb;

    void Update()
    {
        RaycastHit hit;
        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(cameraRay.origin, cameraRay.direction, out hit, 100))
        {
            Target.position = hit.point;
        }

        if(Input.GetMouseButtonDown(0))
        {
            var newBomb = (GameObject)Instantiate(Bomb, transform.position, Quaternion.identity);
            var newBombScript = newBomb.GetComponent<Bomb>();
            newBombScript.TargetPosition = Target.position;
        }
    }
}