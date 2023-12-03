using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionsUI : Singleton<MissionsUI>
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    MissionPanelUI[] missionPanelUIs;

    [SerializeField]
    Animator animator;

    [SerializeField]
    TextMeshProUGUI softCoinsText, hardCoinsText;

    bool isOpen = false;
    void Awake()
    {
        animator.speed = 1.0f / Time.timeScale;
        isOpen = false;
        animator.SetBool("isOpen", isOpen);

        for (int i = 0; i < gameManager.missionSystem.GetTotalMissions() && i < missionPanelUIs.Length; i++)
        {
            missionPanelUIs[i].PrepareForMission(i, gameManager.missionSystem, RefreshPanels);
        }

        RefreshCoins();
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);

        if(isOpen)
        {
            RefreshPanels();
        }
    }

    public void RefreshPanels()
    {
        for (int i = 0; i < missionPanelUIs.Length; i++)
        {
            missionPanelUIs[i].Refresh();
        }
    }

    public void RefreshCoins()
    {
        softCoinsText.text = SaveData.SoftCoin.ToString();
        hardCoinsText.text = SaveData.HardCoin.ToString();
    }
}
