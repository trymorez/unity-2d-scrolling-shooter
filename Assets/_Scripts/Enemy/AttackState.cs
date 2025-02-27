using UnityEngine;
using System;
using System.Collections.Generic;

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
    Transform target, turret, muzzle, transform;
    Waypoints waypoints;

    bool isTimeForNextShoot { get => Time.time > nextShootTime; }

    Dictionary<Vector2, float> spriteAngle = new()
    {
    {Vector2.up, 270f},
    {Vector2.down, 90f},
    {Vector2.left, 180f},
    {Vector2.right, 0f},
    };

    public AttackState(SmallTank smallTank) : base(STankState.Attack) {
        SmallTank = smallTank;
    }

    public override void EnterState()
    {
        //set reference
        target = SmallTank.Target;
        turret = SmallTank.TurretTransform;
        muzzle = SmallTank.Muzzle;
        waypoints = SmallTank.Waypoints;
        transform = SmallTank.transform;

        //initialize variables
        isTartgetAcquired = true;
        isBurstOngoing = false;
        isBurstCompleted = false;
        currentShoot = 0;
        CalculateNextShootTime(SmallTank.DelayPerShoot);

        Vector2 nextDir = waypoints.GetWaypoint();
        Rotate2D(transform, nextDir, Vector2.up);
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
        var vectorToTarget = target.position - muzzle.position;
        float curAngle = turret.rotation.eulerAngles.z;
        float turretAngle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90f;
        float angleDiff = Mathf.DeltaAngle(curAngle, turretAngle);

        bool isTurretAligned = MathF.Abs(angleDiff) <= minAngleDiff;

        if (isTurretAligned)
        {
            //shoot
            currentShoot++;
            CalculateNextShootTime(SmallTank.DelayPerShoot);
            RotateTurretToPlayer();
            SmallTank.Turret.OnTurretMorph();

            SpawnShell(vectorToTarget);
            SpawnMuzzleFlash();
        }
        else 
        {
            //rotate turret smoothly
            float smoothAngleZ = Mathf.LerpAngle(curAngle, turretAngle, Time.deltaTime * turnSpeed);
            turret.rotation = Quaternion.Euler(new Vector3(0, 0, smoothAngleZ));
        }
    }

    void SpawnMuzzleFlash()
    {
        //get muzzle flash from pool manager
        var flash = MuzzleFlashPoolManager.Pool.Get();
        flash.transform.position = muzzle.position;
        flash.transform.SetParent(GameManager.World);
    }

    void SpawnShell(Vector3 targetDir)
    {
        float fireAngleZ = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
        var fireDir = Quaternion.Euler(new Vector3(0, 0, fireAngleZ));
        //get tank shell from pool manager
        var shell = ShellPoolManager.Pool.Get();
        shell.transform.SetPositionAndRotation(muzzle.position, fireDir);
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
        Rotate2D(turret, target, Vector2.down);
    }

    void Rotate2D(Transform sourceTr, Transform targetTr, Vector2 orientation)
    {
        Vector2 vectorToTarget = targetTr.position - sourceTr.position;
        float angleZ = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + spriteAngle[orientation];
        sourceTr.rotation = Quaternion.Euler(new Vector3 (0,0,angleZ));
    }

    void Rotate2D(Transform sourceTr, Vector2 vectorToTarget, Vector2 orientation)
    {
        float angleZ = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + spriteAngle[orientation];
        sourceTr.rotation = Quaternion.Euler(new Vector3(0, 0, angleZ));
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
