using UnityEngine;

public class AttackState : State
{
    [SerializeField]
    public LayerMask HitMask;

    protected override void HandleEnter()
    {
        agent.WeaponManager.SetWeaponVisibility(true);
        if (agent.GroundDetector && agent.GroundDetector.Detected) agent.RigidBody.velocity = Vector3.zero;

        agent.AudioFeedback.PlaySpecificSound(agent.WeaponManager.GetWeapon().WeaponSound);
        agent.WeaponManager.GetWeapon().Attack(agent.TriggerCollider, agent.OrientationController.CurrentOrientation, HitMask);
    }

    protected override void HandleUpdate() { }

    protected override void HandleExit()
    {
        agent.WeaponManager.SetWeaponVisibility(false);
    }
}
