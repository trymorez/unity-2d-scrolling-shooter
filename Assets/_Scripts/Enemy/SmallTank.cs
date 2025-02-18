using UnityEngine;
using UnityEngine.Pool;

public enum STankState
{
    Idle,
    Attack,
    Move,
}

public class SmallTank : StateManager<STankState>
{
    public Transform Target;
    public Transform Turret;
    public TankShell tankShell;
    public Transform Muzzle;
    public ObjectPool<TankShell> Pool;
    public float MoveSpeed = 2f;
    public float ShootPerBurst = 3f;
    public float DelayPerBurst = 2.0f;
    public float DelayPerShoot = 0.2f;
    public bool IsOutOfScreen;

    void Awake()
    {
        States[STankState.Idle] = new IdleState(this);

        States[STankState.Attack] = new AttackState(this);

        States[STankState.Move] = new MoveState(this);

        GameManager.OnPlayingGame += ControlTank;
    }

    void OnDestroy()
    {
        GameManager.OnPlayingGame -= ControlTank;
    }

    protected override void Start()
    {
        ChangeState(STankState.Idle);
        base.Start();
    }

    protected override void Update()
    {
        //overrides base Update() method to control update routine
    }

    void ControlTank()
    {
        base.Update();
        if (IsOutOfScreen)
        {
            Destroy(gameObject);
        }
    }
    
    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Activator"))
        {
            IsOutOfScreen = true;
        }
    }
}
