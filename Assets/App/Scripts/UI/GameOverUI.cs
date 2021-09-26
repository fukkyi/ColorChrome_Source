using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private Text gameOverText = null;
    [SerializeField]
    private GameObject menuContents = null;
    [SerializeField]
    private GameObject firstSelectObj = null;

    private string showAnimKey = "Show";
    private string showAnimName = "GameOver_Show";

    private bool isPlayedAnim = false;

    /// <summary>
    /// ゲームオーバー画面を表示する
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);

        if (animator.GetBool(showAnimKey)) return;

        StartCoroutine(PlayGameOverAnim());
    }

    /// <summary>
    /// ゲームオーバー画面を非表示にする
    /// </summary>
    public void Hide()
    {
        animator.SetBool(showAnimKey, false);
    }

    /// <summary>
    /// ゲームオーバー画面を表示するアニメーションを再生する
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayGameOverAnim()
    {
        animator.SetBool(showAnimKey, true);
        yield return AnimatorUtil.WaitForAnimByName(animator, showAnimName);

        isPlayedAnim = true;
        // ゲームオーバーのアニメーションが終わったらメニューを表示する
        gameOverText.gameObject.SetActive(false);
        menuContents.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstSelectObj);
    }

    /// <summary>
    /// ゲームを続ける際の処理
    /// </summary>
    public void ContinueGame()
    {
        menuContents.SetActive(false);
        SceneTransitionManager.Instance.StartTransitionForReset();

        GamePlayDataManager.continueCount++;

        AudioManager.Instance.StopCurrentBGMWithFade();
    }

    /// <summary>
    /// タイトルへ戻る際の処理
    /// </summary>
    public void BackTitle()
    {
        menuContents.SetActive(false);
        GameSceneManager.Instance.BackTitle();

        AudioManager.Instance.StopCurrentBGMWithFade();
    }
}
