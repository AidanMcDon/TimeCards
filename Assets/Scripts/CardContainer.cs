using System.Collections.Generic;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    [SerializeField] private float handAreaLength = 10f;
    private DeckManager deckManager;

    void Awake()
    {
        deckManager = FindFirstObjectByType<DeckManager>();
        if (deckManager == null)
        {
            Debug.LogError("DeckManager not found in the scene. Please add a DeckManager to the scene.");
        }
    }

    void Update()
    {
        MoveCardsToPositions();
    }

    List<Vector2> positions = new List<Vector2>();


    /// <summary>
    /// Sets the positions of the cards in the hand area based on the number of cards and the available space.
    /// the cards should move to the locations over time not instantly
    /// </summary>
    void MoveCardsToPositions(){
        for (int i = 0; i < deckManager.hand.Count; i++){
            if(deckManager.hand[i] == null) return;
            CardObject cardObject = deckManager.hand[i].GetComponent<CardObject>();
            if (cardObject != null){
                if(Vector2.Distance(cardObject.transform.localPosition, positions[i]) > 0.01f){
                    cardObject.transform.localPosition = Vector2.Lerp(cardObject.transform.localPosition, positions[i], Time.unscaledDeltaTime * 5f);
                }else{
                    cardObject.transform.localPosition = positions[i];
                }
                //set the z position to help with proper ordering of cards
                cardObject.transform.localPosition = new Vector3(cardObject.transform.localPosition.x, cardObject.transform.localPosition.y, -cardObject.transform.localPosition.x/10f);
            } else {
                Debug.LogError("CardObject component not found on card: " + deckManager.hand[i].name);
            }
        }
    }
    void SetPositions(){
        positions.Clear();
        int cardCount = deckManager.hand.Count;
        
        if (cardCount > 0) {
            // Calculate spacing based on available width
            float spacing = handAreaLength / Mathf.Max(1, cardCount - 1);
            // If only one card, center it
            if (cardCount == 1) {
                positions.Add(new Vector2(0, -3f));
            } else {
                // Starting x position (left edge of hand area)
                float startX = -handAreaLength / 2f;
                
                for (int i = 0; i < cardCount; i++) {
                    float x = startX + (spacing * i);
                    float y = -3f;
                    positions.Add(new Vector2(x, y));
                }
            }
        }
    }

    //to be called from deckmanager when resetting hand and deck


    public void OnDraw(){
        SetPositions();
    }
}
