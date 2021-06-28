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
        transform.localPosition += velocity;
        if(transform.localPosition.x > screenSize.x)
        {
            transform.localPosition = new Vector3(-screenSize.x, transform.localPosition.y, 0);
        }
        else if(transform.localPosition.x < -screenSize.x)
        {
            transform.localPosition = new Vector3(screenSize.x, transform.localPosition.y, 0);
        }
        if (transform.localPosition.y > screenSize.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -screenSize.y, 0);
        }
        else if (transform.localPosition.y < -screenSize.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, screenSize.y, 0);
        }
    }
}
