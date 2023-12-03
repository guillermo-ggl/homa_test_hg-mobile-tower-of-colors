using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(menuName = "Mission System")]
public class MissionSystem : ScriptableObject
{
    [Header("GAME CONFIG")]
    [SerializeField]
    int ActiveEasyMissions;
    [SerializeField]
    int ActiveMediumMissions;
    [SerializeField]
    int ActiveHardMissions;

    [Header("MISSIONS")]
    [SerializeField]
    Mission[] EasyMissions;
    [SerializeField]
    Mission[] MediumMissions;
    [SerializeField]
    Mission[] HardMissions;

    [Header("REWARDS")]
    [SerializeField]
    Reward[] EasyRewards;
    [SerializeField]
    Reward[] MediumRewards;
    [SerializeField]
    Reward[] HardRewards;

    public Mission GetMission(int index)
    {
        return GetMissionTypeList(index)[GetSelectedMissionIndex(index)];
    }

    public Reward GetReward(int index)
    {
        return GetRewardTypeList(index)[GetSelectedMissionRewardIndex(index)];
    }

    #region MissionSaveDataPrefs
    public int GetSelectedMissionIndex(int index)
    {
        return PlayerPrefs.GetInt("SelectedMissionIndex_" + index, - 1);
    }

    public void SetSelectedMissionIndex(int index, int value)
    {
        PlayerPrefs.SetInt("SelectedMissionIndex_" + index, value);
    }

    public int GetSelectedMissionRewardIndex(int index)
    {
        return PlayerPrefs.GetInt("SelectedMissionRewardIndex_" + index, -1);
    }

    public void SetSelectedMissionRewardIndex(int index, int value)
    {
        PlayerPrefs.SetInt("SelectedMissionRewardIndex_" + index, value);
    }
    public int GetSelectedMissionProgress(int index)
    {
        return PlayerPrefs.GetInt("SelectedMissionProgress_" + index, 0);
    }

    public void SetSelectedMissionProgress(int index, int value)
    {
        PlayerPrefs.SetInt("SelectedMissionProgress_" + index, value);
    }

    public bool GetSelectedMissionRewardClaimed(int index)
    {
        return PlayerPrefs.GetInt("SelectedMissionRewardClaimed_" + index) != 0;
    }
    public void SetSelectedMissionRewardClaimed(int index, bool value)
    {
        PlayerPrefs.SetInt("SelectedMissionRewardClaimed_" + index, value ? 1:0);
    }

    #endregion

    public void ResetMissions()
    {
        for(int i = 0; i < ActiveEasyMissions + ActiveMediumMissions + ActiveHardMissions; i++)
        {
            Mission[] possibleMissions = GetMissionTypeList(i);
            int excludedMission = GetSelectedMissionIndex(i);
            int selectedMission = GiveMeANumber(0, possibleMissions.Length - 1, new HashSet<int> { excludedMission });
            SetSelectedMissionIndex(i, selectedMission);

            SetSelectedMissionProgress(i, 0);


            Reward[] possibleRewards = GetRewardTypeList(i);
            int selectedReward = UnityEngine.Random.Range(0, possibleRewards.Length);
            SetSelectedMissionRewardIndex(i, selectedReward);

            SetSelectedMissionRewardClaimed(i, false);
        }
    }

    public void ReportData(Mission.DataType dataType, int quantity)
    {
        if (RemoteConfig.MISSIONS_ENABLED)
        {
            for (int i = 0; i < GetTotalMissions(); i++)
            {
                Mission mission = GetMission(i);
                if (mission.GetDataType() == dataType)
                {
                    int result = mission.ReportQuantity(GetSelectedMissionProgress(i), quantity);
                    SetSelectedMissionProgress(i, result);
                }
            }
        }
    }

    public void ClaimMissionReward(int index)
    {
        GetReward(index).Claim();
        SetSelectedMissionRewardClaimed(index, true);

        if (CheckResetMissions())
        {
            ResetMissions();
        }
    }

    public bool CheckResetMissions()
    {
        bool reset = false;
        bool allClaimed = true;
        for(int i = 0; !reset && i < GetTotalMissions(); i++)
        {
            if (GetSelectedMissionIndex(i) < 0)
            {
                reset = true;
            }
            else if(!GetSelectedMissionRewardClaimed(i))
            {
                allClaimed = false;
            }
        }

        if(allClaimed)
        {
            reset = true;
        }

        return reset;
    }

    public static int GiveMeANumber(int from, int to, HashSet<int> excluded) // to included, if all numbers excluded "from" returned
    {
        List<int> numberList = ListPool<int>.Get();

        for (int i = from; i <= to; i++)
        {
            if (!excluded.Contains(i))
            {
                numberList.Add(i);
            }
        }

        if (numberList.Count <= 0)
        {
            return from;
        }

        int rd = UnityEngine.Random.Range(0, numberList.Count);

        return numberList[rd];

    }


    private Mission[] GetMissionTypeList(int index)
    {
        if (index < ActiveEasyMissions)
        {
            return EasyMissions;
        }
        else if (index < ActiveEasyMissions + ActiveMediumMissions)
        {
            return MediumMissions;
        }
        else
        {
            return HardMissions;
        }
    }

    private Reward[] GetRewardTypeList(int index)
    {
        if (index < ActiveEasyMissions)
        {
            return EasyRewards;
        }
        else if (index < ActiveEasyMissions + ActiveMediumMissions)
        {
            return MediumRewards;
        }
        else
        {
            return HardRewards;
        }
    }

    public int GetTotalMissions()
    {
        return ActiveEasyMissions + ActiveMediumMissions + ActiveHardMissions;
    }
}
