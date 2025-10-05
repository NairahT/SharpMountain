using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public CardType Type;
    public Sprite Image;
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