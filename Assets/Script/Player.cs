using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 3f;

    public float SensitivityX = 15f;
    public float SensitivityY = 15f;

    public bool FPS;

    // Update is called once per frame
    void Update()
    {
        var horuizontalStep = Input.GetAxis("Horizontal");
        var verticalStep = Input.GetAxis("Vertical");
        transform.position += new Vector3(horuizontalStep * Time.deltaTime * Speed, 0, verticalStep * Time.deltaTime * Speed);

        if (FPS)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * SensitivityX, 0);
            transform.Rotate(-Input.GetAxis("Mouse Y") * SensitivityX, 0, 0);
        }
    }
}
