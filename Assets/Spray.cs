using UnityEngine;

public class Spray : SingletonMonoBehaviour<Spray>
{
    Transform tf;
    [SerializeField] ParticleSystem sprayBeam, spraySteam;
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
                sprayBeam.Play();
                spraySteam.Play();
            }
            isPlaying = true;
        }
        else
        {
            sprayBeam.Stop();
            spraySteam.Stop();
            isPlaying = false;
        }
    }

    public void LookAt(Vector3 position)
    {
        tf.LookAt(position, Vector3.up);
        spraySteam.transform.position = position;
    }
}
