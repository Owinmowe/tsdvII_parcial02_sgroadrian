using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class PlayerInput : MonoBehaviour
{

    static public Action OnPausePressed;

    Ship shipcomponent = null;

    private void Awake()
    {
        shipcomponent = GetComponent<Ship>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            shipcomponent.Move(MovementType.Accelerate);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            shipcomponent.Move(MovementType.TurnLeft);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            shipcomponent.Move(MovementType.TurnRight);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        OnPausePressed?.Invoke();
    }

}
