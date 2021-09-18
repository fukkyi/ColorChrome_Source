using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// 最小値と最大値がある範囲の値クラス
/// </summary>
[Serializable]
public class MinMaxRange
{
    public float min;
    public float max;
    public float minLimit;
    public float maxLimit;

    public float Range { get { return max - min; } }

    public MinMaxRange(float _minLimit = 0, float _maxLimit = 1.0f) {
        minLimit = _minLimit;
        maxLimit = Mathf.Max(_minLimit, _maxLimit);
        min = minLimit;
        max = maxLimit;
    }

    public bool IsMatchRange(float value) {
        return value >= min && value <= max;
    }

    public float RandOfRange()
    {
        return UnityEngine.Random.Range(min, max);
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMaxRange))]
public class MinMaxRangeDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        using (new EditorGUI.PropertyScope(position, label, property)) {

            SerializedProperty minProperty = property.FindPropertyRelative("min");
            SerializedProperty maxProperty = property.FindPropertyRelative("max");

            float min = minProperty.floatValue;
            float max = maxProperty.floatValue;
            float minLimit = property.FindPropertyRelative("minLimit").floatValue;
            float maxLimit = property.FindPropertyRelative("maxLimit").floatValue;

            Rect labelRect = new Rect(position) {
                width = EditorGUIUtility.labelWidth
            };
            float indentWidth = labelRect.width - EditorGUI.IndentedRect(labelRect).width;

            Rect sliderRect = new Rect(position) {
                x = labelRect.xMax - indentWidth,
                width = position.width - labelRect.width + indentWidth,
                height = EditorGUIUtility.singleLineHeight
            };
            Rect minValueRect = new Rect(sliderRect) {
                y = position.y + EditorGUIUtility.singleLineHeight,
                width = 60.0f + indentWidth
            };
            Rect maxValueRect = new Rect(minValueRect) {
                x = sliderRect.xMax - minValueRect.width
            };

            EditorGUI.LabelField(labelRect, label);

            EditorGUI.BeginChangeCheck(); {
                min = EditorGUI.DelayedFloatField(minValueRect, min);
                max = EditorGUI.DelayedFloatField(maxValueRect, max);
            }
            if (EditorGUI.EndChangeCheck()) {
                min = Mathf.Min(min, maxLimit);
                max = Mathf.Clamp(max, min, maxLimit);
                min = Mathf.Clamp(min, minLimit, maxLimit);
                maxProperty.floatValue = max;
                minProperty.floatValue = min;
            }

            EditorGUI.BeginChangeCheck(); {
                EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, minLimit, maxLimit);
            }
            if (EditorGUI.EndChangeCheck()) {
                min = Mathf.Min(min, maxLimit);
                max = Mathf.Max(max, minLimit);
                minProperty.floatValue = min;
                maxProperty.floatValue = max;
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUIUtility.singleLineHeight * 2;
    }
}
#endif
