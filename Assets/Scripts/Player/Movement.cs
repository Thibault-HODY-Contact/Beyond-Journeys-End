using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform _rootTransform;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _RunSpeed = 5f;

    [SerializeField] Animator _animator;
    [SerializeField] Camera _camera;

    public bool canMove = true;
    public float Speed { get => _speed; set => _speed = value; }

    public float RunSpeed { get => _RunSpeed; set => _RunSpeed = value; }

    public Vector2 MoveDirection { get; set; }

    public bool isRunning { get; set; }

    Rigidbody _rb;
    const float Skin = 0.02f;

    private void Reset()
    {
        _rootTransform = transform.root;
    }

    private void Awake()
    {
        _rb = _rootTransform.GetComponentInChildren<Rigidbody>();

    }

    void MoveRoot(Vector3 localMove)
    {
        Vector3 move = _rootTransform.TransformDirection(localMove);

        if (_rb != null)
        {
            for (int i = 0; i < 2 && move.sqrMagnitude > 1e-8f; i++)  
            {
                Physics.SyncTransforms();                               
                float dist = move.magnitude;
                Vector3 dir = move / dist;

                if (!_rb.SweepTest(dir, out RaycastHit hit, dist + Skin, QueryTriggerInteraction.Ignore))
                    break;                                               
                if (hit.normal.y > 0.5f)
                    break;                                               

                float allowed = Mathf.Max(hit.distance - Skin, 0f);
                _rootTransform.position += dir * allowed;                

                Vector3 n = new Vector3(hit.normal.x, 0f, hit.normal.z).normalized;
                move -= dir * allowed;
                move -= n * Vector3.Dot(move, n);                     
            }
        }

        _rootTransform.position += move;
    }

    void Update()
    {
        if (!canMove)
            return;

        if(MoveDirection.x != 0|| MoveDirection.y != 0)
        {
            if(isRunning)
            {

                _animator.SetFloat("Speed", 1);
            }
            else
            {
                _animator.SetFloat("Speed", 0.5f);

            }

        }
        else
        {
            _animator.SetFloat("Speed", 0);

        }


        if(MoveDirection.y != 0 || MoveDirection.x != 0 )
        {
            _rootTransform.transform.forward = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);

        }

        if (!isRunning)
        {
            MoveRoot(new Vector3(MoveDirection.x, 0f, MoveDirection.y) * _speed * Time.unscaledDeltaTime);
        }
        else
        {
            MoveRoot(new Vector3(0f, 0f, MoveDirection.y) * _RunSpeed * Time.unscaledDeltaTime);
        }


        _animator.SetFloat("Right", MoveDirection.x);
        _animator.SetFloat("Forward", MoveDirection.y);

    }


}
