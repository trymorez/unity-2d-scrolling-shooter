using UnityEngine;
using System;

public class AttackState : BaseState<STankState>
{
    public SmallTank SmallTank;

    int currentShoot;
    float nextShootTime;
    bool isBurstOngoing;
    bool isTartgetAcquired;

    bool isTimeForNextShoot { get => Time.time > nextShootTime; }

    public AttackState() : base(STankState.Attack) { }

    public override void EnterState()
    {
        isTartgetAcquired = true;
        isBurstOngoing = false;
        currentShoot = 0;
        CalculateNextShootTime(SmallTank.delayPerShoot);
    }

    public override void ExitState()
    {
    }

    public override STankState GetNextState()
    {
        if (!isTartgetAcquired && !isBurstOngoing)
        {
            return STankState.Idle;
        }
        else
        {
            return STankState.Attack;
        }
    }

    public override void UpdateState()
    {
        CheckShouldStartBurst();
        RotateTurretToPlayer();

        if (isBurstOngoing)
        {
            CheckIfBurstCompleted();

            if (isTimeForNextShoot)
            {
                CalculateNextShootTime(SmallTank.delayPerShoot);
                RotateTurretToPlayer();

                var dir = SmallTank.Target.position - SmallTank.Muzzle.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                var direction = Quaternion.Euler(new Vector3(0, 0, angle));

                var shell = ShellPoolManager.Pool.Get();
                shell.transform.SetPositionAndRotation(SmallTank.Muzzle.position, direction);
                currentShoot++;
            }
        }
    }
    void CheckShouldStartBurst()
    {
        if (!isBurstOngoing && Time.time > nextShootTime)
        {
            isBurstOngoing = true;
        }
    }

    private void CheckIfBurstCompleted()
    {
        if (currentShoot >= SmallTank.shootPerBurst)
        {
            currentShoot = 0;
            isBurstOngoing = false;
            CalculateNextShootTime(SmallTank.delayPerBurst);
        }
    }

    void RotateTurretToPlayer()
    {
        Vector2 dir = SmallTank.Target.position - SmallTank.Turret.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        SmallTank.Turret.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void CalculateNextShootTime(float delay)
    {
        nextShootTime = Time.time + delay;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTartgetAcquired = false;
        }
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }
}
