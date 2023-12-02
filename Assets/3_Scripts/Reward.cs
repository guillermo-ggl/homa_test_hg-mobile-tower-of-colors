using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    [SerializeField]
    protected int quantity;

    virtual public void Claim()
    {

    }
}
