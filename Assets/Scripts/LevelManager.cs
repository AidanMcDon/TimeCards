using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Vector2[] zonePositions;
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
        return true;
    }
}
