using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    [SerializeField] private Unit unit;
    private void Start()
    {
 
    }

   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // GridSystemVisual.Instance.HideAllGridPosition();
            // GridSystemVisual.Instance.ShowGridPositionList(unit.GetMoveAction().GetValidActionGridPosList());
            
        }
    }
}
