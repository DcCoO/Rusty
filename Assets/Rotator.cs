using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField] bool auto;
    
    [Header("Automatic Config")]
    [SerializeField] float autoSpeed;

    [Header("Manual Config")]
    [SerializeField] float manualMovePercent;
    [SerializeField] float manualAcceleration;
    [SerializeField] float manualMaxSpeed;
    [SerializeField] float manualSpeed;

    Transform tf;
    float sw;
    void Start()
    {
        tf = transform;
        sw = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if(auto) tf.Rotate(0, autoSpeed * Time.deltaTime, 0); 
        else if(Input.GetMouseButton(0))
        {
            float mouseX = Input.mousePosition.x;
            if(mouseX < manualMovePercent * sw)
            {
                tf.Rotate(0, manualSpeed * Time.deltaTime, 0);
                Chisel.instance.Disappear();
            }
            else if (mouseX > (1 - manualMovePercent) * sw)
            {
                tf.Rotate(0, -manualSpeed * Time.deltaTime, 0);
                Chisel.instance.Disappear();
            }
            else
            {
                manualSpeed = 0;
            }
            manualSpeed = Mathf.Clamp(manualSpeed + manualAcceleration * Time.deltaTime, 0, manualMaxSpeed);
        }
        else
        {
            manualSpeed = 0;
        }
    }
}
