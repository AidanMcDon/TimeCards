using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Vector2[] zonePositions;
    private Card heldCard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    enum GameState
    {
        Paused,
        Running,
        PlayingCard,
        Shop
    }
    private GameState gameState = GameState.Paused;

    public bool ActivateCard(Card card){
        //if the game is running then cards cannot be played
        if (gameState != GameState.Running)
        {
            return false;
        }
        if (card == null)
        {
            return false;
        }

        return true;
    }



    
}
