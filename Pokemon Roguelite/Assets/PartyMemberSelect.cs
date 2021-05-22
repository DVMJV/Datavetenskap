using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberSelect : MonoBehaviour
{
    [SerializeField] PokemonContainer slottedMember;
    [SerializeField] Sprite squareFrame;
    [SerializeField] Sprite circularFrame;

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
            frameComp.rectTransform.sizeDelta = new Vector2(125, 125);
            portraitComp.sprite = circularFrame;

            isActive = true;
        }
        else
        {
            EventHandler.current.RemovePokemon(slottedMember);

            frameComp.sprite = squareFrame;
            frameComp.rectTransform.sizeDelta = new Vector2(175, 175);
            portraitComp.sprite = slottedMember.pokemon.sprite;

            isActive = false;
        }
       
        Debug.Log("Button Clicked");
    }
}
