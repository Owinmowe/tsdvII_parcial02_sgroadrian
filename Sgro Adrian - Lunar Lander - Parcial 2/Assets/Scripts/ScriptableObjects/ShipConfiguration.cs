using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship Data", menuName = "Ship/Data")]
public class ShipConfiguration : ScriptableObject
{
    [Header("Movement")]
    public float accelerateSpeed = .2f;
    public float turnSpeed = 1f;
    [Range(0, 360)] public float turnRange = 180;
    public float mass = 1f;
    public float linearDrag = .1f;
    public float gravityScale = .01f;

    [Header("Collision")]
    public CapsuleDirection2D direction = CapsuleDirection2D.Vertical;
    public Vector2 colliderSize = new Vector2(.4f, .6f);

    [Header("Ground Check")]
    public LayerMask terrainLayer = default;
    public float maxTerrainCheckDistance = 1f;

    [Header("Landing")]
    public float landingAngleTolerance = 10f;
    public float landingSpeedTolerance = 10f;
    public LayerMask landingLayer = default;
    public float maxLandingCheckDistance = .5f;

    [Header("Fuel")]
    public float maxFuel = 1000;
    public float fuelAcelerationConsumption = .1f; 

}
