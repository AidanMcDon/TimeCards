using System.Collections.Generic;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    [SerializeField] private float handAreaLength = 10f, handHeight = -3f;
    private DeckManager deckManager;

    private int closestCardIndex, previousClosestCardIndex;
    private LevelManager levelManager;
    void Awake()
    {
        deckManager = FindFirstObjectByType<DeckManager>();
        if (deckManager == null)
        {
            Debug.LogError("DeckManager not found in the scene. Please add a DeckManager to the scene.");
        }
    }

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelManager not found in the scene. Please add a LevelManager to the scene.");
        }
    }

    void Update()
    {
        MoveCardsToPositions();
        PeekClosestCard();
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
                cardObject.transform.localPosition = new Vector3(cardObject.transform.localPosition.x, cardObject.transform.localPosition.y, -cardObject.transform.localPosition.x/100f - cardObject.transform.localPosition.y/20f);
            } else {
                Debug.LogError("CardObject component not found on card: " + deckManager.hand[i].name);
            }
        }
    }


    /// <summary>
    /// Sets the positions of the cards in the hand area based on the number of cards and the available space.
    /// The cards should move to the locations over time, not instantly.
    /// </summary>
    void SetPositions(){
        positions.Clear();
        int cardCount = deckManager.hand.Count;
        closestCardIndex = -1;
        previousClosestCardIndex = -1;
        
        if (cardCount > 0) {
            // Calculate spacing based on available width
            float spacing = handAreaLength / Mathf.Max(1, cardCount - 1);
            // If only one card, center it
            if (cardCount == 1) {
                positions.Add(new Vector2(0, handHeight));
            } else {
                // Starting x position (left edge of hand area)
                float startX = -handAreaLength / 2f;
                
                for (int i = 0; i < cardCount; i++) {
                    float x = startX + (spacing * i);
                    float y = handHeight;
                    positions.Add(new Vector2(x, y));
                }
            }
        }
    }

    /// <summary>
    /// Finds the closest card to the mouse position and updates its position.
    /// If the mouse is above the hand area, reset the closest card index.
    /// </summary>
    /// 
    private bool onMobile = true;
    void PeekClosestCard(){
        if(onMobile) return;
        float handAreaCutoff = -1f;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.y > handAreaCutoff){
            closestCardIndex = -1;
            previousClosestCardIndex = -1;
            SetPositions();
            return;
        }
        closestCardIndex = -1;
        float closestDistance = Mathf.Infinity;
        for(int i = 0; i < positions.Count; i++){
            float distance = Vector2.Distance(mousePos, positions[i]);
            if(distance < closestDistance){
                closestDistance = distance;
                closestCardIndex = i;
            }
        }

        if(closestCardIndex != previousClosestCardIndex){
            SetPositions();
            positions[closestCardIndex] = new Vector2(positions[closestCardIndex].x, positions[closestCardIndex].y + 1f);
            previousClosestCardIndex = closestCardIndex;
        }
    }

    public void ClickCard(Vector2 clickPosition){


        //raycast section to find which card was casted at
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(clickPosition, Vector2.zero);
        if(hits.Length == 0){
            SetPositions();
            return;
        }
        CardObject cardObject = null;
        float closestZ = Mathf.Infinity;
        foreach(RaycastHit2D hit in hits){
            if(hit.collider != null && hit.collider.gameObject.GetComponent<CardObject>() != null){
                if(hit.collider.transform.position.z < closestZ){
                    closestZ = hit.collider.transform.position.z;
                    cardObject = hit.collider.gameObject.GetComponent<CardObject>();
                }
            }
        }

        if(cardObject == null){
            SetPositions();
            return;
        }





        int closestCardIndex = GetIndexOfCard(cardObject);
        if(closestCardIndex == -1) return;
        if(closestCardIndex != previousClosestCardIndex){
            //raise the card so the player can see it better
            SetPositions();
            positions[closestCardIndex] = new Vector2(positions[closestCardIndex].x, positions[closestCardIndex].y + 1f);
            previousClosestCardIndex = closestCardIndex;
        }else{
            //the card has been activated
            
            ActivateCard(closestCardIndex);
        }
    }
    private int GetIndexOfCard(CardObject card){
        for(int i = 0; i < deckManager.hand.Count; i++){
            if(deckManager.hand[i].gameObject == card.gameObject) return i;
        }
        return -1;
    }

    public int GetClosestCardIndex(Vector2 clickPosition){
        float closestDistance = Mathf.Infinity;
        int closestCardIndex = -1;
        for(int i = 0; i < positions.Count; i++){
            if(Vector2.Distance(clickPosition, positions[i]) < closestDistance){
                closestDistance = Vector2.Distance(clickPosition, positions[i]);
                closestCardIndex = i;
            }
        }
        return closestCardIndex;
    }

    private void ActivateCard(int cardIndex){
        if(cardIndex < 0 || cardIndex >= deckManager.hand.Count) return;
        CardObject cardObject = deckManager.hand[cardIndex].GetComponent<CardObject>();
        if(cardObject != null){
            //activate the card
            Debug.Log("Activating card: " + cardObject.card.cardName);
            if(levelManager.ActivateCard(cardObject.card)){

                deckManager.DiscardCard(cardObject);
            }
        }else{
            Debug.LogError("CardObject component not found on card: " + deckManager.hand[cardIndex].name);
        }
    }

    //to be called from deckmanager when resetting hand and deck
    public void OnDraw(){
        SetPositions();
    }

    public void OnDiscard(){
        SetPositions();
    }
    public void OnShuffle(){
        SetPositions();
    }
}
