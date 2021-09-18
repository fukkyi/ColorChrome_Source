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
        GameSceneUIManager.Instance.MissionList.UpdateMissionList();
    }

    /// <summary>
    /// ミッションを完了させる
    /// </summary>
    /// <param name="name"></param>
    public void ComplateMission(MissionName name)
    {
        Mission mission = GetMissionByName(name);

        if (mission == null) return;
        if (mission.complete) return;

        mission.Complete();

        GameSceneUIManager.Instance.MissionList.UpdateMissionList();
        StartCoroutine(GameSceneUIManager.Instance.MissionList.PlayClearAnim(name));

        AudioManager.Instance.PlaySE("Win sound 16");
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

/// <summary>
/// ミッションの名前
/// </summary>
public enum MissionName
{
    MushEnemyKill,
    GotoMushVillage,
    KillMushBoss,
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
    public void Complete()
    {
        if (complete) return;

        complete = true;
        onCompleted.Invoke();
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