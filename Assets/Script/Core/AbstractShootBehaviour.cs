using System;
using UnityEngine;

public abstract class AbstractShootBehaviour : MonoBehaviour
{
    public event EventHandler<EventArgs> ShootEnd;

    public abstract void Shoot(GameObject shootObject, GameObject hitObject);

    public void EndShoot()
    {
        Debug.Log("End Shoot.");

        if (ShootEnd != null)
        {
            ShootEnd(this, new EventArgs());
        }
    }
}
