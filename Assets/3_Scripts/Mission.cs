using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : ScriptableObject
{
    [SerializeField] 
    DataType dataType;
    [SerializeField]
    protected int goal;

    public virtual int ReportQuantity(int actualProgress, int quantity)
    {
        return 0;
    }

    public virtual string GetDescription()
    {
        return "";
    }

    public DataType GetDataType() { return dataType; }

    public int GetGoal() { return goal; }


    public enum DataType
    {
        LevelCompleted,
        Combo,
        TileEliminated,
        TileExploded,
        SunkTile
    }
}
