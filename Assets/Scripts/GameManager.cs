using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    [SerializeField] private List<Card> selectedCards;

    private bool _isCheckingForMatch = false;
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
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        var cardOne = selectedCards[0];
        var cardTwo = selectedCards[1];

        if (cardOne.CardType == cardTwo.CardType)
        {
            Debug.Log($"found a match of type {cardOne.CardType}");
            cardOne.FoundMatch();
            cardTwo.FoundMatch();
        }
        else
        {
            Debug.Log($"No match found between {cardOne.CardType} and {cardTwo.CardType}");
            yield return new WaitForSeconds(1.5f);
            
            cardOne.FlipFaceDown();
            cardTwo.FlipFaceDown();
        }
        selectedCards.Clear();
    }
}
