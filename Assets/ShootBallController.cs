using UnityEngine;

public class ShootBallController : MonoBehaviour
{
    public GameObject ShootBall;

    private ParticleSystem burning;

    public void StartBurning()
    {
        burning.Play();
    }

    public void StopBurning()
    {
        burning.Stop();
    }

    void Awake()
    {
        burning = transform.GetChild(0).GetChild(0).particleSystem;
    }
}
