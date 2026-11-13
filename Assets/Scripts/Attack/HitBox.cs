using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public List<GameObject> EnemyHitted;
    public LayerMask LayerToHit;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerToHit.value) != 0)
        {
            if (!EnemyHitted.Contains(other.transform.root.gameObject))
            {

                EnemyHitted.Add(other.transform.root.gameObject);
            }

        }
    }
}
