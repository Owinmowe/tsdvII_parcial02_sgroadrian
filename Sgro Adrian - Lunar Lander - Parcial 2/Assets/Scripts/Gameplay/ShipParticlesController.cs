using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
public class ShipParticlesController : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField] ParticleSystem thrustParticles = null;
    [SerializeField] int minParticlesThrust = 0;
    [SerializeField] int maxParticlesThrust = 100;
    [SerializeField] int particlesAccelerationSpeed = 5;

    ParticleSystem.EmissionModule emissionModule;

    bool accelerating = false;

    private void Awake()
    {
        emissionModule = thrustParticles.emission;
        GetComponent<ShipMovement>().OnAcceleration += IncrementParticles;
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
