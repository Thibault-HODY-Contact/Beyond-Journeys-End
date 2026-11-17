using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] List<AttackSystem> AttackList;

    [SerializeField] List<GameObject> _objectsToActivateList;

    public void Launch(AttackID id)
    {

        AttackList[id.ID].Attack(id.ComboStep);
    }

    public void UnlockMovement(AttackID id)
    {

        AttackList[id.ID].StopMovement = false;
    }

    public void ActivateObjects()
    {
        foreach (GameObject obj in _objectsToActivateList)
        {
            obj.SetActive(true);
        }
    }
    public void DeactivateObjects()
    {
        foreach (GameObject obj in _objectsToActivateList)
        {
            obj.SetActive(false);
        }
    }
}
