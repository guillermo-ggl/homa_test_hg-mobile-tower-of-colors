using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    [SerializeField]
    protected Type type;

    [SerializeField]
    protected int quantity;

    virtual public void Claim()
    {

    }

    public Type GetRewardType(){ return type; }

    public int GetQuantity() { return quantity; }

    public enum Type
    {
        SoftCoin,
        HardCoin
    }
}
