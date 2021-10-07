using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : SingletonMonoBehaviour<ParticleController>
{
    [SerializeField] GameObject rustPieces;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public GameObject GetRustPieces() => rustPieces;
}
