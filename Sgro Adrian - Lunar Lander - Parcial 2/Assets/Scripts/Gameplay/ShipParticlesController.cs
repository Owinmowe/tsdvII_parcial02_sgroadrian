using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipParticlesController : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField] Ship ship = null;
    [SerializeField] int minParticlesThrust = 0;
    [SerializeField] int maxParticlesThrust = 100;
    [SerializeField] int particlesAccelerationSpeed = 40;

    ParticleSystem.EmissionModule emissionModule;

    bool accelerating = false;

    private void Awake()
    {
        emissionModule = GetComponent<ParticleSystem>().emission;
        if(ship != null)
        {
            ship.OnAcceleration += IncrementParticles;
        }
    }

    void IncrementParticles()
    {
        accelerating = true;
        if (emissionModule.rateOverTime.constant < maxParticlesThrust + particlesAccelerationSpeed)
        {
            emissionModule.rateOverTime = emissionModule.rateOverTime.constant + particlesAccelerationSpeed;
        }
        else
        {
            emissionModule.rateOverTime = particlesAccelerationSpeed;
        }
    }

    private void Update()
    {
        if (accelerating)
        {
            accelerating = false;
            return;
        }
        if (emissionModule.rateOverTime.constant < minParticlesThrust + particlesAccelerationSpeed)
        {
            emissionModule.rateOverTime = minParticlesThrust;
        }
        else
        {
            emissionModule.rateOverTime = emissionModule.rateOverTime.constant - particlesAccelerationSpeed;
        }
    }

}
