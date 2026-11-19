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
    [SerializeField] InputActionReference _TimeStopInput;

    [SerializeField] InputActionReference _spell1Input;

    [SerializeField] Animator _animator;

    [SerializeField] Movement _moveSystem;
    [SerializeField] JumpSystem _jumpSystem;

    [SerializeField] AttackSystem _Attack1;
    [SerializeField] AttackSystem _UltimateAttack; 
    [SerializeField] List<PlayableDirector> _PlayerDirectors;
    [SerializeField] List<TimelineAsset> _UltimesTimeLines;
    [SerializeField] TimelineAsset _TimeStopTimeLine;

    [SerializeField] AttackSystem _spell1;

    [SerializeField] ShieldSystem _shieldSystem;
    [SerializeField] InputActionReference _shieldInput;

    Vector2 _moveInput;

    
    Dictionary<Transform, Quaternion> _timelineRestRotations = new Dictionary<Transform, Quaternion>();


    void Start()
    {
        foreach (var director in _PlayerDirectors)
        {
            if (director != null)
                director.stopped += OnTimelineStopped;
        }

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

        if (_UltimateInput != null)
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
        if (_TimeStopInput != null)
        {

            _TimeStopInput.action.performed += UpdateTimeStopInput;
            _TimeStopInput.action.canceled += UpdateTimeStopInput;

        }

    }

    

    private void OnDestroy()
    {
        foreach (var director in _PlayerDirectors)
        {
            if (director != null)
                director.stopped -= OnTimelineStopped;
        }

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

        if (_UltimateInput == null) return;
        _UltimateInput.action.performed -= UpdateUltimateAttack;
        _UltimateInput.action.canceled -= UpdateUltimateAttack;

        if (_spell1Input == null) return;
        _spell1Input.action.performed -= UpdateSpell1;
        _spell1Input.action.canceled -= UpdateSpell1;

        if (_shieldInput != null)
            _shieldInput.action.performed -= UpdateShield;
        if (_TimeStopInput == null) return;
        _TimeStopInput.action.performed -= UpdateTimeStopInput;
        _TimeStopInput.action.canceled -= UpdateTimeStopInput;
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
            if (AnyTimelinePlaying()) return;

            for (int i = 0; i < _PlayerDirectors.Count; i++)
            {
                PlayTimeline(_PlayerDirectors[i], _UltimesTimeLines[i]);
            }
        }

    }

    bool AnyTimelinePlaying()
    {
        foreach (var director in _PlayerDirectors)
        {
            if (director != null && director.state == PlayState.Playing)
                return true;
        }
        return false;
    }

    void PlayTimeline(PlayableDirector director, TimelineAsset timeline)
    {
        director.playableAsset = timeline;

        foreach (var output in timeline.outputs)
        {
            if (director.GetGenericBinding(output.sourceObject) is Animator anim && anim != null
                && !_timelineRestRotations.ContainsKey(anim.transform))
            {
                _timelineRestRotations[anim.transform] = anim.transform.localRotation;
            }
        }

        director.Play();
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        if (!(director.playableAsset is TimelineAsset timeline)) return;

        foreach (var output in timeline.outputs)
        {
            if (director.GetGenericBinding(output.sourceObject) is Animator anim && anim != null
                && _timelineRestRotations.TryGetValue(anim.transform, out var restRotation))
            {
                anim.transform.localRotation = restRotation;
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
    private void UpdateTimeStopInput(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && !_jumpSystem.Jump && _jumpSystem.isGrounded)
        {
            if (AnyTimelinePlaying()) return;

            PlayTimeline(_PlayerDirectors[0], _TimeStopTimeLine);
        }
    }
}
