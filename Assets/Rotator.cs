using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float speed;
    Transform tf;
    void Start()
    {
        tf = transform;
    }

    // Update is called once per frame
    void Update()
    {
        tf.Rotate(0, speed * Time.deltaTime, 0);    
    }
}
