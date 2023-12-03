using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Additive Mission")]
public class Mission_Additive : Mission
{
    public override int ReportQuantity(int actualProgress, int quantity)
    {
        return Mathf.Min(actualProgress + quantity, goal);
    }

    public override string GetDescription()
    {
        string result = "";
        switch (GetDataType())
        {
            case DataType.TileEliminated:
                result = "Destroy {0} barrel";
                break;
            case DataType.TileExploded:
                result = "Explode {0} barrel";
                break;
            case DataType.SunkTile:
                result = "Sink {0} barrel";
                break;
            case DataType.LevelCompleted:
                result = "Complete {0} level";
                break;
            default:
                return base.GetDescription();
        }

        result = result.Replace("{0}", GetGoal().ToString());
        result += GetGoal() > 1 ? "s" : "";
        return result;
    }
}
