using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickParticles : MonoBehaviour
{
    ParticleSystem _particle; 
    
    void Awake()
    {
        _particle = gameObject.GetComponent<ParticleSystem>();
        Destroy(gameObject, _particle.main.duration);
    }
}
