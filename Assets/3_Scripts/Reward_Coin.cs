using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Reward_Coin;

[CreateAssetMenu(menuName = "Coin Reward")]
public class Reward_Coin : Reward
{
    public override void Claim()
    {
        switch (type)
        {
            case Type.SoftCoin:
                SaveData.SoftCoin += quantity;
                break;
            case Type.HardCoin:
                SaveData.HardCoin += quantity;
                break;
        }

        MissionsUI.Instance.RefreshCoins();
        base.Claim();
    }
}
