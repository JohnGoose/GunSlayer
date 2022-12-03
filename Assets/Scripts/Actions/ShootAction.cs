using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    
    private enum State {
        Aiming,
        Shooting,
        Cooloff,
    }
    private State state;
    private int maxShootDistance = 7;

    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update()
    {
        if (!isActive)
            return ;
        stateTimer -= Time.deltaTime;
        switch (state) {
            case State.Aiming:
            Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
            break ;
        case State.Shooting:
        if (canShootBullet) {
            Shoot();
            canShootBullet = false;
        }
            break ;
        case State.Cooloff:
            break;
        }
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break ;
            case State.Cooloff:
                ActionComplete();
                break;
        }
        // Debug.Log(state);
    }

	public override string GetActionName()
	{
		return "Shoot";
	}

	public override List<GridPosition> GetValidActionGridPosList()
	{
		List<GridPosition> validGridPosList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridPos = unitGridPosition + offsetGridPos;

                if (!LevelGrid.Instance.IsValidGridPos(testGridPos))
                    continue ;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                    continue ;

                if  (!LevelGrid.Instance.HasAnyUnitOnGridPos(testGridPos))
                // Grid position is empty, No unit
                    continue ;
                
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPos(testGridPos);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both units on same team
                    continue;
                }

                validGridPosList.Add(testGridPos);
            }
        }
        return validGridPosList;
	}

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.Damage(40);
    }

	public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
	{
        targetUnit = LevelGrid.Instance.GetUnitAtGridPos(gridPosition);

        // Debug.Log("Aiming");
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        
        canShootBullet = true;

        ActionStart(onActionComplete);
	}

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }


    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
}
