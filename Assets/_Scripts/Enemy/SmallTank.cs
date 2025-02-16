using UnityEngine;
using UnityEngine.Pool;

public enum STankState
{
    Idle,
    Attack,
}

public class SmallTank : StateManager<STankState>
{
    public Transform Target;
    public Transform Turret;
    public TankShell tankShell;
    public Transform Muzzle;
    public ObjectPool<TankShell> Pool;
    public float shootPerBurst = 3f;
    public float delayPerBurst = 1.0f;
    public float delayPerShoot = 0.2f;

    void Awake()
    {
        var idle = new IdleState();
        States[STankState.Idle] = idle;
        idle.SmallTank = this;

        var attack = new AttackState();
        States[STankState.Attack] = attack;
        attack.SmallTank = this;

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
