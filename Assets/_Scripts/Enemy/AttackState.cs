using UnityEngine;
using System;

public class AttackState : BaseState<STankState>
{
    float speed = 3f;
    
    int shootPerBurst = 3;
    int currentShoot;
    float delayPerBurst = 2f;
    float delayPerShoot = 0.5f;
    float nextShootTime;
    bool isBurstOngoing;

    public static TankShell tankShell;
    public static Transform target;
    public static Transform turret;
    public static Transform muzzle;
    bool tartgetAcquired;

    public AttackState() : base(STankState.Attack) { }

    public override void EnterState()
    {
        tartgetAcquired = true;
        isBurstOngoing = false;
        currentShoot = 0;
        CalculateNextShootTime(delayPerShoot);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        CheckShouldStartBurst();
        RotateTurretToPlayer();

        if (isBurstOngoing)
        {
            CheckIfBurstCompleted();

            if (Time.time > nextShootTime)
            {
                CalculateNextShootTime(delayPerShoot);
                RotateTurretToPlayer();
                Debug.Log("shoot!");

                Vector2 dir = target.position - muzzle.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
                var direction = Quaternion.Euler(new Vector3(0, 0, angle));
                var shell = UnityEngine.Object.Instantiate(tankShell, muzzle.position, direction);
                shell.transform.rotation = direction;
                shell.SetDirection(new Vector3(0, 0, angle));
            }
        }
    }

    private void CheckIfBurstCompleted()
    {
        if (++currentShoot >= shootPerBurst)
        {
            currentShoot = 0;
            isBurstOngoing = false;
            CalculateNextShootTime(delayPerBurst);
        }
    }

    void RotateTurretToPlayer()
    {
        Vector2 dir = target.position - turret.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        turret.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void CalculateNextShootTime(float delay)
    {
        nextShootTime = Time.time + delay;
    }

    void CheckShouldStartBurst()
    {
        if (!isBurstOngoing && Time.time > nextShootTime)
        {
            isBurstOngoing = true;
        }
    }


    public override STankState GetNextState()
    {
        if (!tartgetAcquired && !isBurstOngoing)
        {
            return STankState.Idle;
        }
        else
        {
            return STankState.Attack;
        }
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
