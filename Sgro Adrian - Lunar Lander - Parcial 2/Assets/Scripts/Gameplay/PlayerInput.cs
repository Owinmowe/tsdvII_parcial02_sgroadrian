using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
public class PlayerInput : MonoBehaviour
{

    ShipMovement shipMovement = null;

    private void Awake()
    {
        shipMovement = GetComponent<ShipMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            shipMovement.Move(MovementType.Accelerate);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            shipMovement.Move(MovementType.TurnLeft);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            shipMovement.Move(MovementType.TurnRight);
        }
    }
}
