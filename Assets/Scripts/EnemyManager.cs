using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeManager;


[System.Serializable]
public class WaveConfig{
    public string waveName;
    public int easyEnemyCount;
    public int mediumEnemyCount;
    public int hardEnemyCount;
    public int bossEnemyCount;
    public float delayForNextWave = 5f;
    public float innerWaveDelay = 0.2f;
}
public class EnemyManager : MonoBehaviour
{

    [SerializeField] private float waveFrequency = 5f, elapsedTime = 0f;
    [SerializeField] private Card[] easyCards, mediumCards, hardCards, bossCards;
    [SerializeField] private List<WaveConfig> waves;
    private LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = GameObject.FindFirstObjectByType<LevelManager>();
        if(levelManager == null){
            Debug.LogError("LevelManager not found in the scene.");
        }
        StartCoroutine(SpawnWave(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnWave(int waveIndex){
        float elapsedTime = 0f;
        WaveConfig currentWave;
        if(waveIndex <= waves.Count -1){
            currentWave = waves[waveIndex];
        }else{
            currentWave = GenerateNextWave(waves[waveIndex - 1]);
            waves.Add(currentWave);
        }

        for (int i = 0; i < currentWave.easyEnemyCount; i++) {
            Debug.Log("Spawning Easy Enemy: " + i);
            SpawnEnemy(easyCards[Random.Range(0, easyCards.Length)]);
            yield return new WaitForGameSeconds(currentWave.innerWaveDelay);
        }
        
        for (int i = 0; i < currentWave.mediumEnemyCount; i++) {
            SpawnEnemy(mediumCards[Random.Range(0, mediumCards.Length)]);
            yield return new WaitForGameSeconds(currentWave.innerWaveDelay);
        }
        
        for (int i = 0; i < currentWave.hardEnemyCount; i++) {
            SpawnEnemy(hardCards[Random.Range(0, hardCards.Length)]);
            yield return new WaitForGameSeconds(currentWave.innerWaveDelay);
        }
        
        for (int i = 0; i < currentWave.bossEnemyCount; i++) {
            SpawnEnemy(bossCards[Random.Range(0, bossCards.Length)]);
            yield return new WaitForGameSeconds(currentWave.innerWaveDelay);
        }

        while(elapsedTime < currentWave.delayForNextWave){
            elapsedTime += TimeManager.DeltaTime;
            yield return null;
        }
        StartCoroutine(SpawnWave(waveIndex + 1));

    }

    private void SpawnEnemy(Card card){
        levelManager.ActivateEnemyCard(card);
    }

    private WaveConfig GenerateNextWave(WaveConfig previousWave){
        WaveConfig nextWave = new WaveConfig();
        nextWave.waveName = previousWave.waveName + " 2.0";
        nextWave.easyEnemyCount = previousWave.easyEnemyCount + Random.Range(1, 3);
        nextWave.mediumEnemyCount = previousWave.mediumEnemyCount;
        nextWave.hardEnemyCount = previousWave.hardEnemyCount;
        nextWave.bossEnemyCount = previousWave.bossEnemyCount;
        if(IncreaseStrength(nextWave.easyEnemyCount)){
            nextWave.easyEnemyCount -= 3;
            nextWave.mediumEnemyCount = previousWave.mediumEnemyCount + 1;
        }
        if(IncreaseStrength(nextWave.mediumEnemyCount)){
            nextWave.mediumEnemyCount -= 3;
            nextWave.hardEnemyCount = previousWave.hardEnemyCount + 1;
        }
        if(IncreaseStrength(nextWave.hardEnemyCount)){
            nextWave.hardEnemyCount -= 5;
            nextWave.bossEnemyCount = previousWave.bossEnemyCount + 1;
        }
        nextWave.delayForNextWave = previousWave.delayForNextWave;
        nextWave.innerWaveDelay = previousWave.innerWaveDelay;
        return nextWave;
    }
    private bool IncreaseStrength(int numberOfEnemyType){
        if(numberOfEnemyType > 3 && Random.Range(0,10-numberOfEnemyType) == 0){
            return true;
        }else{
            return false;
        }
    }
}
