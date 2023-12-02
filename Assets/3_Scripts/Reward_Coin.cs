using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Reward_Coin;

[CreateAssetMenu(menuName = "Coin Reward")]
public class Reward_Coin : Reward
{
    [SerializeField]
    CoinType coinType;

    public override void Claim()
    {
        switch (coinType)
        {
            case CoinType.SoftCoin:
                SaveData.SoftCoin += quantity;
                break;
            case CoinType.HardCoin:
                SaveData.HardCoin += quantity;
                break;
        }
        base.Claim();
    }

    public enum CoinType
    {
        SoftCoin,
        HardCoin
    }
}
