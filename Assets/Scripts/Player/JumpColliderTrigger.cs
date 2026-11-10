using UnityEngine;

public class JumpColliderTrigger : MonoBehaviour
{
    [SerializeField] JumpSystem _jumpSystem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _jumpSystem.isJumping = false;
            _jumpSystem.isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _jumpSystem.isJumping = true;
            _jumpSystem.isGrounded = false;

            _jumpSystem.canJump = false;

        }
    }
}
