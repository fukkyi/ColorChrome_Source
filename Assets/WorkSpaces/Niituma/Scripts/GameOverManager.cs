using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameOverManager : MonoBehaviour
{
    bool _isTrigger = false;

    void SceneLoad()
    {
        SceneManager.LoadScene("TitleScene");
        _isTrigger = false;
    }

    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.buttonEast.isPressed && !_isTrigger)
            {
                FadeController.Instance.FadeOut(() => SceneLoad());
            }
        }
        else
        {
            if (Keyboard.current.spaceKey.isPressed && !_isTrigger)
            {
                FadeController.Instance.FadeOut(() => SceneLoad());
            }
        }
    }
}
