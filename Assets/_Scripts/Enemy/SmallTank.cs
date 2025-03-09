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
    public Transform TurretTransform;
    public TankTurret Turret;
    public TankShell tankShell;
    public Transform Muzzle;
    public MuzzleFlash muzzleFlash;
    public ObjectPool<TankShell> Pool;
    public Waypoints Waypoints;
    public float MoveSpeed = 2f;
    public float ShootPerBurst = 3f;
    public float DelayPerBurst = 2.0f;
    public float DelayPerShoot = 0.2f;
    public bool IsOutOfScreen;
    public bool IsTurning;
    float tankTurnSpeed = 5f;
    public Vector3 previousPos;

    void Awake()
    {
        States[STankState.Idle] = new IdleState(this);
        States[STankState.Attack] = new AttackState(this);
        States[STankState.Move] = new MoveState(this);

        ChangeState(STankState.Idle);
        GameManager.OnPlaying += ControlTank;
    }

    protected override void Start()
    {
        base.Start();
        previousPos = transform.localPosition;
    }

    void OnDestroy()
    {
        States[STankState.Idle] = null;
        States[STankState.Attack] = null;
        States[STankState.Move] = null;

        GameManager.OnPlaying -= ControlTank;
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

    public void TurnToNextWaypont()
    {
        var minAngleDiff = 1f;
        Vector2 VectorToNextWaypoint = Waypoints.GetWaypoint();
        float wantedTankAngleZ = Mathf.Atan2(VectorToNextWaypoint.y, VectorToNextWaypoint.x) * Mathf.Rad2Deg + 270f;
        float currentAngleZ = transform.rotation.eulerAngles.z;
        float smoothAngleZ = Mathf.LerpAngle(currentAngleZ, wantedTankAngleZ, Time.deltaTime * tankTurnSpeed);

        var turretRotation = TurretTransform.rotation;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothAngleZ));
        TurretTransform.rotation = turretRotation;

        float angleDiff = Mathf.DeltaAngle(currentAngleZ, wantedTankAngleZ);
        if (Mathf.Abs(angleDiff) < minAngleDiff)
        {
            //align tank to next waypoint
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, wantedTankAngleZ));
            IsTurning = false;
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
