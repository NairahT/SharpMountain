using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    [SerializeField] private List<Card> selectedCards;

    private void Start()
    {
        foreach (var card in cards)
        {
            card.OnCardSelected += HandleSelectedCard;
        }
    }

    private void HandleSelectedCard(Card card)
    {
        selectedCards.Add(card);
        if (selectedCards.Count == 2)
        {
            CheckMatch();
        }
    }

    private void CheckMatch()
    {
        var cardOne = selectedCards[0];
        var cardTwo = selectedCards[1];

        if (cardOne.CardType == cardTwo.CardType)
        {
            Debug.Log($"found a match of type {cardOne.CardType}");
        }
        else
        {
            Debug.Log($"No match found between {cardOne.CardType} and {cardTwo.CardType}");
        }
        selectedCards.Clear();
    }
}
