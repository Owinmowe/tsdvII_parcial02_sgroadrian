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
    public Action OnRotation;
    public Action<bool> OnLanding;
    public Action<Vector2> OnVelocityChange;

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
    }

    private void Update()
    {
        if(!shipLocked) OnVelocityChange?.Invoke(rb.velocity);
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
                Vector2 force = transform.up * shipConfiguration.accelerateSpeed;
                rb.AddForce(force, ForceMode2D.Force);
                OnAcceleration?.Invoke();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool correctLandingAngle = Vector2.Angle(transform.up, Vector3.up) < shipConfiguration.landingAngleTolerance;
        bool correctLandingPosition = Physics2D.Raycast(transform.position, Vector2.down, shipConfiguration.checkDistance, shipConfiguration.landingLayer);
        bool correctLandingSpeed = rb.velocity.magnitude < shipConfiguration.landingSpeedTolerance;
        OnLanding?.Invoke(correctLandingAngle && correctLandingPosition && correctLandingSpeed);
        shipLocked = true;
        rb.Sleep();
    }
}
