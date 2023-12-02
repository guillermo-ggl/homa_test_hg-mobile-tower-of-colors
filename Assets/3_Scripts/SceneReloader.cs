using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public bool AllowChange = false;

    IEnumerator Start()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("GameScene");
        loadOp.allowSceneActivation = false;
        while(!AllowChange || loadOp.progress < 0.9f)
            yield return null;
        loadOp.allowSceneActivation = true;
    }
}
