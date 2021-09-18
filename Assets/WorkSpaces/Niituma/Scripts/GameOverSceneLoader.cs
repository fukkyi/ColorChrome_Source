using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverSceneLoader : MonoBehaviour
{
    [SerializeField] private Texture fadeTex;
    bool _isTrigger = false;
    public void GameOver()
    {
        GS_Parameter.FadeTextureChange(FadeController.Instance.GetFadeMat, fadeTex, Color.black);
    }

    void SceneLoad()
    {
        SceneManager.LoadScene("GameOverScene");
        _isTrigger = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed && !_isTrigger)
        {
            GameOver();
            FadeController.Instance.FadeOut(() => SceneLoad());
        }
    }
}
