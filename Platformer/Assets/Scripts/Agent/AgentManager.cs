using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class AgentManager : MonoBehaviour, IHittable
{
    public UnityEvent OnDeathComplete, OnFallOut, OnRespawnRequired;
    public UnityEvent<Collider2D, Weapon> OnHit;
    [field: SerializeField]
    public AgentData DefaultData { get; private set; }
    public AgentInstanceData InstanceData;
    public Rigidbody2D RigidBody { get; private set; }
    public InputController InputController { get; private set; }
    public AgentAnimator Animator { get; private set; }
    public AudioFeedback AudioFeedback { get; private set; }
    public WeaponManager WeaponManager { get; private set; }
    public OrientationController OrientationController { get; private set; }
    public GroundDetector GroundDetector { get; private set; }
    public TriggerDetector ClimbDetector { get; private set; }
    public HealthManager HealthManager { get; private set; }
    public PointManager PointManager { get; private set; }
    public Invulnerability Invulnerability { get; private set; }
    public FiniteStateMachine StateMachine { get; private set; }
    [field: SerializeField]
    public Collider2D PhysicsCollider { get; private set; }
    [field: SerializeField]
    public Collider2D TriggerCollider { get; private set; }

    public float EnclosingCircleRadius { get => MathUtility.GetEnclosingCircleRadius(TriggerCollider); }
    public Vector2 CenterPosition { get => TriggerCollider.bounds.center; }

    private void Awake()
    {
        InputController = GetComponentInParent<InputController>();
        RigidBody = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<AgentAnimator>();
        OrientationController = GetComponentInChildren<OrientationController>();
        GroundDetector = GetComponentInChildren<GroundDetector>();
        ClimbDetector = GetComponent<TriggerDetector>();
        AudioFeedback = GetComponentInChildren<AudioFeedback>();
        WeaponManager = GetComponentInChildren<WeaponManager>();
        HealthManager = GetComponent<HealthManager>();
        PointManager = GetComponent<PointManager>();
        Invulnerability = GetComponent<Invulnerability>();
        StateMachine = GetComponentInChildren<FiniteStateMachine>();
        TriggerCollider = TriggerCollider == null ? GetComponent<Collider2D>() : TriggerCollider;
        PhysicsCollider = PhysicsCollider == null ? GetComponentInChildren<Collider2D>() : PhysicsCollider;

        InstanceData = new AgentInstanceData()
        {
            Health = DefaultData.Health,
            MaxSpeed = DefaultData.MaxSpeed,
            MaxForce = DefaultData.MaxForce,
            JumpForce = DefaultData.JumpForce,
            JumpGravityModifier = DefaultData.JumpGravityModifier,
            FallGravityModifier = DefaultData.FallGravityModifier,
            ClimbSpeed = DefaultData.ClimbSpeed
        };
    }

    private void Start()
    {
        HealthManager.Initialize(InstanceData.Health);
        Animator.OnAnimationComplete.AddListener(() => StateMachine.PerformInterruptTransition(this, InterruptType.AnimationComplete));
    }

    private void Update()
    {
        if (InputController.InputData.WeaponSwap == InputState.Pressed)
        {
            WeaponManager.SwapWeapon();
        }
        OrientationController.SetAgentOrientation(RigidBody.velocity);
        StateMachine.PerformStateUpdate(this);
    }

    private void FixedUpdate()
    {
        GroundDetector.Detect();
    }

    public void FallOut()
    {
        OnFallOut?.Invoke();
        OnRespawnRequired?.Invoke();
    }
    
    public void Hit(Collider2D attacker, Weapon damageWeapon)
    {
        if (attacker == TriggerCollider) return;
        Hit(damageWeapon.AttackDamage);
        OnHit?.Invoke(attacker, damageWeapon);
        PerformKnockback(attacker.bounds.center, damageWeapon.KnockbackForce);
    }

    public void Hit(int attackDamage)
    {
        if (!HealthManager.IsAlive() || (Invulnerability != null && Invulnerability.IsActive)) return;

        HealthManager.ChangeHealth(-attackDamage);
        StateMachine.PerformInterruptTransition(this, InterruptType.Hit);
    }

    public void Kill()
    {
        Hit(HealthManager.CurrentHealth);
    }

    public void PerformKnockback(Vector2 from, float knockbackForce)
    {
        if (knockbackForce <= 0) return;
        Vector2 direction = CenterPosition - from;
        RigidBody.AddForce(new Vector2(direction.normalized.x, 0) * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D collider = GetComponent<Collider2D>();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(CenterPosition, MathUtility.GetEnclosingCircleRadius(collider));
    }
}