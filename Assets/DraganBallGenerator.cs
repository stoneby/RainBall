using System.Collections.Generic;
using UnityEngine;

public class DraganBallGenerator : MonoBehaviour
{
    public GameObject CommanderBall;

    public GameObject KeyBall;

    public int Size;

    public List<Color> ColorList;

    // Use this for initialization
    void Start()
    {
        Generate();
    }

    private void DestroyChildren()
    {
        for (var i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.gameObject == CommanderBall)
            {
                //Destroy(CommanderBall.GetComponent<BallUpdater>());
            }
            else
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public void Generate()
    {
        DestroyChildren();

        var materials = Resources.LoadAll("Materials", typeof(Material));
        if (materials == null)
        {
            Debug.Log("Error loading resources.");
            return;
        }

        Debug.Log("Material loading count: " + materials.Length);

        var parentBall = CommanderBall;
        for (var i = 0; i < Size; ++i)
        {
            var newBall = Instantiate(KeyBall, CommanderBall.transform.position, CommanderBall.transform.rotation) as GameObject;
            newBall.transform.parent = CommanderBall.transform.parent;
            newBall.renderer.material = materials[Random.Range(0, materials.Length)] as Material;
            newBall.name = newBall.renderer.material.name;

            var ballUpdater = parentBall.GetComponent<BallUpdater>();
            if (ballUpdater == null)
            {
                parentBall.AddComponent<BallUpdater>();
            }
            ballUpdater = parentBall.GetComponent<BallUpdater>();
            ballUpdater.BrotherBall = newBall;
            ballUpdater.Name = newBall.name;

            Debug.Log("Parent: " + parentBall.name + ", adding bro: " + ballUpdater.BrotherBall.name);

            parentBall = newBall;
        }
    }
}
