using System;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [SerializeField, Min(0)] int _maxHealth = 100;
    [SerializeField] int _health = 100;

    [SerializeField] ShieldSystem _shieldSystem;

    public int Health
    {
        get => _health;
        private set
        {
            if (value > _maxHealth)
            {
                value = _maxHealth;
            }
            if (value < 0)
            {
                value = 0;
            }
            _health = value;

            if (_health == 0)
            {
                OnDeath?.Invoke();
            }
        }
    }



    public int MaxHealth => _maxHealth;
    public bool IsDead => _health == 0;
    public bool HaveParry;

    public Action<int, int> OnHeal;
    public Action<int, int , AttackID> OnDamage;
    public Action< AttackID> OnParry;
    public Action OnDeath;

    private void Reset()
    {
        _maxHealth = 100;
        _health = _maxHealth;
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    public void Heal(int amount)
    {
        if (IsDead) return;

        int newHealth = Mathf.Min(Health + amount, _maxHealth);

        OnHeal?.Invoke(Health, newHealth);

        Health = newHealth;
    }

    public void TakeDamage(int amount, AttackID _attackID)
    {
        if (_shieldSystem != null && _shieldSystem.IsShieldActive)
            return;

        if (IsDead) return;
        if (HaveParry)
        Debug.Log($"{gameObject.name} took {amount} damage.");

        int newHealth = Mathf.Max(Health - amount, 0);

        OnDamage?.Invoke(Health, newHealth, _attackID);

        //Health = newHealth;

        Debug.Log($"{gameObject.name} health is now {Health}.");
    }
    public void Revive()
    {
        OnHeal?.Invoke(_health, _maxHealth);
        Health = _maxHealth;
    }
}
