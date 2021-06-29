using System;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { TurnLeft, TurnRight, Accelerate}

public class Ship: MonoBehaviour
{

    [SerializeField] ShipConfiguration shipConfiguration = null;

    Rigidbody2D rb = null;
    CapsuleCollider2D col = null;

    public Action OnShipReset;
    public Action OnAcceleration;
    public Action<int> OnScoreGet;
    public Action<bool, float> OnAltitudeChange;
    public Action<float, float> OnFuelConsumed;
    public Action<int> OnPointsRecieved;
    public Action OnRotation;
    public Action<bool> OnLanding;
    public Action OnOutOfMoonGravity;
    public Action<Vector2> OnVelocityChange;

    float currentFuel;
    float currentAngle = 0;
    Vector2 savedVelocity = Vector2.zero;
    Vector3 startingPosition = Vector3.zero;
    bool shipLocked = false;
    int currentLimit = 0;

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

        startingPosition = transform.position;
    }

    private void Start()
    {
        OnFuelConsumed?.Invoke(currentFuel, shipConfiguration.maxFuel);
        OnScoreGet?.Invoke(0);
    }

    private void Update()
    {
        if (!shipLocked)
        {
            OnVelocityChange?.Invoke(rb.velocity);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, shipConfiguration.maxTerrainCheckDistance, shipConfiguration.terrainLayer);
            OnAltitudeChange?.Invoke(hit, hit.distance - col.size.y / 2);
            if (transform.position.y > currentLimit)
            {
                OnOutOfMoonGravity?.Invoke();
                rb.drag = 0;
                rb.gravityScale = 0;
                shipLocked = true;
            }
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
                OnFuelConsumed?.Invoke(currentFuel, shipConfiguration.maxFuel);
                OnAcceleration?.Invoke();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckIfLanded(collision.gameObject.layer))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, shipConfiguration.maxLandingCheckDistance, shipConfiguration.landingLayer);
            bool correctLandingAngle = Vector2.Angle(transform.up, Vector3.up) < shipConfiguration.landingAngleTolerance;
            bool correctLandingSpeed = rb.velocity.magnitude < shipConfiguration.landingSpeedTolerance;
            bool correctLanding = correctLandingAngle && hit && correctLandingSpeed;
            OnLanding?.Invoke(correctLanding);

            if (correctLanding) OnScoreGet?.Invoke(hit.collider.gameObject.GetComponentInParent<LandingSite>().GetScore());

            OnVelocityChange?.Invoke(Vector2.zero);
            OnAltitudeChange?.Invoke(true, 0);
            shipLocked = true;
            rb.Sleep();
        }
    }

    bool CheckIfLanded(int layer)
    {
        return shipConfiguration.landingLayer == (shipConfiguration.landingLayer | (1 << layer)) ||
            shipConfiguration.terrainLayer == (shipConfiguration.terrainLayer | (1 << layer));
    }

    public void ToggleMovement()
    {
        shipLocked = !shipLocked;
        if (shipLocked)
        {
            savedVelocity = rb.velocity;
            rb.Sleep();
        }
        else
        {
            rb.WakeUp();
            rb.velocity = savedVelocity;
        }
    }

    public void ResetPositionToStart()
    {
        transform.position = startingPosition;
        OnShipReset?.Invoke();
        shipLocked = false;
        rb.WakeUp();
    }

    public void SetLimitY(int limit)
    {
        currentLimit = limit;
    }

}
