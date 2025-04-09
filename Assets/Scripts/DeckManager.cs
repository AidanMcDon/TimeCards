using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<Card> playerCards = new List<Card>();
    private Queue<Card> deck = new Queue<Card>();
    private Queue<Card> discard = new Queue<Card>();
    public List<CardObject> hand = new List<CardObject>();
    private CardContainer cardContainer;


    [SerializeField]private GameObject cardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        cardContainer = GameObject.FindFirstObjectByType<CardContainer>();
        if (cardContainer == null)
        {
            Debug.LogError("CardContainer not found in the scene. Please add a CardContainer to the scene.");
        }
    }
    void Start()
    {
        foreach (Card card in playerCards)
        {
            deck.Enqueue(card);
        }
        ShuffleDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Resets the deck by clearing the current deck and discard piles, and re-adding all cards to the deck and shuffling.
    /// </summary>
    public void ResetDeck(){
        deck.Clear();
        discard.Clear();
        foreach(CardObject obj in hand){
            Destroy(obj.gameObject);
        }
        hand.Clear();
        foreach (Card card in playerCards){
            deck.Enqueue(card);
            ShuffleDeck();
        }
    }

    public void ShuffleDeck(){
        deck = deck.Shuffle();
    }

    public void DrawCard(){
        if (deck.Count > 0){
            Card drawnCard = deck.Dequeue();
            hand.Add(CreateCardObject(drawnCard)); //this step creates a new card object on screen
            cardContainer.OnDraw();
        }
        else{
            ReshuffleDeck();
            DrawCard();
        }
    }

    public void DiscardHand(){
        foreach (CardObject card in hand){
            discard.Enqueue(card.card);
            Destroy(card.gameObject);
        }
        hand.Clear();
    }

    public void DiscardCard(CardObject card){
        discard.Enqueue(card.card);
        hand.Remove(card);
        Destroy(card.gameObject);
        cardContainer.OnDiscard();
    }

    public void DrawHand(int amount){
        for (int i = 0; i < amount; i++){
            DrawCard();
        }
    }

    void ReshuffleDeck(){
        foreach (Card card in discard){
            deck.Enqueue(card);
        }
        discard.Clear();
        ShuffleDeck();
    }

    public CardObject CreateCardObject(Card card){
        GameObject cardObject = Instantiate(cardPrefab, transform.position, Quaternion.identity);
        CardObject cardObj = cardObject.GetComponent<CardObject>();
        if (cardObj != null){
            cardObj.Build(card);
            return cardObj;
        }
        else{
            Debug.LogError("CardObject component not found on prefab: " + cardPrefab.name);
            return null;
        }
    }
}
