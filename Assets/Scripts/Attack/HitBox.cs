using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public List<GameObject> EnemyHitted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyHitted.Add(other.transform.root.gameObject);

            //other.gameObject.GetComponentInChildren<LifeSystem>().TakeDamage(Damage);
        }
    }
}
