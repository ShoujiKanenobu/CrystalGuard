using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimateOnStart : MonoBehaviour
{
    [SerializeField]
    private AnimationClip anim;
    void Start()
    {
        this.GetComponent<Animator>().Play(anim.name);
    }
}
