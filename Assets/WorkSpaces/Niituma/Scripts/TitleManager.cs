using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    [SerializeField] private Image _image;
    [SerializeField] private float _graySpeed = 1;
    [SerializeField] private float bgmFadeInTime = 3.0f;
    [SerializeField] private float bgmFadeOutTime = 2.0f;

    string _propName = "_Threshold";
    Material _mat;
    float _grayValue = 0;
    bool _isTrigger = false;
    bool isTransitioning = false;


    void Start()
    {
        _mat = _image.material;
        AudioManager.Instance.PlayBGMWithFade("Title_adventurers", bgmFadeInTime);
    }

    void SceneLoad()
    {
        SceneManager.LoadScene("Debug");
        _isTrigger = false;
    }
    void Update()
    {
        var gamepad = Gamepad.current;
        //　ゲームパッドが接続されていなければこれ以降
        if (gamepad != null)
        {
            if (gamepad.buttonEast.isPressed && !_isTrigger)
            {
                _mat.SetFloat(_propName, 0);
                _grayValue = 0;
                _isTrigger = true;

                AudioManager.Instance.PlaySE("Click 7");
            }
        }
        else
        {
            if (Keyboard.current.spaceKey.isPressed && !_isTrigger)
            {
                _mat.SetFloat(_propName, 0);
                _grayValue = 0;
                _isTrigger = true;

                AudioManager.Instance.PlaySE("Click 7");
            }
        }
        
        if (_isTrigger)
        {
            _grayValue += Time.deltaTime * _graySpeed;
            _grayValue = Mathf.Clamp01(_grayValue);
            _mat.SetFloat(_propName, _grayValue);
            //if (_grayValue == 1) { FadeController.Instance.FadeOut(() => SceneLoad()); }
            if (_grayValue == 1 && !isTransitioning)
            {
                isTransitioning = true;

                AudioManager.Instance.StopCurrentBGMWithFade(bgmFadeOutTime);
                SceneTransitionManager.Instance.StartTransitionByName(SceneTransitionManager.OpeningSceneName);
            }
        }
    }
}
