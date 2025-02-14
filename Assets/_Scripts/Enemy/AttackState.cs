using UnityEngine;
using System;

public class AttackState : BaseState<STankState>
{
    public STank stank;

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
        CalculateNextShootTime(stank.delayPerShoot);
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
                CalculateNextShootTime(stank.delayPerShoot);
                RotateTurretToPlayer();

                var dir = stank.target.position - stank.muzzle.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                var direction = Quaternion.Euler(new Vector3(0, 0, angle));
                var shell = UnityEngine.Object.Instantiate(stank.tankShell, stank.muzzle.position, direction);
                //shell.SetDirection(dir.normalized);
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
        if (currentShoot >= stank.shootPerBurst)
        {
            currentShoot = 0;
            isBurstOngoing = false;
            CalculateNextShootTime(stank.delayPerBurst);
        }
    }

    void RotateTurretToPlayer()
    {
        Vector2 dir = stank.target.position - stank.turret.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        stank.turret.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
