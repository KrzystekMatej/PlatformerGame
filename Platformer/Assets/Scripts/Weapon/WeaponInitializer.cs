using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInitializer : MonoBehaviour
{
    [SerializeField]
    private List<AttackingWeapon> startWeapons = new List<AttackingWeapon>();

    void Start()
    {
        AgentManager agent = GetComponentInChildren<AgentManager>();
        if (agent == null) return;
        foreach (AttackingWeapon item in startWeapons)
        {
            agent.WeaponManager.AddWeaponWithSwap(item);
        }
    }

}