using UnityEngine;
using System.Collections;

public class CameraTest : MonoBehaviour
{
    public GameObject RootObject;

    // Use this for initialization
	void Start () {
        Debug.Log("Camera: " + Camera.main.nearClipPlane + ", far: " + Camera.main.farClipPlane);
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(1))
	    {
            var mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
	        var screenPoint = Camera.main.WorldToScreenPoint(RootObject.transform.position);

            Debug.Log("Mouse screen: " + Input.mousePosition + ", world: " + mousePoint);
            Debug.Log("Root object world: " + RootObject.transform.position + ", screen: " + screenPoint);
	    }
	}
}
