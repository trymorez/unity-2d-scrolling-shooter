using UnityEngine;

public enum STankState
{
    Idle,
    Attack,
}

public class STank : StateManager<STankState>
{
    [SerializeField] Transform turret;
    [SerializeField] TankShell tankShell;
    [SerializeField] Transform muzzle;

    void Awake()
    {
        States[STankState.Idle] = new IdleState();
        States[STankState.Attack] = new AttackState();
        AttackState.turret = turret;
        AttackState.tankShell = tankShell;
        AttackState.muzzle = muzzle;
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
