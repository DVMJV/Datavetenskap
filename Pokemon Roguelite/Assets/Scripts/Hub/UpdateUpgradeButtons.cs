using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUpgradeButtons : MonoBehaviour
{
    [SerializeField] GameObject upgradeButtonPrefab;

    Pokemon slottedPokemonTemplate; //Should probably be the Container after future implementation.
    PokemonContainer slottedPokemon;

    List<Button> upgradeButtons = new List<Button>();

    void Start()
    {
        EventHandler.current.onUpgradeSlotFilled += SetUpButtons;
        EventHandler.current.onUpgradeSlotEmpty += EmptyUpgradeList;
    }

    void SetUpButtons(PokemonContainer pokemonContainer)
    {
        slottedPokemon = pokemonContainer;
        slottedPokemonTemplate = pokemonContainer.pokemon;

        int totalMoves = slottedPokemonTemplate.attackToLearn.Count;

        int i;
        for (i = 0; i < totalMoves; i++)
        {
            if (upgradeButtons.Count <= i)
            {
                GameObject go = Instantiate(upgradeButtonPrefab, transform);
                upgradeButtons.Add(go.GetComponent<Button>());
            }

            if (slottedPokemon.learnedMoves.Contains(new PokemonAttackContainer(slottedPokemonTemplate.attackToLearn[i], pokemonContainer)))
            {
                upgradeButtons[i].gameObject.SetActive(false);
                continue;
            }

            upgradeButtons[i].GetComponent<UpgradeSlot>().SetUpUpgradeSlot(slottedPokemon, slottedPokemonTemplate.attackToLearn[i]);

            upgradeButtons[i].gameObject.SetActive(true);
            upgradeButtons[i].interactable = true;

            if (slottedPokemon.currentLevel < slottedPokemonTemplate.levelToLearnAt[i])
                upgradeButtons[i].interactable = false;
        }

        for (; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(false); //Could use Destroy in the future
        }
    }

    public void EmptyUpgradeList()
    {
        foreach(Button b in upgradeButtons)
        {
            b.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        EventHandler.current.onUpgradeSlotFilled -= SetUpButtons;
        EventHandler.current.onUpgradeSlotEmpty -= EmptyUpgradeList;
    }
}
