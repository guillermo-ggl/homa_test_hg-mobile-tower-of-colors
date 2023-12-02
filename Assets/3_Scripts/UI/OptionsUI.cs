using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField]
    CustomToggle colorblindToggle;
    [SerializeField]
    CustomToggle vibrationToggle;

    Animator animator;
    bool isOpen = false;
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 1.0f / Time.timeScale;
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
        colorblindToggle.SetEnabled(SaveData.CurrentColorList == 1);
        vibrationToggle.SetEnabled(SaveData.VibrationEnabled == 1);
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }

    public void OnColorblindClick(bool value)
    {
        if (SaveData.CurrentColorList == 1 != value) {
            SaveData.CurrentColorList = value ? 1 : 0;
            TileColorManager.Instance.SetColorList(SaveData.CurrentColorList);
        }
    }

    public void OnVibrationClick(bool value)
    {
        if (SaveData.VibrationEnabled == 1 != value) {
            SaveData.VibrationEnabled = value ? 1 : 0;
            if (value)
                Handheld.Vibrate();
        }
    }
}
