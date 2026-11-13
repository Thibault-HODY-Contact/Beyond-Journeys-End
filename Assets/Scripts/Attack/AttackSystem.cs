using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class AttackSystem : MonoBehaviour
{
    [Serializable]
    class Attacks
    {
        [SerializeField] public List<HitBox> ListHitBox;
        [SerializeField] public float HitDuration = 0.2f;

    }
    [SerializeField] AttackID _attackID;

    [SerializeField] List<Attacks> ListCombo;
    [SerializeField] bool EnableByDefault = false;
    [SerializeField] int Damage = 10;

    [SerializeField] float Cooldown = 0.5f;
    [SerializeField] float ComboCooldown = 1f;

    public bool Attacking = false;

    public bool StopMovement = false;

    float _CooldownTimer = 0;
    float _ComboCooldownTimer = 0;

    float _HitDurationTimer = 0;

    Attacks currentAttack = null;

    int CurrentComboIndex = 0;

    void Start()
    {
        foreach (Attacks combo in ListCombo)
        {
            foreach (HitBox Hit in combo.ListHitBox)
            {
                Hit.gameObject.SetActive(EnableByDefault);
                Hit.EnemyHitted.Clear();
            }
        }

        _CooldownTimer = Cooldown;
    }

    public void Attack(int step)
    {
        if (Attacking) return;
        Attacking = true;
        StopMovement = true;
        currentAttack = ListCombo[step];
    }


    void Update()
    {
        if (!Attacking)
        {
            _ComboCooldownTimer += Time.deltaTime;
            if (_ComboCooldownTimer >= ComboCooldown)
                CurrentComboIndex = 0;
        }

        if (_CooldownTimer < Cooldown)
        {
            _CooldownTimer += Time.deltaTime;

        }
        else if (_CooldownTimer >= Cooldown && Attacking)
        {
            if (_HitDurationTimer < currentAttack.HitDuration)
            {
                foreach (HitBox Hit in currentAttack.ListHitBox)
                {
                    Hit.gameObject.SetActive(true);
                }
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
                        if (life != null) life.TakeDamage(Damage , _attackID);
                    }
                    Hit.EnemyHitted.Clear();
                    Hit.gameObject.SetActive(false);
                }
                _CooldownTimer = 0;
                _HitDurationTimer = 0;
                Attacking = false;

                CurrentComboIndex = (CurrentComboIndex + 1) % ListCombo.Count;
                _ComboCooldownTimer = 0;
                StopMovement = false;
            }
        }

    }
}