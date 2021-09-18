using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

public class CustomButton : Button
{
    public string selectSoundName = string.Empty;
    public string submitSoundName = string.Empty;

    /// <summary>
    /// カーソルが移動した際のイベント
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);
        PlayButtonSound(selectSoundName);
    }

    /// <summary>
    /// 決定した際のイベント
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);

        if (submitSoundName == string.Empty) return;

        PlayButtonSound(submitSoundName);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        PlayButtonSound(submitSoundName);
    }

    private void PlayButtonSound(string soundName)
    {
        if (soundName == string.Empty) return;

        AudioManager.Instance.PlaySE(soundName);
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects, CustomEditor(typeof(CustomButton), true)]
public class CustomButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("selectSoundName"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("submitSoundName"), true);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
