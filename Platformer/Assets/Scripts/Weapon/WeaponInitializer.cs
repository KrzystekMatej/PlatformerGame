using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInitializer : MonoBehaviour
{
    [SerializeField]
    private List<AgentWeapon> startWeapons = new List<AgentWeapon>();

    void Start()
    {
        Agent agent = GetComponentInChildren<Agent>();
        if (agent == null) return;
        foreach (AgentWeapon item in startWeapons)
        {
            agent.WeaponManager.AddWeaponWithSwap(item);
        }
    }

}