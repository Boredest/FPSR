using UnityEngine;
using System;

public class GunAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
       animator = GetComponent<Animator>();
       Q3Movement.Q3PlayerController.OnWalkingStateChanged += UpdateWalkAnimation;
       PlayerShoot.shootInput += PlayShotAnimation;
       
    }

    private void PlayShotAnimation()
    {
        animator.SetTrigger("Shoot");

    }

    private void UpdateWalkAnimation(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }


    private void OnDestroy()
    {
      Q3Movement.Q3PlayerController.OnWalkingStateChanged -= UpdateWalkAnimation;
        PlayerShoot.shootInput -= PlayShotAnimation;
    }
}
