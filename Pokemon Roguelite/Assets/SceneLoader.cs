using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(int sceneToUnloadID, int sceneToLoadID)
    {
        SceneManager.LoadSceneAsync(sceneToLoadID, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(sceneToUnloadID);
    }
}
