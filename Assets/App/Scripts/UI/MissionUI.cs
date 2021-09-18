using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MissionUI : MonoBehaviour
{
    [SerializeField]
    private MissionBanner originMissionBanner = null;
    [SerializeField]
    private Text missionLabelText = null;
    [SerializeField]
    private Transform bannerContentsTrans = null;

    [SerializeField]
    private int maxShowMissionCount = 3;
    [SerializeField]
    private float bannerHeight = 70;

    private MissionBanner[] showBanners = new MissionBanner[0];

    private void Awake()
    {
        Array.Resize(ref showBanners, maxShowMissionCount);
        for(int i = 0; i < showBanners.Length; i++)
        {
            Vector2 bannerPos = Vector2.up * -bannerHeight * i;

            MissionBanner generateBanner = Instantiate(originMissionBanner, bannerContentsTrans);

            generateBanner.GetComponent<RectTransform>().anchoredPosition = bannerPos;
            generateBanner.gameObject.SetActive(false);

            showBanners[i] = generateBanner;
        }
    }

    /// <summary>
    /// ミッションリストを更新する
    /// </summary>
    public void UpdateMissionList()
    {
        HideAllBanner();

        List<Mission> currentMission = MissionManager.Instance.GetCurrentMission();
        int currentMissionCount = currentMission.Count;
        // ミッションがない場合はラベルを表示させない
        if (currentMissionCount <= 0)
        {
            missionLabelText.enabled = false;
            return;
        }

        missionLabelText.enabled = true;

        // 取得するミッション数を決める
        int showMissioinCount = Mathf.Min(maxShowMissionCount, currentMissionCount);
        List<Mission> currentShowMission = currentMission.GetRange(0, showMissioinCount);

        int bannerCount = 0;
        foreach(Mission mission in currentShowMission)
        {
            // ミッションが撃破数ミッションならカウント形式のバナーにする
            if (mission.GetType() == typeof(CountableMission))
            {
                CountableMission killMission = (CountableMission)mission;
                showBanners[bannerCount].SetCountMissionDetail(mission.title, killMission.currentCount, killMission.targetCount, mission.complete);
            }
            else
            {
                showBanners[bannerCount].SetMissionDetail(mission.title, mission.complete);
            }

            showBanners[bannerCount].gameObject.SetActive(true);

            bannerCount++;
        }
    }

    /// <summary>
    /// 全てのミッションバナーの表示を消す
    /// </summary>
    public void HideAllBanner()
    {
        foreach(MissionBanner banner in showBanners)
        {
            banner.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// クリア時のアニメーションを再生する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerator PlayClearAnim(MissionName name)
    {
        List<Mission> currentMission = MissionManager.Instance.GetCurrentMission();

        // 取得するミッション数を決める
        int showMissioinCount = Mathf.Min(maxShowMissionCount, currentMission.Count);
        List<Mission> currentShowMission = currentMission.GetRange(0, showMissioinCount);

        int bannerCount = 0;
        MissionBanner clearBanner = null;
        foreach (Mission mission in currentShowMission)
        {
            if (mission.missionName == name)
            {
                clearBanner = showBanners[bannerCount];
            }

            bannerCount++;
        }

        if (clearBanner == null) yield break;

        yield return clearBanner.PlayClearAnim();

        MissionManager.Instance.RemoveMission(name);
    }
}
