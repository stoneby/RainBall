using UnityEngine;

public enum SelectionType
{
    LeftMouse = 0,
    RightMouse,
    MiddleMouse
}

public class SelectionDetector : MonoBehaviour
{
    public SelectionType Type;

    public Vector3 Point { get; set; }
    public float Duration { get; set; }
    public GameObject SelectedObject { get; set; }

    private float startTime;

    void Update()
    {
        if (Input.GetMouseButtonDown((int)Type))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                startTime = Time.time;
                Point = hit.point;
                SelectedObject = hit.transform.gameObject;
            }
        }
        else if (Input.GetMouseButtonUp((int)Type))
        {
            Duration = Time.time - startTime;

            if (SelectedObject != null)
            {
                Debug.Log("Selection Detecting: Point-" + Point + ", SelectedObject-" + SelectedObject.name + ", Duration-" +
                          Duration);

                var extents = SelectedObject.collider.bounds.extents;
                Debug.Log("extents of collider is: " + extents);

                Debug.DrawRay(SelectedObject.transform.position, SelectedObject.collider.bounds.extents);
                Debug.DrawRay(SelectedObject.transform.position, new Vector3(-extents.x, extents.y, extents.z));
                Debug.DrawRay(SelectedObject.transform.position, new Vector3(extents.x, extents.y, -extents.z));
                Debug.DrawRay(SelectedObject.transform.position, new Vector3(-extents.x, extents.y, -extents.z));
            }
        }
    }
}
