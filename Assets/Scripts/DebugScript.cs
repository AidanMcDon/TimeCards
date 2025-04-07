using UnityEngine;

public class DebugScript : MonoBehaviour
{

    private CardContainer cardContainer;
    private DeckManager deckManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardContainer = FindFirstObjectByType<CardContainer>();
        deckManager = FindFirstObjectByType<DeckManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            deckManager.DrawCard();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            deckManager.ResetDeck();
        }
    }
}
