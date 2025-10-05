using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public CardType type;
    public Sprite image;
}

public enum CardType
{
    Bear,
    Cat,
    Dog,
    Duck,
    Fox,
    Lion
}