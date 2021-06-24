using System;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { TurnLeft, TurnRight, Accelerate}

[RequireComponent(typeof(Rigidbody2D))]
public class ShipMovement: MonoBehaviour
{
    [SerializeField] float accelerateSpeed = 5f;
    [SerializeField] float turnSpeed = 2f;
    [SerializeField] float mass = 1f;
    [SerializeField] float angularDrag = 0f;
    [SerializeField] float gravityScale = 1f;

    Rigidbody2D rb = null;

    public Action OnAcceleration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.mass = mass;
        rb.angularDrag = angularDrag;
    }

    public void Move(MovementType moveType)
    {
        switch (moveType)
        {
            case MovementType.TurnLeft:
                transform.Rotate(Vector3.forward, turnSpeed);
                //rb.AddTorque(turnSpeed, ForceMode2D.Force);
                break;
            case MovementType.TurnRight:
                transform.Rotate(Vector3.forward, -turnSpeed);
                //rb.AddTorque(-turnSpeed, ForceMode2D.Force);
                break;
            case MovementType.Accelerate:
                Vector2 force = transform.up * accelerateSpeed;
                rb.AddForce(force, ForceMode2D.Force);
                OnAcceleration?.Invoke();
                break;
            default:
                break;
        }
    }
}
