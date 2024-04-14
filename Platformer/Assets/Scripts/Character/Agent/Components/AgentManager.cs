using UnityEngine;
using UnityEngine.Events;

public class AgentManager : MonoBehaviour, IHittable
{
    public UnityEvent OnFallOut, OnRespawnRequired;
    public UnityEvent<Collider2D, Weapon> OnHit;
    [field: SerializeField]
    public AgentDefaultData DefaultData { get; private set; }
    public AgentInstanceData InstanceData { get; set; }
    public Rigidbody2D RigidBody { get; private set; }
    public InputController InputController { get; private set; }
    public AnimatorManager Animator { get; private set; }
    public AudioFeedback AudioFeedback { get; private set; }
    public WeaponManager WeaponManager { get; private set; }
    public OrientationController OrientationController { get; private set; }
    public GroundDetector GroundDetector { get; private set; }
    public TriggerFilter ClimbDetector { get; private set; }
    public HealthManager HealthManager { get; private set; }
    public PointManager PointManager { get; private set; }
    public Invulnerability Invulnerability { get; private set; }
    public FiniteStateMachine StateMachine { get; private set; }
    [field: SerializeField]
    public Collider2D PhysicsCollider { get; private set; }
    [field: SerializeField]
    public Collider2D TriggerCollider { get; private set; }


    public Vector2 CenterPosition { get => TriggerCollider.bounds.center; }
    public float EnclosingCircleRadius { get => MathUtility.GetEnclosingCircleRadius(TriggerCollider); }
    public LayerMask CollisionMask { get; private set; }


    private void Awake()
    {
        InputController = GetComponentInParent<InputController>();
        RigidBody = GetComponent<Rigidbody2D>();
        RigidBody.gravityScale = DefaultData.GravityScale;
        Animator = GetComponentInChildren<AnimatorManager>();
        OrientationController = GetComponentInChildren<OrientationController>();
        GroundDetector = GetComponentInChildren<GroundDetector>();
        ClimbDetector = GetComponent<TriggerFilter>();
        AudioFeedback = GetComponentInChildren<AudioFeedback>();
        WeaponManager = GetComponentInChildren<WeaponManager>();
        HealthManager = GetComponent<HealthManager>();
        PointManager = GetComponent<PointManager>();
        Invulnerability = GetComponent<Invulnerability>();
        StateMachine = GetComponentInChildren<FiniteStateMachine>();
        TriggerCollider = TriggerCollider == null ? GetComponent<Collider2D>() : TriggerCollider;
        PhysicsCollider = PhysicsCollider == null ? GetComponentInChildren<Collider2D>() : PhysicsCollider;
        CollisionMask = Utility.GetCollisionLayerMask(PhysicsCollider.gameObject.layer);

        InstanceData = new AgentInstanceData()
        {
            MaxSpeed = DefaultData.MaxSpeed,
            MaxForce = DefaultData.MaxForce,
            JumpForce = DefaultData.JumpForce,
            ClimbSpeed = DefaultData.ClimbSpeed
        };
    }

    private void Start()
    {
        HealthManager.Initialize(DefaultData.Health);
        Animator.OnAnimationAction.AddListener(() => StateMachine.InterruptFilter |= InterruptMask.AnimationAction);
        Animator.OnAnimationComplete.AddListener(() => StateMachine.InterruptFilter |= InterruptMask.AnimationComplete);
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
        if (!HealthManager.IsAlive()) return;
        if (Invulnerability != null)
        {
            if (Invulnerability.IsActive) return;
            else StartCoroutine(Invulnerability.Run(StateMachine.Factory));
        }
        HealthManager.AddHealth(-attackDamage);
        StateMachine.InterruptFilter |= InterruptMask.Hurt;
    }

    public void FallOut()
    {
        Invulnerability temp = Invulnerability;
        Invulnerability = null;
        OnFallOut?.Invoke();
        Invulnerability = temp;
        if (HealthManager.IsAlive()) OnRespawnRequired?.Invoke();
    }

    private void PerformKnockback(Vector2 from, float knockbackForce)
    {
        RigidBody.velocity = Vector2.zero;
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