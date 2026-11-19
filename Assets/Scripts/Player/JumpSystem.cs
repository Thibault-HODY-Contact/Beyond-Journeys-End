using System;
using Unity.VisualScripting;
using UnityEngine;

public class JumpSystem : MonoBehaviour
{
    [SerializeField] Transform _rootTransform;

    [SerializeField] Animator _animator;
    [SerializeField] Camera _camera;
    [SerializeField] Rigidbody _rigidBody;
    [SerializeField] Movement _Movement;
    [SerializeField] float jumpPower;

    public bool Jump = false;

    public bool isJumping = false;

    public bool isGrounded = true;


    public bool canJump = true;

    public float Cooldown = 0.5f;

    float timer = 0;

    void Update()
    {
        if (Jump && canJump && Time.timeScale > .5f) 
        {
            _animator.SetTrigger("Jumped");
            

            Vector3 DirectionForce = _rootTransform.forward + (Vector3.up*jumpPower);


            _rigidBody.AddForce(DirectionForce,ForceMode.Impulse);
            Jump = false;
        }
        _animator.SetBool("IsJumping", isJumping);


        if(!canJump&& timer < Cooldown)
        {
            timer += Time.unscaledDeltaTime;
        }else if(!canJump && timer >= Cooldown&& isGrounded)
        {
            timer = 0;
            canJump = true;
        }

    }
}
