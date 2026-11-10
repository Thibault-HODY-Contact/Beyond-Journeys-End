using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] InputActionReference _movementInput;
    [SerializeField] InputActionReference _runInput;
    [SerializeField] InputActionReference _JumpInput;

    [SerializeField] Movement _moveSystem;
    [SerializeField] JumpSystem _jumpSystem;


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
    }

    void Update()
    {
        
    }


    void UpdateMove(InputAction.CallbackContext context)
    {
        _moveSystem.MoveDirection = context.ReadValue<Vector2>();
    }

    void UpdateRun(InputAction.CallbackContext context)
    {
        _moveSystem.isRunning = context.action.WasPressedThisFrame();
    }

    void UpdateJump(InputAction.CallbackContext context)
    {
        _jumpSystem.Jump = context.action.WasPressedThisFrame();
    }
}
