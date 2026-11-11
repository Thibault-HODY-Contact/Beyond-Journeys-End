using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] List<AttackSystem> AttackList;

    public void Launch(AttackID id)
    {

        AttackList[id.ID].Attack(id.ComboStep);
    }

    public void UnlockMovement(AttackID id)
    {

        AttackList[id.ID].StopMovement = false;
    }

}
