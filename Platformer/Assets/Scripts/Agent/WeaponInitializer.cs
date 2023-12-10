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
            Vision vision = GetComponentInChildren<Vision>();
            if (agent == null) return;
            foreach (AgentWeapon item in startWeapons)
            {
                item.Initialize();
                agent.WeaponManager.AddWeapon(item);
                if (vision != null) AddWeaponDetector(agent, item, vision);
            }
        }
        
        public void AddWeaponDetector(Agent agent, Weapon weapon, Vision vision)
        {
            MeleeWeapon melee = weapon as MeleeWeapon;
            AttackState attackState = agent.GetComponentInChildren<AttackState>();
            if (melee != null)
            {
                vision.AddCastDetector
                (
                    "Right" + weapon.WeaponName,
                    Color.red,
                    new Vector2(melee.AttackRange, melee.AttackWidth),
                    0,
                    Vector2.right,
                    new Vector2(melee.AttackRange / 2, 0),
                    attackState.hitMask,
                    CastType.Box
                );
                vision.AddCastDetector
                (
                    "Left" + weapon.WeaponName,
                    Color.red,
                    new Vector2(melee.AttackRange, melee.AttackWidth),
                    0,
                    Vector2.left,
                    new Vector2(-melee.AttackRange / 2, 0),
                    attackState.hitMask,
                    CastType.Box
                );
            }
        }

    }
}