using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneController : MonoBehaviour
{

    [SerializeField] int particlesMultiplier = 10;
    ParticleSystem pSystem = null;
    ParticleSystem.EmissionModule emissionModule = default;
    AreaEffector2D windZoneEffector;

    private void Awake()
    {
        windZoneEffector = GetComponent<AreaEffector2D>();
        pSystem = GetComponentInChildren<ParticleSystem>();
        emissionModule = pSystem.emission;
    }

    public void SetWindzone(float baseForce, float variance)
    {
        windZoneEffector.forceMagnitude = baseForce;
        windZoneEffector.forceVariation = variance;
        emissionModule.rateOverTime = baseForce * particlesMultiplier;
    }
}
