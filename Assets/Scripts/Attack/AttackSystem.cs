
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [Serializable]
    class Attacks
    {
        [SerializeField] public List<HitBox> ListHitBox;
        [SerializeField] public List<GameObject> ObjectsToActivate;
        [SerializeField] public float HitDuration = 0.2f;
    }

    [SerializeField] AttackID _attackID;

    [SerializeField] List<Attacks> ListCombo;
    [SerializeField] bool EnableByDefault = false;
    [SerializeField] int Damage = 10;

    [SerializeField] float Cooldown = 0.5f;
    [SerializeField] float ComboCooldown = 1f;

    [SerializeField] float _spawnDuration = 0.5f;

    public bool Attacking = false;
    public bool StopMovement = false;
    public bool CanAttack = false;

    float _CooldownTimer = 0;
    float _ComboCooldownTimer = 0;
    float _HitDurationTimer = 0;

    Attacks currentAttack = null;

    int CurrentComboIndex = 0;

    Coroutine _spawnCoroutine;

    Dictionary<GameObject, Vector3> _originalScales = new();

    void Awake()
    {
        foreach (Attacks combo in ListCombo)
        {
            foreach (HitBox Hit in combo.ListHitBox)
            {
                Hit.gameObject.SetActive(EnableByDefault);
                Hit.EnemyHitted.Clear();
            }

            foreach (GameObject obj in combo.ObjectsToActivate)
            {
                if (obj == null) continue;

                if (!_originalScales.ContainsKey(obj))
                    _originalScales.Add(obj, obj.transform.localScale);

                obj.SetActive(false);
            }
        }
    }

    void Start()
    {
        CanAttack = true;
        _CooldownTimer = Cooldown;
    }

    public void Attack(int step)
    {
        if (Attacking) return;

        Attacking = true;
        StopMovement = true;
        currentAttack = ListCombo[step];
    }

    public void ActivateObjects()
    {
        if (currentAttack == null) return;

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);


        foreach (GameObject obj in currentAttack.ObjectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        _spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        foreach (GameObject obj in currentAttack.ObjectsToActivate)
        {
            if (obj != null)
                obj.transform.localScale = Vector3.zero;
        }

        float timer = 0f;

        while (timer < _spawnDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / _spawnDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            foreach (GameObject obj in currentAttack.ObjectsToActivate)
            {
                if (obj != null && _originalScales.TryGetValue(obj, out Vector3 originalScale))
                {
                    obj.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, smoothT);
                }
            }

            yield return null;
        }

        foreach (GameObject obj in currentAttack.ObjectsToActivate)
        {
            if (obj != null && _originalScales.TryGetValue(obj, out Vector3 originalScale))
                obj.transform.localScale = originalScale;
        }

        _spawnCoroutine = null;
    }

    public void DeactivateObjects()
    {
        if (currentAttack == null) return;

        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        foreach (GameObject obj in currentAttack.ObjectsToActivate)
        {
            if (obj == null) continue;

            obj.transform.localScale = _originalScales[obj];
            obj.SetActive(false);
        }
    }

    void Update()
    {
        if (!Attacking)
        {
            _ComboCooldownTimer += Time.deltaTime;

            if (_ComboCooldownTimer >= ComboCooldown)
                CurrentComboIndex = 0;

            if (_CooldownTimer > Cooldown)
                CanAttack = true;
        }

        if (_CooldownTimer < Cooldown)
        {
            _CooldownTimer += Time.deltaTime;
        }
        else if (_CooldownTimer >= Cooldown && Attacking)
        {
            CanAttack = false;

            if (_HitDurationTimer < currentAttack.HitDuration)
            {
                foreach (HitBox Hit in currentAttack.ListHitBox)
                    Hit.gameObject.SetActive(true);

                _HitDurationTimer += Time.deltaTime;
            }
            else
            {
                foreach (HitBox Hit in currentAttack.ListHitBox)
                {
                    foreach (GameObject hitted in Hit.EnemyHitted)
                    {
                        if (hitted == null) continue;

                        LifeSystem life = hitted.GetComponentInChildren<LifeSystem>();

                        if (life != null)
                            life.TakeDamage(Damage, _attackID);
                    }

                    Hit.EnemyHitted.Clear();
                    Hit.gameObject.SetActive(false);
                }

                _CooldownTimer = 0;
                _HitDurationTimer = 0;
                Attacking = false;

                CurrentComboIndex = (CurrentComboIndex + 1) % ListCombo.Count;
                _ComboCooldownTimer = 0;
            }
        }
    }
}
