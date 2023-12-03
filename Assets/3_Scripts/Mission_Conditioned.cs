using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conditioned Mission")]
public class Mission_Conditioned : Mission
{
    [SerializeField]
    int condition;
    public override int ReportQuantity(int actualProgress, int quantity)
    {
        if(quantity > condition)
        {
            return Mathf.Min(actualProgress + 1, goal);
        }

        return actualProgress;
    }

    public override string GetDescription()
    {
        string result = "";
        switch (GetDataType())
        {
            case DataType.Combo:
                result ="Reach a {0} combo {1} time";
                break;
            default:
                return base.GetDescription();
        }

        result = result.Replace("{0}", condition.ToString()).Replace("{1}", GetGoal().ToString());
        result += GetGoal() > 1 ? "s" : "";
        return result;
    }
}
