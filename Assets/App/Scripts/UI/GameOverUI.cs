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
    /// �Q�[���I�[�o�[��ʂ�\������
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);

        if (animator.GetBool(showAnimKey)) return;

        StartCoroutine(PlayGameOverAnim());
    }

    /// <summary>
    /// �Q�[���I�[�o�[��ʂ��\���ɂ���
    /// </summary>
    public void Hide()
    {
        animator.SetBool(showAnimKey, false);
    }

    /// <summary>
    /// �Q�[���I�[�o�[��ʂ�\������A�j���[�V�������Đ�����
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayGameOverAnim()
    {
        animator.SetBool(showAnimKey, true);
        yield return AnimatorUtil.WaitForAnimByName(animator, showAnimName);

        isPlayedAnim = true;
        // �Q�[���I�[�o�[�̃A�j���[�V�������I������烁�j���[��\������
        gameOverText.gameObject.SetActive(false);
        menuContents.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstSelectObj);
    }

    /// <summary>
    /// �Q�[���𑱂���ۂ̏���
    /// </summary>
    public void ContinueGame()
    {
        menuContents.SetActive(false);
        SceneTransitionManager.Instance.StartTransitionForReset();

        GamePlayDataManager.continueCount++;

        AudioManager.Instance.StopCurrentBGMWithFade();
    }

    /// <summary>
    /// �^�C�g���֖߂�ۂ̏���
    /// </summary>
    public void BackTitle()
    {
        menuContents.SetActive(false);
        GameSceneManager.Instance.BackTitle();

        AudioManager.Instance.StopCurrentBGMWithFade();
    }
}
