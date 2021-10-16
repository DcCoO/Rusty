using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMesh : MonoBehaviour
{
    void Start()
    {
        RustyBreakController.instance.Reset(transform);
    }

    void Update()
    {
        
    }
}
