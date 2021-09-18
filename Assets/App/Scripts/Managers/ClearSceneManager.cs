using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSceneManager : BaseSceneManager<ClearSceneManager>
{

    private bool isBackedTitle = false;

    public void BackTitle()
    {
        if (isBackedTitle) return;

        SceneTransitionManager.Instance.StartTransitionByName(SceneTransitionManager.TitleSceneName);

        isBackedTitle = true;
    }
}
