using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBanner : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private Text basicTitleText = null;
    [SerializeField]
    private Text countTitleText = null;
    [SerializeField]
    private Text countText = null;
    [SerializeField]
    private GameObject checkMark = null;
    [SerializeField]
    private GameObject basicContent = null;
    [SerializeField]
    private GameObject countContent = null;

    private string clearAnimName = "Clear";

    public void SetMissionDetail(string title, bool complate)
    {
        basicTitleText.text = title;
        checkMark.SetActive(complate);

        basicContent.SetActive(true);
        countContent.SetActive(false);
    }

    public void SetCountMissionDetail(string title, int currentCount, int targetCount, bool complate)
    {
        countTitleText.text = title;
        countText.text = string.Format("{0} / {1}", currentCount, targetCount);
        checkMark.SetActive(complate);

        basicContent.SetActive(false);
        countContent.SetActive(true);
    }

    public IEnumerator PlayClearAnim()
    {
        animator.SetTrigger("Clear");
        yield return AnimatorUtil.WaitForAnimByName(animator, clearAnimName);
    }
}
