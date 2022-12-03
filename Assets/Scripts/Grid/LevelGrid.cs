using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{

    public event EventHandler OnAnyUnitMovedGridPos;
    public static LevelGrid Instance { get; private set; }
    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one LevelGrid!! " + transform + " - " + Instance);
            Destroy(gameObject);
            return ;
        }
        Instance = this;

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public void AddUnitGridPos(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListGridPos(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitGridPos(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovePos(Unit unit, GridPosition fromGridPos, GridPosition toGridPos)
    {
        RemoveUnitGridPos(fromGridPos, unit);

        AddUnitGridPos(toGridPos, unit);

        OnAnyUnitMovedGridPos?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPos(Vector3 worldpos) => gridSystem.GetGridPosition(worldpos);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPos(GridPosition gridPosition) => gridSystem.IsValidGridPos(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyUnitOnGridPos(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPos(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }




}
