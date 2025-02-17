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
        var idle = new IdleState();
        States[STankState.Idle] = idle;
        idle.SmallTank = this;

        var attack = new AttackState();
        States[STankState.Attack] = attack;
        attack.SmallTank = this;

        var move = new MoveState();
        States[STankState.Move] = move;
        move.SmallTank = this;

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

    }

    void ControlTank()
    {
        base.Update();
    }
}
