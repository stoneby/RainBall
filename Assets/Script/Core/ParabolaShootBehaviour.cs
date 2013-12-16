using System.Collections;
using UnityEngine;

public class ParabolaShootBehaviour : AbstractShootBehaviour
{
    public float EruptHeight;
    public float EruptTime;

    public override void Shoot(GameObject shootObject, GameObject hitObject)
    {
        StartCoroutine(DoShoot(shootObject, hitObject));
    }

    IEnumerator DoShoot(GameObject shootObject, GameObject hitObject)
    {
        var targetPosition = hitObject.transform.position;
        var shootBall = shootObject.transform.GetChild(0).gameObject;

        iTween.MoveBy(shootBall, iTween.Hash("z", EruptHeight, "time", EruptTime / 2, "easeType", iTween.EaseType.easeOutQuad));
        iTween.MoveBy(shootBall,
                      iTween.Hash("z", -EruptHeight, "time", EruptTime / 2, "delay", EruptTime / 2, "easeType",
                                  iTween.EaseType.easeInCubic));
        iTween.MoveTo(shootObject,
                      iTween.Hash("position", targetPosition, "time", EruptTime, "easeType", iTween.EaseType.linear));

        yield return new WaitForSeconds(EruptTime);

        EndShoot();
    }
}
