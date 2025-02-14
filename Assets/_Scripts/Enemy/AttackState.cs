using UnityEngine;
using System;

public class AttackState : BaseState<STankState>
{
    public STank stank;

    float speed = 3f;
    int currentShoot;
    float nextShootTime;
    bool burstOngoing;

    bool isTimeForNextShoot { get => Time.time > nextShootTime; }
    bool tartgetAcquired;

    public AttackState() : base(STankState.Attack) { }

    public override void EnterState()
    {
        tartgetAcquired = true;
        burstOngoing = false;
        currentShoot = 0;
        CalculateNextShootTime(stank.delayPerShoot);
    }

    public override void ExitState()
    {
    }

    public override STankState GetNextState()
    {
        if (!tartgetAcquired && !burstOngoing)
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

        if (burstOngoing)
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
                shell.SetDirection(dir.normalized);
                currentShoot++;
            }
        }
    }
    void CheckShouldStartBurst()
    {
        if (!burstOngoing && Time.time > nextShootTime)
        {
            burstOngoing = true;
        }
    }

    private void CheckIfBurstCompleted()
    {
        if (currentShoot >= stank.shootPerBurst)
        {
            currentShoot = 0;
            burstOngoing = false;
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
            tartgetAcquired = false;
        }
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }
}
