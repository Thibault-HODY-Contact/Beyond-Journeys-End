using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] InputActionReference _movementInput;
    [SerializeField] InputActionReference _runInput;
    [SerializeField] InputActionReference _JumpInput;

    [SerializeField] InputActionReference _Attack1Input;

    [SerializeField] Animator _animator;

    [SerializeField] Movement _moveSystem;
    [SerializeField] JumpSystem _jumpSystem;

    [SerializeField] AttackSystem _Attack1;



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

    }

    void Update()
    {
        
    }


    void UpdateMove(InputAction.CallbackContext context)
    {
        if (!_Attack1.Attacking)
        {
            _moveSystem.canMove = true;
            _moveSystem.MoveDirection = context.ReadValue<Vector2>(); 
        }
    }

    void UpdateRun(InputAction.CallbackContext context)
    {
        _moveSystem.isRunning = context.action.WasPressedThisFrame();
    }

    void UpdateJump(InputAction.CallbackContext context)
    {
        _jumpSystem.Jump = context.action.WasPressedThisFrame();
    }

    void UpdateAttack1(InputAction.CallbackContext context)
    {
        if(context.action.WasPressedThisFrame())
        {
            _animator.SetTrigger("Attack1");
            _moveSystem.canMove = false;
        }
    }
}
