using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerLoader : MonoBehaviour
{
    private void Awake()
    {
        if (!SceneManager.GetSceneByName("Hub").isLoaded)
            SceneManager.LoadScene("ManagerScene", LoadSceneMode.Additive);
    }
}
