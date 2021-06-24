using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UI_Component : MonoBehaviour
{

    public Action OnTransitionEnd;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void TransitionEnd()
    {
       OnTransitionEnd?.Invoke();
    }

    public void TransitionIn()
    {
        anim.SetTrigger("Transition In");
    }

    public void TransitionOut()
    {
        anim.SetTrigger("Transition Out");
    }
}
