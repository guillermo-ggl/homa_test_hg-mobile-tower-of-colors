using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanelUI : MonoBehaviour
{
    [SerializeField]
    Image check;
    [SerializeField]
    TextMeshProUGUI descriptionText;
    [SerializeField]
    GameObject fillBarContainer;
    [SerializeField]
    Image fillBar;
    [SerializeField]
    TextMeshProUGUI fillBarText;
    [SerializeField]
    GameObject claimButton;
    [SerializeField]
    Image rewardIcon;
    [SerializeField]
    Sprite[] rewardIconSprites;
    [SerializeField]
    TextMeshProUGUI rewardQuantityText;

    private int missionIndex;
    private MissionSystem missionSystem;
    private Action OnCompleted;
    public void PrepareForMission(int missionIndex, MissionSystem missionSystem, Action OnCompleted)
    {
        this.missionIndex = missionIndex;
        this.missionSystem = missionSystem;
        this.OnCompleted = OnCompleted;

        Refresh();
    }

    public void Refresh()
    {
        Mission mission = missionSystem.GetMission(missionIndex);
        bool missionRewardClaimed = missionSystem.GetSelectedMissionRewardClaimed(missionIndex);
        int progress = missionSystem.GetSelectedMissionProgress(missionIndex);
        Reward reward = missionSystem.GetReward(missionIndex);

        check.enabled = missionRewardClaimed;
        descriptionText.text = mission.GetDescription();
        fillBarContainer.SetActive(!missionRewardClaimed && progress < mission.GetGoal());
        fillBar.fillAmount = progress / (float)mission.GetGoal();
        fillBarText.text = progress + "/" + mission.GetGoal();
        claimButton.SetActive(!missionRewardClaimed && progress >= mission.GetGoal());
        rewardIcon.sprite = rewardIconSprites[(int)reward.GetRewardType()];
        rewardQuantityText.text = reward.GetQuantity().ToString();
    }

    public void ClaimButton()
    {
        missionSystem.ClaimMissionReward(missionIndex);
        OnCompleted?.Invoke();
    }
}
