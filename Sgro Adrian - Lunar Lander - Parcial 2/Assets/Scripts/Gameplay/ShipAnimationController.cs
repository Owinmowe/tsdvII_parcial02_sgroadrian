using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimationController : MonoBehaviour
{
    [SerializeField] Ship ship = null;
    [SerializeField] string shipExplosionTrigger = "Explosion";
    [SerializeField] string shipSuccessTrigger = "Landed";

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if(ship != null)
        {
            ship.OnLanding += ShipLandAnimation;
        }   
    }

    void ShipLandAnimation(bool collisionSuccess)
    {
        if (collisionSuccess) anim.SetTrigger(shipSuccessTrigger);
        else anim.SetTrigger(shipExplosionTrigger);
    }

}
