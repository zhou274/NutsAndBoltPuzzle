using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastObjectAnimations : MonoBehaviour
{
    //public DOTweenAnimation[] animations;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        // animations = GetComponents<DOTweenAnimation>();
        StartCoroutine(StartAnimations());
    }

    private IEnumerator StartAnimations()
    {
        yield return new WaitForSeconds(3f);
        animator.enabled = true;
       /* int randomNUm = Random.Range(0, animations.Length);
        animations[randomNUm].DORestart();
        yield return new WaitForSeconds(3f);
        StartCoroutine(StartAnimations());*/
    }
}
