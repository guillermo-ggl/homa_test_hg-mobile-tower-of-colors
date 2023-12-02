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
}
