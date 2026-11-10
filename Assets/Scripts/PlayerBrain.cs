using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] InputActionReference _movementInput;
    [SerializeField] InputActionReference _runInput;

    [SerializeField] Movement _moveSystem;


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

    }
    private void OnDestroy()
    {
        if (_movementInput == null) return;

        _movementInput.action.performed -= UpdateMove;
        _movementInput.action.canceled -= UpdateMove;

        if (_runInput == null) return;
        _runInput.action.performed -= UpdateRun;
        _runInput.action.canceled -= UpdateRun;
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

}
