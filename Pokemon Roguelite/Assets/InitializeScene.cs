using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeScene : MonoBehaviour
{
    List<GameObject> pokemons = new List<GameObject>();

    void Start()
    {
        StartCoroutine(DelaySpawn());
    }

    IEnumerator DelaySpawn()
    {
        while (!SceneManager.SetActiveScene(SceneManager.GetSceneByName("TileMap_new")) && !SceneManager.GetSceneByName("Hub").isLoaded)
            yield return new WaitForSeconds(0.1f);
        
        InstantiatePokemons();
        EventHandler.current.CreatePlayerPokemons(pokemons);
    }
    //Start coroutine for scene done load check SceneManager.scene.isLoaded;

    void InstantiatePokemons()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        for (int i = 0; i < PartyMemberManager.selectedParty.Count; i++)
        {
            GameObject go = Instantiate(PartyMemberManager.selectedParty[i]);
            go.tag = "Friendly";
            pokemons.Add(go);
        }
    }
}
