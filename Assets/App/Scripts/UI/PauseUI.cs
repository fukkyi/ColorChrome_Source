using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(FadeableUI))]
public class PauseUI : MonoBehaviour
{
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
        // ˆê’Unull‚ð“ü‚ê‚é‚±‚Æ‚Å–ˆ‰ñSelectƒCƒxƒ“ƒg‚ª”­‰Î‚·‚é‚æ‚¤‚É‚·‚é
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseTopFirstSelectObj);

        fadeable.FadeIn();
    }

    public void HidePauseUI()
    {
        fadeable.FadeOut(onFinish: () => {
            HideBackTitleContent();
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
