using UnityEngine;

public class Laser : SingletonMonoBehaviour<Laser>
{
    Transform tf;
    [SerializeField] ParticleSystem laserBeam, laserSteam;
    bool isPlaying;

    void Start()
    {
        tf = transform;
    }

    void Update()
    {

    }

    public void SetState(bool state)
    {
        if (state)
        {
            if (!isPlaying)
            {
                laserBeam.Play();
                laserSteam.Play();
            }
            isPlaying = true;
        }
        else
        {
            laserBeam.Stop();
            laserSteam.Stop();
            isPlaying = false;
        }
    }

    public void LookAt(Vector3 position, Vector3 normal)
    {
        tf.LookAt(position, Vector3.up);
        laserSteam.transform.position = position;
        laserSteam.transform.LookAt(position + normal);
    }
}
