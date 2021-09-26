using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MissionManager : BaseManager<MissionManager>
{
    private List<Mission> currentMissionList = new List<Mission>();

    /// <summary>
    /// ミッションを追加する
    /// </summary>
    /// <param name="mission"></param>
    public void AddMission(Mission mission)
    {
        currentMissionList.Add(mission);
        mission.OnAdded();

        GameSceneUIManager.Instance.MissionList.UpdateMissionList();
    }

    /// <summary>
    /// ミッションを完了させる
    /// </summary>
    /// <param name="name"></param>
    public void ComplateMission(MissionName name, bool playSound = true)
    {
        Mission mission = GetMissionByName(name);

        if (mission == null) return;
        if (mission.complete) return;

        mission.Complete();

        GameSceneUIManager.Instance.MissionList.UpdateMissionList();
        StartCoroutine(GameSceneUIManager.Instance.MissionList.PlayClearAnim(name));

        if (playSound)
        {
            AudioManager.Instance.PlaySE("Win sound 16");
        }
    }

    /// <summary>
    /// カウント制ミッションのカウントを加算する
    /// </summary>
    /// <param name="name"></param>
    /// <param name="count"></param>
    public void AddCountOfMission(MissionName name, int count)
    {
        Mission mission = GetMissionByName(name);
        if (mission == null) return;
        if (mission.GetType() != typeof(CountableMission)) return;

        CountableMission contableMission = (CountableMission)mission;
        contableMission.AddCount(count);

        if (contableMission.currentCount >= contableMission.targetCount)
        {
            ComplateMission(name);
        }
        else
        {
            GameSceneUIManager.Instance.MissionList.UpdateMissionList();
        }
    }

    /// <summary>
    /// ミッションを取り除く
    /// </summary>
    /// <param name="name"></param>
    public void RemoveMission(MissionName name)
    {
        Mission mission = GetMissionByName(name);

        if (mission == null) return;

        currentMissionList.Remove(mission);

        GameSceneUIManager.Instance.MissionList.UpdateMissionList();
    }

    /// <summary>
    /// ミッションをリセットする
    /// </summary>
    public void ResetMission()
    {
        currentMissionList.Clear();
    }

    /// <summary>
    /// 現在のミッション一覧を取得する
    /// </summary>
    /// <returns></returns>
    public List<Mission> GetCurrentMission()
    {
        return currentMissionList;
    }

    /// <summary>
    /// ミッションを名前から取得する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Mission GetMissionByName(MissionName name)
    {
        Mission resultMission = null;
        foreach(Mission mission in currentMissionList)
        {
            if (mission.missionName != name) continue;

            resultMission = mission;
            break;
        }

        return resultMission;
    }
}

/*
public enum TrialMissionName
{
    MushEnemyKill,
    GotoMushVillage,
    KillMushBoss,
}
*/

/// <summary>
/// ミッションの名前
/// </summary>
public enum MissionName
{
    ExitValley,
    FindKeoPrison,
    FindHyou,
    KillHyou,
    BackKeoPrison,
    killEnemies,
    BackKeoPrison2,
    KillRose
}

[Serializable]
public class Mission
{
    public MissionName missionName;
    public string title;
    [NonSerialized]
    public bool complete;
    public UnityEvent onCompleted;

    /// <summary>
    /// ミッション達成させる
    /// </summary>
    public virtual void Complete()
    {
        if (complete) return;

        complete = true;
        onCompleted.Invoke();
    }

    /// <summary>
    /// ミッションが追加された時の処理
    /// </summary>
    public virtual void OnAdded()
    {

    }
}

[Serializable]
public class CountableMission : Mission
{
    public int targetCount;
    [NonSerialized]
    public int currentCount;

    /// <summary>
    /// カウントを加算する
    /// </summary>
    /// <param name="count"></param>
    public void AddCount(int count)
    {
        currentCount += count;
    }
}

[Serializable]
public class TowardMission : Mission
{
    public MissionTargetArea targetArea;

    public override void OnAdded()
    {
        base.OnAdded();
        // 目的地アイコンの追従目標にする
        GameSceneUIManager.Instance.DestinationIcon.SetDestination(targetArea.transform);
        // 目的地に着いた時にミッションクリアにさせる
        targetArea.AddReachedAction(() => { MissionManager.Instance.ComplateMission(missionName); });
        // 目的地アイコンを有効化する
        targetArea.gameObject.SetActive(true);
    }

    public override void Complete()
    {
        if (complete) return;
        // クリア時に目的地アイコンを無効化させる
        targetArea.gameObject.SetActive(false);
        GameSceneUIManager.Instance.DestinationIcon.UnSetDestination();

        complete = true;
        onCompleted.Invoke();
    }
}