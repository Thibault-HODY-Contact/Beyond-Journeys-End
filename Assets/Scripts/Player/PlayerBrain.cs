using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] InputActionReference _movementInput;
    [SerializeField] InputActionReference _runInput;
    [SerializeField] InputActionReference _JumpInput;

    [SerializeField] InputActionReference _Attack1Input;

    [SerializeField] InputActionReference _UltimateInput;

    [SerializeField] InputActionReference _spell1Input;

    [SerializeField] Animator _animator;

    [SerializeField] Movement _moveSystem;
    [SerializeField] JumpSystem _jumpSystem;

    [SerializeField] AttackSystem _Attack1;
    [SerializeField] AttackSystem _UltimateAttack;
    [SerializeField] List<PlayableDirector> _PlayerDirectors;
    [SerializeField] List<TimelineAsset> _UltimesTimeLines;

    [SerializeField] AttackSystem _spell1;

    [SerializeField] ShieldSystem _shieldSystem;
    [SerializeField] InputActionReference _shieldInput;

    Vector2 _moveInput;


    void Start()
    {
        if (_movementInput != null)
        {

            _movementInput.action.performed += UpdateMove;
            _movementInput.action.canceled += UpdateMove;

        }

        if (_runInput != null)
        {

            _runInput.action.performed += UpdateRun;
            _runInput.action.canceled += UpdateRun;

        }

        if (_JumpInput != null)
        {

            _JumpInput.action.performed += UpdateJump;
            _JumpInput.action.canceled += UpdateJump;

        }

        if (_Attack1Input != null)
        {

            _Attack1Input.action.performed += UpdateAttack1;
            _Attack1Input.action.canceled += UpdateAttack1;

        }

        if (_Attack1Input != null)
        {

            _UltimateInput.action.performed += UpdateUltimateAttack;
            _UltimateInput.action.canceled += UpdateUltimateAttack;

        }
        if (_spell1Input != null)
        {
            _spell1Input.action.performed += UpdateSpell1;
            _spell1Input.action.canceled += UpdateSpell1;
        }

        if (_shieldInput != null)
        {
            _shieldInput.action.performed += UpdateShield;
        }
    }


    private void OnDestroy()
    {
        if (_movementInput == null) return;

        _movementInput.action.performed -= UpdateMove;
        _movementInput.action.canceled -= UpdateMove;

        if (_runInput == null) return;
        _runInput.action.performed -= UpdateRun;
        _runInput.action.canceled -= UpdateRun;

        if (_JumpInput == null) return;
        _JumpInput.action.performed -= UpdateJump;
        _JumpInput.action.canceled -= UpdateJump;

        if (_Attack1Input == null) return;
        _Attack1Input.action.performed -= UpdateAttack1;
        _Attack1Input.action.canceled -= UpdateAttack1;

        if (_Attack1Input == null) return;
        _UltimateInput.action.performed -= UpdateUltimateAttack;
        _UltimateInput.action.canceled -= UpdateUltimateAttack;

        if (_spell1Input == null) return;
        _spell1Input.action.performed -= UpdateSpell1;
        _spell1Input.action.canceled -= UpdateSpell1;

        if (_shieldInput != null)
            _shieldInput.action.performed -= UpdateShield;
    }

    void UpdateMove(InputAction.CallbackContext context)
    {
        if (_Attack1.StopMovement || _spell1.StopMovement || _shieldSystem.StopMovement)
            return;

        _moveInput = context.ReadValue<Vector2>(); 
    }

    void Update()
    {
        if (_Attack1.StopMovement || _spell1.StopMovement || _shieldSystem.StopMovement)
            _moveSystem.MoveDirection = Vector2.zero;
        else
            _moveSystem.MoveDirection = _moveInput;
    }


    void UpdateRun(InputAction.CallbackContext context)
    {
        _moveSystem.isRunning = context.action.WasPressedThisFrame();
    }

    void UpdateJump(InputAction.CallbackContext context)
    {
        if (_Attack1.StopMovement) return;          
        _jumpSystem.Jump = context.action.WasPressedThisFrame();
    }

    void UpdateAttack1(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && !_jumpSystem.Jump && _jumpSystem.isGrounded)
        {
            _animator.SetTrigger("Attack1");
            _Attack1.StopMovement = true;          
        }
    }
    private void UpdateUltimateAttack(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && !_jumpSystem.Jump && _jumpSystem.isGrounded)
        {
            for (int i = 0; i < _PlayerDirectors.Count; i++)
            {
                _PlayerDirectors[i].playableAsset = _UltimesTimeLines[i];
                _PlayerDirectors[i].Play();
            }
        }

    }


    void UpdateSpell1(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && !_jumpSystem.Jump && _jumpSystem.isGrounded && _spell1.CanAttack)
        {
            _animator.SetTrigger("Spell1");
            _spell1.StopMovement = true;  
        }
    }

    void UpdateShield(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            if(_shieldSystem.CanShield) _animator.SetTrigger("Shield");
            _shieldSystem.ActivateShield();
        }
    }
}
