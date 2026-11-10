using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [SerializeField] List<HitBox> ListHitBox;
    [SerializeField] bool EnableByDefault = false;
    [SerializeField] int Damage = 10;

    [SerializeField] float Cooldown = 0.5f;
    [SerializeField] float HitDuration = 0.2f;

    public bool Attacking = false;

    public bool StopMovement = false;

    float _CooldownTimer = 0;
    float _HitDurationTimer = 0;


    void Start()
    {
        foreach(HitBox Hit in ListHitBox )
        {
            Hit.gameObject.SetActive(EnableByDefault);
            Hit.EnemyHitted.Clear();
        }



    }

    public void Attack()
    {
        Attacking = true;
        StopMovement = true;
    }


    void Update()
    {
        
        if( _CooldownTimer < Cooldown)
        {
            _CooldownTimer += Time.deltaTime;
            Attacking = false;
        }
        else if (_CooldownTimer >= Cooldown && Attacking)
        {
            if(_HitDurationTimer < HitDuration)
            {
                foreach (HitBox Hit in ListHitBox)
                {
                    Hit.gameObject.SetActive(true);
                }
                _HitDurationTimer += Time.deltaTime;
            }
            else
            {
                foreach (HitBox Hit in ListHitBox)
                {
                    foreach(GameObject hitted in Hit.EnemyHitted)
                    {
                        hitted.GetComponentInChildren<LifeSystem>().TakeDamage(Damage);
                    }
                    Hit.EnemyHitted.Clear();
                    Hit.gameObject.SetActive(false);

                }
                _CooldownTimer = 0;
                _HitDurationTimer = 0;
                Attacking = false;
            }



        }




    }
}
