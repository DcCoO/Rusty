using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chisel : SingletonMonoBehaviour<Chisel>
{
    Transform tf;

    void Start()
    {
        tf = transform;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 position, float angle)
    {
        StartCoroutine(MoveRoutine(position, angle));
    }

    IEnumerator MoveRoutine(Vector3 position, float angle)
    {
        Vector3 begin = tf.position;
        Vector3 beginEa = tf.localEulerAngles, endEa = new Vector3(tf.localEulerAngles.x, angle, 0);
        for(float t = 0; t < 1; t += Time.deltaTime * 3)
        {
            tf.position = Vector3.Lerp(begin, position, -(Mathf.Cos(Mathf.PI * t) - 1) * 0.5f);
            //tf.localEulerAngles = Vector3.Lerp(beginEa, endEa, t);
            yield return null;
        }
        tf.position = position;
    }

    public void Disappear()
    {

        StartCoroutine(DisappearRoutine());
    }

    IEnumerator DisappearRoutine()
    {
        Vector3 begin = tf.position;
        for(float t = 0; t < 1; t += Time.deltaTime * 4)
        {
            tf.position = Vector3.Lerp(begin, 0.3f * Vector3.up, t);
            yield return null;
        }
        tf.position = 0.3f * Vector3.up;
    }
}
