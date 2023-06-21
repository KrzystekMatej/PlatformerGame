using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class WeaponInitializer : MonoBehaviour
    {
        [SerializeField]
        private List<Weapon> startWeapons = new List<Weapon>();

        void Start()
        {
            Agent agent = GetComponentInChildren<Agent>();
            if (agent == null) return;
            foreach (Weapon item in startWeapons)
            {
                item.Initialize();
                agent.WeaponManager.AddWeapon(item);
            }
        }
    }
}