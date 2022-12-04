using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

     [SerializeField] private bool isEnemy;

     private const int ACTION_POINTS_MAX = 2;

     public static event EventHandler OnAnyActionPointsChanged;
     public static event EventHandler OnAnyUnitSpawned;
     public static event EventHandler OnAnyUnitDead;

     private GridPosition gridPosition;
     private HealthSystem healthSystem;
     private MoveAction moveAction;
     private SpinAction spinAction;
     private BaseAction[] baseActionArray;
     private int actionPoints = ACTION_POINTS_MAX;

     private void Awake()
     {
          healthSystem = GetComponent<HealthSystem>();
          moveAction = GetComponent<MoveAction>();
          spinAction = GetComponent<SpinAction>();
          baseActionArray = GetComponents<BaseAction>();
     }

     private void Start()
     {
          gridPosition = LevelGrid.Instance.GetGridPos(transform.position);
          LevelGrid.Instance.AddUnitGridPos(gridPosition, this);

          TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
          healthSystem.onDead += HealthSystem_onDead;

          OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
     }
     private void Update()
     {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPos(transform.position);
        if (newGridPosition != gridPosition)
        {
          // Unit changed grid pos
          GridPosition oldGridPosition = gridPosition;
          gridPosition = newGridPosition;
          LevelGrid.Instance.UnitMovePos(this, oldGridPosition, newGridPosition);
        }
     }

     public MoveAction GetMoveAction()
     {
          return moveAction;
     }

     public SpinAction GetSpinAction()
     {
          return spinAction;
     }

     public GridPosition GetGridPosition()
     {
          return gridPosition;
     }

     public Vector3 GetWorldPosition()
     {
          return transform.position;
     }

     public BaseAction[] GetBaseActionArray()
     {
          return baseActionArray;
     }

     public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
     {
          if (CanSpendActionPointsToTakeAction(baseAction))
          {
               SpendActionPoints(baseAction.GetActionPointsCost());
               return true;
          } else
               return false ;
     }

     public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
     {
          if (actionPoints >= baseAction.GetActionPointsCost())
               return true;
          else
               return false;
     }

     private void SpendActionPoints(int amount)
     {
          actionPoints -= amount;

          OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
     }

     public int GetActionPoints()
     {
          return actionPoints;
     }

     private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
     {
          if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || 
               (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
          {
          actionPoints = ACTION_POINTS_MAX;

          OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
          }
     }

     public bool IsEnemy()
     {
          return isEnemy;
     }

     public void Damage(int damageAmout)
     {
          healthSystem.Damage(damageAmout);
     }

     private void HealthSystem_onDead(object sender, EventArgs e)
     {
          LevelGrid.Instance.RemoveUnitGridPos(gridPosition, this);
          Destroy(gameObject);

          OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
     }
}
