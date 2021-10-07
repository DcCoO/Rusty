using UnityEngine;

public class Hose : SingletonMonoBehaviour<Hose>
{
    Transform tf;
    [SerializeField] ParticleSystem waterBeam, waterSteam;
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
        if(state)
        {
            if (!isPlaying)
            {
                waterBeam.Play();
                waterSteam.Play();
            }
            isPlaying = true;
        }
        else
        {
            waterBeam.Stop();
            waterSteam.Stop();
            isPlaying = false;
        }
    }

    public void LookAt(Vector3 position)
    {
        tf.LookAt(position, Vector3.up);
        waterSteam.transform.position = position;
    }
}
