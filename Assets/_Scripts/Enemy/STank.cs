using UnityEngine;
using UnityEngine.Pool;

public enum STankState
{
    Idle,
    Attack,
}

public class STank : StateManager<STankState>
{
    public Transform target;
    public Transform turret;
    public TankShell tankShell;
    public Transform muzzle;
    public ObjectPool<TankShell> Pool;
    public float shootPerBurst = 3f;
    public float delayPerBurst = 1.0f;
    public float delayPerShoot = 0.2f;

    void Awake()
    {
        var idle = new IdleState();
        States[STankState.Idle] = idle;
        idle.stank = this;

        var attack = new AttackState();
        States[STankState.Attack] = attack;
        attack.stank = this;

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
