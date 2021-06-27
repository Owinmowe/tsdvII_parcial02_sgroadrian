using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class PlayerInput : MonoBehaviour
{

    public static Action OnPausePressed;

    Ship shipMovement = null;

    private void Awake()
    {
        shipMovement = GetComponent<Ship>();
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
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        OnPausePressed?.Invoke();
    }

}
