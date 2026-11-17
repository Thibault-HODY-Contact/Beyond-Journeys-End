using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class OnDamageFeedBack : MonoBehaviour
{
    [Serializable]
    class DamageFeedBack
    {
        public AttackID ID;
        public List<ParticleSystem> ParticleSystems;
        public List<VisualEffect> VisualEffects;
    }

    [Serializable]
    class ParryFeedBack
    {
        public AttackID ID;
        public List<ParticleSystem> ParticleSystems;
        public List<VisualEffect> VisualEffects;
    }


    [SerializeField] LifeSystem _lifeSystem;
    [SerializeField] List<DamageFeedBack> _damageFeedback;
    [SerializeField] List<ParryFeedBack> _parryFeedBack;
    [SerializeField] Animator _animator;
    AttackID _lastAttack = null;



    void Start()
    {
        _lifeSystem.OnDamage += SetDamageFeedback;
        _lifeSystem.OnParry += SetParryFeedback;

    }

    private void SetDamageFeedback(int lastHealth, int newHealth, AttackID iD)
    {
        _lastAttack = iD;
        _animator.SetInteger("LastAttackReceived", iD.ID);
        _animator.SetTrigger("Hurt");
    }
    private void SetParryFeedback( AttackID iD)
    {
        _lastAttack = iD;

    }
    void Update()
    {
        
    }

    virtual public void StartDamageFeedback(AttackID attackID)
    {
        if (!_lifeSystem.HaveParry)
        {
        

            var found = _damageFeedback.FirstOrDefault(i => i.ID == attackID);
            if (found == null)
            {
                return;
            }
            
            foreach (var ps in found.ParticleSystems)
            {
                ps.Play();
            }
            foreach (var vfx in found.VisualEffects)
            {
                vfx.Play();

            }

        }
        else
        {
            var found = _parryFeedBack.FirstOrDefault(i => i.ID == attackID);
            if (found == null)
            {
                return;
            }
            foreach (var ps in found.ParticleSystems)
            {
                ps.Play();
            }
            foreach (var vfx in found.VisualEffects)
            {
                vfx.Play();

            }
        }
    }
    virtual public void EndFeedback()
    {
        if (!_lifeSystem.HaveParry && _lastAttack)
        {
            var found = _damageFeedback.FirstOrDefault(i => i.ID == _lastAttack);
            if (found == null)
            {
                return;
            }

            foreach (var ps in found.ParticleSystems)
            {
                ps.Stop();
            }
            foreach (var vfx in found.VisualEffects)
            {
                vfx.Stop();

            }

        }
        else if (_lastAttack)
        {
            var found = _parryFeedBack.FirstOrDefault(i => i.ID == _lastAttack);
            if (found == null)
            {
                return;
            }
            foreach (var ps in found.ParticleSystems)
            {
                ps.Stop();
            }
            foreach (var vfx in found.VisualEffects)
            {
                vfx.Stop();

            }
        }
    }

}
