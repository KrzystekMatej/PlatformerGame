using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
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
                item.Initialize();
                agent.WeaponManager.AddWeapon(item);
            }
        }
    }
}