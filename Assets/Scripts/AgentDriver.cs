using UnityEngine;

[RequireComponent(typeof(Agent))]
public class AgentDriver : MonoBehaviour
{
    public enum DrivingState{
        Idle,
        Moving
    }
    public DrivingState drivingState = DrivingState.Idle;
    public float speed = 5f;
    private LevelManager levelManager;


    public void Start()
    {
        levelManager = GameObject.FindFirstObjectByType<LevelManager>();
        if(levelManager == null){
            Debug.LogError("LevelManager not found in the scene.");
        }
    }
}