using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    [SerializeField]
    List<Animation> encouragementList;
    [SerializeField]
    TextMeshProUGUI comboText;
    [SerializeField]
    Animator comboAnimation;
    [SerializeField]
    int minCount = 5;
    [SerializeField]
    int countForEncouragement = 10;
    [SerializeField]
    float finishComboTime = 1;
    [SerializeField]
    RectTransform comboRect;
    [SerializeField]
    float moveTime;

    int currentCombo = 0;
    Coroutine resetComboRoutine;

    Camera mainCamera;
    Vector3 targetPos;
    bool showingCombo = false;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Lerp is not supposed to be used like this but in this case it gives a nice non-linear smoothing without having to store extra data.
        if (showingCombo)
            comboRect.position = Vector3.Lerp(comboRect.position, targetPos, moveTime * Time.deltaTime);
    }

    private void OnTileDestroyed(TowerTile obj)
    {
        CountCombo(obj.transform.position);
    }

    public void CountCombo(Vector3 worldPos)
    {
        if (this.isActiveAndEnabled) {
            if (++currentCombo > minCount) {
                targetPos = mainCamera.WorldToScreenPoint(worldPos);
                comboText.text = $"x{currentCombo}";
                if (!showingCombo) {
                    showingCombo = true;
                    comboRect.position = targetPos;
                    comboAnimation.SetBool("show", true);
                }
                comboAnimation.SetTrigger("bounce");
            }
            if (resetComboRoutine != null)
                StopCoroutine(resetComboRoutine);
            resetComboRoutine = StartCoroutine(FinishCombo());
        }
    }

    void ShowEncouragement()
    {
        Animation randomAnim = encouragementList[Random.Range(0, encouragementList.Count)];
        randomAnim.transform.position = targetPos + Vector3.up * 50;
        randomAnim.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(8, 20) * Mathf.Sign(Random.value - 0.5f));
        randomAnim.Play();
    }

    IEnumerator FinishCombo()
    {
        yield return new WaitForSeconds(finishComboTime * Time.timeScale);
        if (currentCombo >= countForEncouragement)
            ShowEncouragement();
        showingCombo = false;
        comboAnimation.SetBool("show", false);
        currentCombo = 0;
        resetComboRoutine = null;
    }
}
