using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class AgentDriver : MonoBehaviour
{
    public enum DrivingState{
        Idle,
        Moving
    }
    public Transform zone;
    public DrivingState drivingState = DrivingState.Idle;
    public float speed = 5f;
    private LevelManager levelManager;
    private Vector2 targetIdlePosition;


    public void Start()
    {
        levelManager = GameObject.FindFirstObjectByType<LevelManager>();
        if(levelManager == null){
            Debug.LogError("LevelManager not found in the scene.");
        }
        StartCoroutine(IdleInZone());
    }

    void Update()
    {

    }



    public void ApplyZone(Transform zone){
        this.zone = zone;
        MoveToZone(zone);
    }

    public void MoveToZone(Transform zone){
        if(drivingState == DrivingState.Idle){
            drivingState = DrivingState.Moving;
            StartCoroutine(MoveToZoneCoroutine(zone));
        }
    }

    private IEnumerator MoveToZoneCoroutine(Transform zone){
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = GetRandomZonePosition();
        float distance = Vector3.Distance(startPosition, targetPosition);
        float time = 0f;
        while (time < distance/speed){
            transform.position = Vector3.Lerp(startPosition, targetPosition, time/(distance/speed));
            time += TimeManager.DeltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        drivingState = DrivingState.Idle;
    }

    private Vector2 GetRandomZonePosition(){
        Vector2 randomPosition = new Vector2(Random.Range(-zone.localScale.x/2, zone.localScale.x/2), Random.Range(-zone.localScale.y/2, zone.localScale.y/2));
        return randomPosition + (Vector2)zone.position;
    }

    IEnumerator IdleInZone(){
        StartCoroutine(_SetNewIdlePosition());
        targetIdlePosition = GetRandomZonePosition();
        while (drivingState == DrivingState.Idle){
            transform.position = Vector2.MoveTowards(transform.position, targetIdlePosition, speed * TimeManager.DeltaTime);
            yield return null;
        }
    }

    IEnumerator _SetNewIdlePosition(){
        while (drivingState == DrivingState.Idle){
            targetIdlePosition = GetRandomZonePosition();
            yield return new WaitForSeconds(Random.Range(1f,5f));
        }
    }


}