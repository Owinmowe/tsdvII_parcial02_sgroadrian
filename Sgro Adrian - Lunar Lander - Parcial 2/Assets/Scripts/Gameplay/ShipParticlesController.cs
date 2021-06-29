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

    ParticleSystem pSystem = null;
    ParticleSystem.EmissionModule emissionModule = default;

    bool accelerating = false;

    bool paused = false;

    private void Awake()
    {
        pSystem = GetComponent<ParticleSystem>();
        emissionModule = pSystem.emission;
        if(ship != null)
        {
            ship.OnAcceleration += IncrementParticles;
            PlayerInput.OnPausePressed += TogglePauseSystem;
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

    void TogglePauseSystem()
    {
        paused = !paused;
        if (paused) pSystem.Pause();
        else pSystem.Play();
    }

}
