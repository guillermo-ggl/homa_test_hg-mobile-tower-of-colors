using UnityEngine;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{
    [SerializeField]
    GameObject enabledObject;
    [SerializeField]
    GameObject disabledObject;
    [SerializeField]
    Toggle.ToggleEvent toggleEvent = new Toggle.ToggleEvent();

    public bool isEnabled { get; private set; }
    Button button;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        isEnabled = false;
    }

    public void OnClick()
    {
        SetEnabled(!isEnabled);
    }

    public void SetEnabled(bool value)
    {
        isEnabled = value;
        enabledObject.SetActive(isEnabled);
        disabledObject.SetActive(!isEnabled);
        toggleEvent?.Invoke(isEnabled);
    }
}
