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
        //overrides the base Update() method, allowing control over Update() execution
    }

    void ControlTank()
    {
        base.Update();
    }
}
