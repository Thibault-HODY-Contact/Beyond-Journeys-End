
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] List<AttackSystem> AttackList;

    AttackSystem _currentAttack;

    public void Launch(AttackID id)
    {
        _currentAttack = AttackList[id.ID];
        _currentAttack.Attack(id.ComboStep);
    }

    public void UnlockMovement(AttackID id)
    {
        AttackList[id.ID].StopMovement = false;
    }

    public void ActivateObjects()
    {
        if (_currentAttack != null)
            _currentAttack.ActivateObjects();
    }

    public void DeactivateObjects()
    {
        if (_currentAttack != null)
            _currentAttack.DeactivateObjects();
    }
}