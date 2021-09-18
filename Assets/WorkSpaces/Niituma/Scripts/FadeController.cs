using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(MaterialSetter))]
public class FadeController : MonoBehaviour
{
    public static FadeController Instance { get { return _instance; } }
    static FadeController _instance;

    public Material GetFadeMat { get { return mat; } }

    public bool startFadeIn = false;
    public  float fadeTime;

    private Material mat;

    private float _value;
    private bool _isBoot;

    void Awake()
    {
        _instance = this;
        mat = GetComponent<Image>().material;
    }
    public void FadeIn(Action call = null)
    {
        if(!_isBoot) StartCoroutine(In(call));
    }

    public void FadeOut(Action call = null)
    {
        if (!_isBoot) StartCoroutine(Out(call));
    }

    void Start()
    {
        if (startFadeIn) { FadeIn(); }
    }
    public IEnumerator Out(Action call, bool unScaleTime = false)
    {
        _isBoot = true;
        GS_Parameter.SetThreshold(mat, 0);
        _value = 0;
        while (_value < 1)
        {
            _value += TimeUtil.GetDeltaTime(unScaleTime) * fadeTime;
            if (1 <= _value) _value = 1;
            GS_Parameter.SetThreshold(mat, _value);
            yield return null;
        }
        if (call != null) { call(); }
        _isBoot = false;
    }

    public IEnumerator In(Action call, bool unScaleTime = false)
    {
        _isBoot = true;
        GS_Parameter.SetThreshold(mat, 1);
        _value = 1;
        while (0 < _value)
        {
            _value -= TimeUtil.GetDeltaTime(unScaleTime) * fadeTime;
            if (_value <= 0) _value = 0;
            GS_Parameter.SetThreshold(mat, _value);
            yield return null;
        }
        if(call != null) { call(); }
        _isBoot = false;
    }
}
