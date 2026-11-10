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


    void Update()
    {
        if(Jump)
        {
            _animator.SetTrigger("Jumped");
            

            Vector3 DirectionForce = _rootTransform.forward + (Vector3.up*jumpPower);


            _rigidBody.AddForce(DirectionForce,ForceMode.Impulse);
            Jump = false;
        }
        _animator.SetBool("IsJumping", isJumping);


    }
}
