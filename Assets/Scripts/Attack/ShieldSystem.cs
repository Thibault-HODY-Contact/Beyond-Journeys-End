
using System.Collections;
using UnityEngine;

public class ShieldSystem : MonoBehaviour
{
    [SerializeField] GameObject _shieldObject;
    [SerializeField] float _shieldDuration = 2f;
    [SerializeField] float _cooldown = 5f;
    [SerializeField] float _spawnDuration = 0.5f;


    public AudioClip _AudioClip;
    [SerializeField] AudioSource _audioSource;

    public bool IsShieldActive = false;
    public bool CanShield = true;
    public bool StopMovement = false;

    float _shieldTimer = 0f;
    float _cooldownTimer = 0f;

    Vector3 _originalScale;
    Coroutine _spawnCoroutine;

    void Awake()
    {
        if (_shieldObject != null)
            _originalScale = _shieldObject.transform.localScale;
    }

    void Start()
    {
        if (_shieldObject != null)
            _shieldObject.SetActive(false);
    }

    void Update()
    {
        if (IsShieldActive)
        {
            _shieldTimer += Time.deltaTime;

            if (_shieldTimer >= _shieldDuration)
                DisableShield();
        }

        if (!CanShield)
        {
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer >= _cooldown)
            {
                CanShield = true;
                _cooldownTimer = 0f;
            }
        }
    }

    public void ActivateShield()
    {
        if (!CanShield || IsShieldActive)
            return;

        IsShieldActive = true;
        CanShield = false;
        StopMovement = true;

        _shieldTimer = 0f;

        if (_shieldObject != null)
        {
            _shieldObject.SetActive(true);
            _shieldObject.transform.localScale = Vector3.zero;

            _spawnCoroutine = StartCoroutine(SpawnShield());
        }
        if (_AudioClip != null)
        {
            _audioSource.clip = _AudioClip;
            _audioSource.Play();
        }
    }

    IEnumerator SpawnShield()
    {
        float timer = 0f;

        while (timer < _spawnDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / _spawnDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            _shieldObject.transform.localScale = Vector3.Lerp(Vector3.zero, _originalScale, smoothT);

            yield return null;
        }

        _shieldObject.transform.localScale = _originalScale;
        _spawnCoroutine = null;
    }

    void DisableShield()
    {
        IsShieldActive = false;
        StopMovement = false;
        _shieldTimer = 0f;

        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        if (_shieldObject != null)
        {
            _shieldObject.transform.localScale = _originalScale;
            _shieldObject.SetActive(false);
        }
    }
}
