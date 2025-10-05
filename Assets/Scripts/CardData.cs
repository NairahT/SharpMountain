using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public CardType type;
    public Color cardColor;
}

public enum CardType
{
    A,
    B,
    C
}