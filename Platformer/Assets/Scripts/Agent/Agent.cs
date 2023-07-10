using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Agent : MonoBehaviour, IHittable
{
    public UnityEvent OnDeathComplete, OnFallOut, OnRespawnRequired;
    public UnityEvent<GameObject, Weapon> OnHit;

    public AgentData DefaultData;
    public AgentInstanceData InstanceData;
    public Rigidbody2D RigidBody { get; private set; }
    public InputController InputController { get; private set; }
    public AgentAnimator Animator { get; private set; }
    public AudioFeedback AudioFeedback { get; private set; }
    public WeaponManager WeaponManager { get; private set; }
    public OrientationController OrientationController { get; private set; }
    public CollisionDetector GroundDetector { get; private set; }
    public TriggerDetector ClimbDetector { get; private set; }
    public HealthManager HealthManager { get; private set; }
    public PointManager PointManager { get; private set; }
    public Invulnerability Invulnerability { get; private set; }
    public FiniteStateMachine StateMachine { get; private set; }
    [field: SerializeField]
    public Collider2D PhysicsCollider { get; private set; }
    [field: SerializeField]
    public Collider2D TriggerCollider { get; private set; }


    private void Awake()
    {
        InputController = GetComponentInParent<InputController>();
        RigidBody = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<AgentAnimator>();
        OrientationController = GetComponentInChildren<OrientationController>();
        GroundDetector = GetComponentInChildren<CollisionDetector>();
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
            Acceleration = DefaultData.Acceleration,
            Deacceleration = DefaultData.Deacceleration,
            JumpForce = DefaultData.JumpForce,
            JumpGravityModifier = DefaultData.JumpGravityModifier,
            FallGravityModifier = DefaultData.FallGravityModifier,
            ClimbSpeed = DefaultData.ClimbSpeed,
            DefaultGravityScale = RigidBody.gravityScale
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
        OrientationController.SetAgentOrientation(InputController.InputData.MovementVector);
        StateMachine.UpdateState(this);
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
    
    public void Hit(GameObject gameObject, Weapon attackingWeapon)
    {
        Hit(attackingWeapon.AttackDamage);
        OnHit?.Invoke(gameObject, attackingWeapon);
        PerformKnockback(gameObject.transform.position, attackingWeapon.KnockbackForce);
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

    public void PerformKnockback(Vector3 from, float knockbackForce)
    {
        if (knockbackForce <= 0) return;
        Vector2 direction = transform.position - from;
        RigidBody.AddForce(new Vector2(direction.normalized.x, 0) * knockbackForce, ForceMode2D.Impulse);
    }
}
