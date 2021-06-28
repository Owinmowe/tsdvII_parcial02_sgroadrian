using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimationController : MonoBehaviour
{
    [SerializeField] Ship ship = null;
    [SerializeField] string shipExplosionTrigger = "Explosion";
    [SerializeField] string shipSuccessTrigger = "Landed";
    [SerializeField] string shipResetTrigger = "Reset";

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
            ship.OnShipReset += ShipReset;
        }   
    }

    void ShipLandAnimation(bool collisionSuccess)
    {
        if (collisionSuccess) anim.SetTrigger(shipSuccessTrigger);
        else anim.SetTrigger(shipExplosionTrigger);
    }

    void ShipReset()
    {
        anim.SetTrigger(shipResetTrigger);
    }

}
