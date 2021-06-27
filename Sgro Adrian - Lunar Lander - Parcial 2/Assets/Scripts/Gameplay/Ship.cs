using System;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { TurnLeft, TurnRight, Accelerate}

public class Ship: MonoBehaviour
{

    [SerializeField] ShipConfiguration shipConfiguration = null;

    Rigidbody2D rb = null;
    CapsuleCollider2D col = null;

    public Action OnAcceleration;
    public Action<bool, float> OnAltitudeChange;
    public Action<float, float> OnFuelConsumed;
    public Action<int> OnPointsRecieved;
    public Action OnRotation;
    public Action<bool> OnLanding;
    public Action<Vector2> OnVelocityChange;

    float currentFuel;
    float currentAngle = 0;
    bool shipLocked = false;

    private void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = shipConfiguration.gravityScale;
        rb.mass = shipConfiguration.mass;
        rb.drag = shipConfiguration.linearDrag;

        col = gameObject.AddComponent<CapsuleCollider2D>();
        col.direction = shipConfiguration.direction;
        col.size = shipConfiguration.colliderSize;
        col.isTrigger = true;

        currentFuel = shipConfiguration.maxFuel;
    }

    private void Update()
    {
        if (!shipLocked)
        {
            OnVelocityChange?.Invoke(rb.velocity);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, shipConfiguration.maxTerrainCheckDistance, shipConfiguration.terrainLayer);
            OnAltitudeChange?.Invoke(hit, hit.distance);
        }
    }

    public void Move(MovementType moveType)
    {
        if (shipLocked) return;
        switch (moveType)
        {
            case MovementType.TurnLeft:
                currentAngle = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
                if(currentAngle > -shipConfiguration.turnRange / 2)
                {
                    transform.Rotate(Vector3.forward, shipConfiguration.turnSpeed);
                    OnRotation?.Invoke();
                }
                break;
            case MovementType.TurnRight:
                currentAngle = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
                if (currentAngle < shipConfiguration.turnRange / 2)
                {
                    transform.Rotate(Vector3.forward, -shipConfiguration.turnSpeed);
                    OnRotation?.Invoke();
                }
                break;
            case MovementType.Accelerate:
                if (currentFuel < 0) return;
                Vector2 force = transform.up * shipConfiguration.accelerateSpeed;
                rb.AddForce(force, ForceMode2D.Force);
                currentFuel -= Time.deltaTime * shipConfiguration.fuelAcelerationConsumption;
                OnAcceleration?.Invoke();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool correctLandingAngle = Vector2.Angle(transform.up, Vector3.up) < shipConfiguration.landingAngleTolerance;
        bool correctLandingPosition = Physics2D.Raycast(transform.position, Vector2.down, shipConfiguration.maxLandingCheckDistance, shipConfiguration.landingLayer);
        bool correctLandingSpeed = rb.velocity.magnitude < shipConfiguration.landingSpeedTolerance;
        OnLanding?.Invoke(correctLandingAngle && correctLandingPosition && correctLandingSpeed);
        OnVelocityChange?.Invoke(Vector2.zero);
        OnAltitudeChange?.Invoke(true, 0);
        shipLocked = true;
        rb.Sleep();
    }
}
