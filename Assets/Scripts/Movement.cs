using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform _rootTransform;
    [SerializeField] float _speed = 5f;
    [SerializeField] Animator _animator;
    [SerializeField] Camera _camera;
    public float Speed { get => _speed; set => _speed = value; }
    public Vector2 MoveDirection { get; set; }
    private void Reset()
    {
        _rootTransform = transform.root;
    }

    void Update()
    {
        if(MoveDirection.x != 0|| MoveDirection.y != 0)
        {
            _animator.SetFloat("Speed", 1);

        }
        else
        {
            _animator.SetFloat("Speed", 0);

        }


        if(MoveDirection.y != 0 && MoveDirection.x == 0)
        {
            _rootTransform.transform.forward = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);

        }

        _rootTransform.Translate(new Vector3(MoveDirection.x, 0f, MoveDirection.y) * _speed * Time.deltaTime, Space.Self);

        _animator.SetFloat("Right", MoveDirection.x);
        _animator.SetFloat("Forward", MoveDirection.y);

    }


}
