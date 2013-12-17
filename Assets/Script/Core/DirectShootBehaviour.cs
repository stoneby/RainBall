using UnityEngine;

public class DirectShootBehaviour : AbstractShootBehaviour
{
    public override void Shoot(GameObject shootObject, GameObject hitObject)
    {
        iTween.MoveTo(shootObject,
                      iTween.Hash("position", hitObject.transform.position, "speed", Utils.Settings.ShootSpeed, "easetype",
                                  iTween.EaseType.linear, "oncomplete", "MoveComplete", "oncompletetarget", gameObject));
    }

    void MoveComplete()
    {
        EndShoot();
    }
}