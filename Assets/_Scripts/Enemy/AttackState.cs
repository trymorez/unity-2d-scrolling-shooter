using Unity.Mathematics;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;

public class AttackState : BaseState<STankState>
{
    public SmallTank SmallTank;

    int currentShoot;
    float nextShootTime;
    bool isBurstOngoing;
    bool isTartgetAcquired;
    bool isBurstCompleted;

    float minAngleDiff = 10f;
    float turnSpeed = 10f;
    Transform target, turret, muzzle;

    bool isTimeForNextShoot { get => Time.time > nextShootTime; }

    public AttackState(SmallTank smallTank) : base(STankState.Attack) {
        SmallTank = smallTank;
    }

    public override void EnterState()
    {
        //set reference
        target = SmallTank.Target;
        turret = SmallTank.TurretTransform;
        muzzle = SmallTank.Muzzle;

        isTartgetAcquired = true;
        isBurstOngoing = false;
        isBurstCompleted = false;
        currentShoot = 0;
        CalculateNextShootTime(SmallTank.DelayPerShoot);
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
        if (isTartgetAcquired && isBurstCompleted)
        {
            return STankState.Move;
        }
        else
        {
            return STankState.Attack;
        }
    }

    public override void UpdateState()
    {
        CheckShouldStartBurst();

        if (isBurstOngoing)
        {
            CheckIfBurstCompleted();

            if (isTimeForNextShoot)
            {
                ShootIfCan();
            }
        }
    }

    void ShootIfCan()
    {
        var targetDir = target.position - muzzle.position;
        float curAngle = turret.rotation.eulerAngles.z;
        float turretAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg + 90f;
        float angleDiff = Mathf.DeltaAngle(curAngle, turretAngle);

        bool isTurretAligned = MathF.Abs(angleDiff) <= minAngleDiff;

        if (isTurretAligned)
        {
            CalculateNextShootTime(SmallTank.DelayPerShoot);
            RotateTurretToPlayer();
            SmallTank.Turret.OnTurretMorph();
            float fireAngleZ = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            var fireDir = Quaternion.Euler(new Vector3(0, 0, fireAngleZ));

            var shell = ShellPoolManager.Pool.Get();
            shell.transform.SetPositionAndRotation(muzzle.position, fireDir);

            var flash = MuzzleFlashPoolManager.Pool.Get();
            flash.transform.position = muzzle.position;
            flash.transform.SetParent(GameManager.World);
            currentShoot++;
        }
        else
        {
            float smoothRotate = Mathf.LerpAngle(curAngle, turretAngle, Time.deltaTime * turnSpeed);
            turret.rotation = Quaternion.Euler(new Vector3(0, 0, smoothRotate));
        }
    }

    void CheckShouldStartBurst()
    {
        if (!isBurstOngoing && Time.time > nextShootTime)
        {
            isBurstOngoing = true;
            isBurstCompleted = false;
        }
    }

    private void CheckIfBurstCompleted()
    {
        if (currentShoot >= SmallTank.ShootPerBurst)
        {
            currentShoot = 0;
            isBurstCompleted = true;
            isBurstOngoing = false;
            CalculateNextShootTime(SmallTank.DelayPerBurst);
        }
    }

    void RotateTurretToPlayer()
    {
        Vector2 dir = target.position - turret.position;
        var fireAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        turret.rotation = Quaternion.Euler(new Vector3(0, 0, fireAngle));
    }

    void CalculateNextShootTime(float delay)
    {
        nextShootTime = Time.time + delay;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTartgetAcquired = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTartgetAcquired = false;
        }
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }
}
