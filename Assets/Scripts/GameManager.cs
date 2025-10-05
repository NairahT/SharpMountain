using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    [SerializeField] private List<Card> selectedCards;
    [SerializeField] private Card firstSelectedCard;
    [SerializeField] private Card secondSelectedCard;

    private int _cardCounter = 0;
    
    private void Start()
    {
        foreach (var card in cards)
        {
            card.OnCardSelected += HandleSelectedCard;
        }
    }

    private void HandleSelectedCard(Card card)
    {
        switch (_cardCounter)
        {
            case 0:
                firstSelectedCard = card;
                _cardCounter++;
                break;
            case 1:
                secondSelectedCard = card;
                _cardCounter = 0;
                CheckMatch();
                break;
        }
    }

    private void CheckMatch()
    {
        if (firstSelectedCard.CardType == secondSelectedCard.CardType)
        {
            Debug.Log($"found a match of type {firstSelectedCard.CardType}");
            firstSelectedCard.FoundMatch();
            secondSelectedCard.FoundMatch();
        }
        else
        {
            Debug.Log($"No match found between {firstSelectedCard.CardType} and {secondSelectedCard.CardType}");
            
            firstSelectedCard.FlipFaceDown();
            secondSelectedCard.FlipFaceDown();
        }
        firstSelectedCard = null;
        secondSelectedCard = null;
    }

    private void OnDisable()
    {
        foreach (var card in cards)
        {
            card.OnCardSelected -= HandleSelectedCard;
        }
    }
}
