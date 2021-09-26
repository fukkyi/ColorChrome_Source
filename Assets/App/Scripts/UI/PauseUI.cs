using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(FadeableUI))]
public class PauseUI : MonoBehaviour
{
    public bool isShowingOption = false;

    [SerializeField]
    private GameObject pauseTopContent = null;
    [SerializeField]
    private GameObject backTitleContent = null;
    [SerializeField]
    private GameObject pauseTopFirstSelectObj = null;
    [SerializeField]
    private GameObject backTitleFirstSelectObj = null;

    private FadeableUI fadeable = null;

    private void Awake()
    {
        fadeable = GetComponent<FadeableUI>();
    }

    public void ShowPauseUI()
    {
        gameObject.SetActive(true);
        // ��Unull�����邱�ƂŖ���Select�C�x���g�����΂���悤�ɂ���
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseTopFirstSelectObj);

        fadeable.FadeIn();
    }

    public void ShowOptionUI()
    {
        isShowingOption = true;

        OptionValuesSetter.LoadOptionScene(() => {
            pauseTopContent.SetActive(true);
            EventSystem.current.SetSelectedGameObject(pauseTopFirstSelectObj);
            isShowingOption = false;
        });
        pauseTopContent.SetActive(false);
    }

    public void HidePauseUI()
    {
        OptionValuesSetter.UnLoadOptionScene();
        fadeable.FadeOut(onFinish: () => {
            HideBackTitleContent();
            EventSystem.current.SetSelectedGameObject(null);
            gameObject.SetActive(false);
        });
    }

    public void ShowBackTitleContent()
    {
        pauseTopContent.SetActive(false);
        backTitleContent.SetActive(true);

        EventSystem.current.SetSelectedGameObject(backTitleFirstSelectObj);
    }

    public void HideBackTitleContent()
    {
        backTitleContent.SetActive(false);
        pauseTopContent.SetActive(true);

        EventSystem.current.SetSelectedGameObject(pauseTopFirstSelectObj);
    }

    public void BackTitle()
    {
        backTitleContent.SetActive(false);
        GameSceneManager.Instance.BackTitle();
    }
}
