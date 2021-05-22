using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberSelect : MonoBehaviour
{
    [SerializeField] PokemonContainer slottedMember;
    [SerializeField] Sprite frame;

    bool isActive = false;

    Button button;
    Image portraitComp;
    Image frameComp;

    void Start()
    {
        button = GetComponent<Button>();
        frameComp = transform.GetChild(0).GetComponent<Image>();
        portraitComp = GetComponent<Image>();

        button.image.sprite = slottedMember.pokemon.sprite;
    }

    public void UpdateSlot()
    {    
        if (!isActive)
        {
            if (PartyMemberManager.selectedParty.Count >= PartyMemberManager.maxPartySize)
                return;

            EventHandler.current.AddPokemon(slottedMember);

            frameComp.sprite = slottedMember.pokemon.sprite;
            frameComp.rectTransform.sizeDelta = new Vector2(145, 145);
            portraitComp.sprite = frame;

            isActive = true;
        }
        else
        {
            EventHandler.current.RemovePokemon(slottedMember);

            frameComp.sprite = frame;
            frameComp.rectTransform.sizeDelta = new Vector2(175, 175);
            portraitComp.sprite = slottedMember.pokemon.sprite;

            isActive = false;
        }
       
        Debug.Log("Button Clicked");
    }
}
