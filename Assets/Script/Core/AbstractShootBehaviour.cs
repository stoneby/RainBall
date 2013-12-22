using System;
using UnityEngine;

public abstract class AbstractShootBehaviour : MonoBehaviour
{
    public bool ShootEndEnabled;
    public event EventHandler<EventArgs> ShootEnd;

    public abstract void Shoot(GameObject shootObject, GameObject hitObject);

    public void EndShoot()
    {
        if (ShootEndEnabled && ShootEnd != null)
        {
            Debug.Log("End Shoot.");
            ShootEnd(this, new EventArgs());
        }
    }
}
