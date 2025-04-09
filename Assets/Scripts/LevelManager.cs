using System.Collections;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameStateText;
    public Transform[] zonePositions;
    private Card heldCard;
    private GameState gameState = GameState.Paused;
    private CardPlayer cardPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardPlayer = gameObject.GetComponent<CardPlayer>();
        if(cardPlayer == null){
            Debug.LogError("CardPlayer not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == GameState.PlayingCard){
            //check if the player is in a zone and if so, place the card
            if(heldCard != null){
                PlacingCard();
            }else{
                gameState = GameState.Paused;
            }
        }
        gameStateText.text = gameState.ToString();
    }

    enum GameState
    {
        Paused,
        Running,
        PlayingCard,
        Shop
    }

    public bool ActivateCard(Card card){
        //if the game is running then cards cannot be played
        if (gameState != GameState.Paused)
        {
            return false;
        }
        if (card == null)
        {
            return false;
        }

        ShowZones(true);
        heldCard = card;
        gameState = GameState.PlayingCard;

        return true;
    }

    void PlacingCard(){
        if(Input.touchCount > 0){
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            for(int i = 0; i < zonePositions.Length; i++){
                Vector2 zonePos = zonePositions[i].position;
                if(CheckIfInBounds(touchPosition, zonePositions[i])){
                    //check if the card is a unit card and if so, place it in the zone
                    if(heldCard is UnitCard unitCard){
                        cardPlayer.PlayUnit(unitCard, zonePositions[i]);
                        RunGame(heldCard.cost);
                        heldCard = null;
                        ShowZones(false);
                    }
                    else{
                        //if the card is not a unit card then do nothing
                        Debug.LogError("Card is not a UnitCard");
                    }
                }
            }
        }
    }

    private void RunGame(float time){
        StartCoroutine(PlayGame(time));
    }

    bool CheckIfInBounds(Vector2 touchPos, Transform zone){
        //check if the touch position is within the bounds of the zone
        Vector2 zonePos = zone.position;
        Vector2 zoneSize = zone.localScale;
        if(touchPos.x > zonePos.x - zoneSize.x/2 && touchPos.x < zonePos.x + zoneSize.x/2 && touchPos.y > zonePos.y - zoneSize.y/2 && touchPos.y < zonePos.y + zoneSize.y/2){
            return true;
        }
        return false;
    }

    IEnumerator PlayGame(float time){
        gameState = GameState.Running;
        TimeManager.Unpause();
        yield return new WaitForSeconds(time);
        TimeManager.Pause();
        gameState = GameState.Paused;
    }

    void ShowZones(bool show){
        for(int i = 0; i < zonePositions.Length; i++){
            zonePositions[i].gameObject.SetActive(show);
        }
    }



    
}
