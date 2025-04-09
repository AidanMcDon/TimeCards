using UnityEngine;


public enum CardType{
    Unit,
    Spell,
    Augment
}

public enum AttackType{
    Projectile,
    HitScan,
    Magic
}

[System.Serializable]
public class CardStats
{
    public int health;
    public int attack;
    public int defense;
    public int speed;
    public AttackType attackType;
    public int range;
}

[System.Serializable]
public class Visuals
{
    public Sprite cardImage;
    public Color unitColor;
    public Sprite unitSprite;
}

public class Card : ScriptableObject
{
    public CardType cardType;
    public string cardName;
    public string description;
    public int cost;
}

public class AugmentStats{
    
}


[CreateAssetMenu(fileName = "UnitCard", menuName = "Scriptable Objects/UnitCard")]
public class UnitCard : Card
{
    private void OnValidate()
    {
        cardType = CardType.Unit;    
    }
    public CardStats stats = new CardStats();
    public Visuals visuals = new Visuals();
}