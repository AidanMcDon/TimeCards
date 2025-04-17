using Unity.Mathematics;
using UnityEngine;

public class CardPlayer : MonoBehaviour
{
    [SerializeField] private GameObject vehiclePrefab;
    
    public void PlayAugment(Card card){

    }

    public void PlayUnit(UnitCard card, Transform zone, string team = "Player"){
        GameObject vehicle = Instantiate(vehiclePrefab, zone.position * 2,quaternion.identity);
        if(vehicle.GetComponent<Agent>() == null || vehicle.GetComponent<AgentDriver>() == null){
            vehicle.AddComponent<Agent>();
            vehicle.AddComponent<AgentDriver>();
        }
        vehicle.GetComponent<Agent>().card = card;
        vehicle.GetComponent<Agent>().team = team;
        vehicle.GetComponent<AgentDriver>().zone = zone;
        if(card.visuals.unitSprite != null){
            vehicle.GetComponent<SpriteRenderer>().sprite = card.visuals.unitSprite;
        }else{
            vehicle.GetComponent<SpriteRenderer>().color = card.visuals.unitColor;
        }
    }
}