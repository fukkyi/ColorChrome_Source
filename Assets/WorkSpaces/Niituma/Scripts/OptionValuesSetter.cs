using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

public class OptionValuesSetter : MonoBehaviour
{
    private static readonly string OptionSceneName = "OptionScene";
    private static bool isLoadedOptionScene = false;
    private static Action unLoadedAction = null;

    [SerializeField] private Slider cSpeedSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Toggle[] xyToggles = new Toggle[2];

    [SerializeField] private Text[] labels = new Text[(int)SelectType.SelectTypeMax];
    [SerializeField] private Color selectColor;

    [SerializeField] private Text[] sliderPerTexts = new Text[2];

    [SerializeField]
    private AudioMixer audioMixer = null;

    private Gamepad gamepad;
    private int selectNum;
    private int maxNum;
    private SelectType selectType;

    private enum SelectType
    {
        CameraSpeed,
        BgmVolume,
        XToggle,
        YToggle,
        SelectTypeMax,
    }

    private void Start()
    {
        gamepad = Gamepad.current;
        cSpeedSlider.value = OptionValues.cameraSpeed;
        bgmVolumeSlider.value = OptionValues.bgmVolume;
        xyToggles[0].isOn = OptionValues.xAxisLeftAndRightReversals;
        xyToggles[1].isOn = OptionValues.yAxisUpsideDown;
        selectNum = 0;
        maxNum = labels.Length;
        sliderPerTexts[0].text = (OptionValues.cameraSpeed * 100).ToString() + "%";
        sliderPerTexts[1].text = (OptionValues.bgmVolume * 100).ToString() + "%";
        SelectLabel(true);
    }

    public void OnCameraSliderChanged(float value)
    {
        OptionValues.cameraSpeed = cSpeedSlider.value;
        sliderPerTexts[0].text = (Mathf.Floor(OptionValues.cameraSpeed * 100)).ToString() + "%";
    }

    public void OnBGMSliderChanged(float value)
    {
        OptionValues.bgmVolume = bgmVolumeSlider.value;
        sliderPerTexts[1].text = (Mathf.Floor(OptionValues.bgmVolume * 100)).ToString() + "%";

        audioMixer.SetFloat("BGM", Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(OptionValues.bgmVolume, 0f, 1f)), -80f, 0f));
    }

    public void OnXReverseToggleChanged(bool isOn)
    {
        OptionValues.xAxisLeftAndRightReversals = isOn;
    }

    public void OnYReverseToggleChanged(bool isOn)
    {
        OptionValues.yAxisUpsideDown = isOn;
    }


    private void SelectLabel()
    {
        if (gamepad.rightStick.down.wasPressedThisFrame)
        {
            selectNum += selectNum < maxNum - 1 ? 1 : 0;
            selectType = (SelectType)selectNum;
            SelectLabel(true);
        }
        if (gamepad.rightStick.up.wasPressedThisFrame)
        {
            selectNum -= 0 < selectNum ? 1 : 0;
            selectType = (SelectType)selectNum;
            SelectLabel(false);
        }
    }

    private void SelectSliderValueSetting(SelectType type)
    {
        if (gamepad.rightStick.right.isPressed)
        {
            if (type == SelectType.CameraSpeed)
            {
                cSpeedSlider.value += cSpeedSlider.value <= 2 ? TimeUtil.GetDeltaTime(true) / 2 : 0;
                OptionValues.cameraSpeed = cSpeedSlider.value;
            }
            else if (type == SelectType.BgmVolume)
            {
                bgmVolumeSlider.value += bgmVolumeSlider.value <= 1 ? TimeUtil.GetDeltaTime(true) / 2 : 0;
                OptionValues.bgmVolume = bgmVolumeSlider.value;

                audioMixer.SetFloat("BGM" , Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(OptionValues.bgmVolume, 0f, 1f)), -80f, 0f));
            }
        }
        if (gamepad.rightStick.left.isPressed)
        {
            if (type == SelectType.CameraSpeed)
            {
                cSpeedSlider.value -= 0 <= cSpeedSlider.value ? TimeUtil.GetDeltaTime(true) / 2 : 0;
                OptionValues.cameraSpeed = cSpeedSlider.value;
            }
            else if (type == SelectType.BgmVolume)
            {
                bgmVolumeSlider.value -= 0 <= bgmVolumeSlider.value ? TimeUtil.GetDeltaTime(true) / 2 : 0;
                OptionValues.bgmVolume = bgmVolumeSlider.value;

                audioMixer.SetFloat("BGM", Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(OptionValues.bgmVolume, 0f, 1f)), -80f, 0f));
            }
        }
        sliderPerTexts[0].text = (Mathf.Floor(OptionValues.cameraSpeed * 100)).ToString() + "%";
        sliderPerTexts[1].text = (Mathf.Floor(OptionValues.bgmVolume * 100)).ToString() + "%";
    }

    private void SelectXyToggleSetting(SelectType type)
    {
        if (gamepad.circleButton.wasPressedThisFrame)
        {
            if (type == SelectType.XToggle)
            {
                xyToggles[0].isOn = !xyToggles[0].isOn;
                OptionValues.xAxisLeftAndRightReversals = xyToggles[0].isOn;
            }
            else if (type == SelectType.YToggle)
            {
                xyToggles[1].isOn = !xyToggles[1].isOn;
                OptionValues.yAxisUpsideDown = xyToggles[1].isOn;
            }
        }
    }

    private void SelectLabel(bool add)
    {
        labels[selectNum].color = selectColor;
        if (add)
        {
            if (0 < selectNum) { labels[selectNum - 1].color = Color.white; }
        }
        else
        {
            labels[selectNum + 1].color = Color.white;
        }
    }

    private void DefaultCameraSpeed()
    {
        OptionValues.cameraSpeed = 0.5f;
        OptionValues.bgmVolume = 1f;
        OptionValues.seVolume = 1f;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.isPressed)
        {
            UnLoadOptionScene();
        }

        if (gamepad == null) { return; }

        SelectLabel();
        SelectSliderValueSetting(selectType);
        SelectXyToggleSetting(selectType);

        if (gamepad.crossButton.isPressed)
        {
            UnLoadOptionScene();
        }
    }

    public static void LoadOptionScene(Action unLoadedAction = null)
    {
        SceneManager.LoadScene(OptionSceneName, LoadSceneMode.Additive);
        OptionValuesSetter.unLoadedAction = unLoadedAction;

        isLoadedOptionScene = true;
}

    public static void UnLoadOptionScene()
    {
        if (!isLoadedOptionScene) return;

        unLoadedAction?.Invoke();
        SceneManager.UnloadSceneAsync(OptionSceneName);

        isLoadedOptionScene = false;
    }
}