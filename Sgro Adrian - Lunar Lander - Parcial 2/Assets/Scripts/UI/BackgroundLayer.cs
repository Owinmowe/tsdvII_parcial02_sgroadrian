using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLayer : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    float baseSpeedMultiplier = 0;
    Vector2 screenSize = Vector2.zero;

    public void SetScreenSize(Vector2 size)
    {
        screenSize = size;
    }

    public void SetBaseSpeedMultiplier(float multiplier)
    {
        baseSpeedMultiplier = multiplier;
    }

    public void SetVelocity(Vector2 _velocity)
    {
        velocity = _velocity * baseSpeedMultiplier;
    }

    public void UpdateLayer()
    {
        transform.position += velocity;
        if(transform.position.x > screenSize.x)
        {
            transform.position = new Vector3(-screenSize.x, transform.position.y, 0);
        }
        else if(transform.position.x < -screenSize.x)
        {
            transform.position = new Vector3(screenSize.x, transform.position.y, 0);
        }
        if (transform.position.y > screenSize.y)
        {
            transform.position = new Vector3(transform.position.x, -screenSize.y, 0);
        }
        else if (transform.position.y < -screenSize.y)
        {
            transform.position = new Vector3(transform.position.x, screenSize.y, 0);
        }
    }
}
