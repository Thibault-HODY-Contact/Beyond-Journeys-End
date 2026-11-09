using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] InputActionReference _movementInput;
    [SerializeField] InputActionReference _lookInput;

    [SerializeField] Movement _moveSystem;


    void Start()
    {
        if (_movementInput != null)
        {

            _movementInput.action.performed += UpdateMove;
            _movementInput.action.canceled += UpdateMove;

        }

       

    }
    private void OnDestroy()
    {
        if (_movementInput == null) return;

        _movementInput.action.performed -= UpdateMove;
        _movementInput.action.canceled -= UpdateMove;
        

    }

    void Update()
    {
        
    }


    void UpdateMove(InputAction.CallbackContext context)
    {
        _moveSystem.MoveDirection = context.ReadValue<Vector2>();
    }

   
}
