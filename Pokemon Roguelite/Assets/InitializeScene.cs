using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeScene : MonoBehaviour
{
    List<GameObject> pokemons = new List<GameObject>();

    void Start()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        //Debug.Log(SceneManager.GetSceneAt(1).name);

        InstantiatePokemons();
        EventHandler.current.CreatePlayerPokemons(pokemons);
    }
    //Start coroutine for scene done load check SceneManager.scene.isLoaded;

    void InstantiatePokemons()
    {
        for (int i = 0; i < PartyMemberManager.selectedParty.Count; i++)
        {
            GameObject go = Instantiate(PartyMemberManager.selectedParty[i]);
            go.tag = "Friendly";
            pokemons.Add(go);
        }
    }
}
