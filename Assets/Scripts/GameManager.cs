using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    [SerializeField] private ScoreManager scoreManager;
    
    private Card _firstSelectedCard;
    private Card _secondSelectedCard;

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
                _firstSelectedCard = card;
                _cardCounter++;
                break;
            case 1:
                _secondSelectedCard = card;
                _cardCounter = 0;
                CheckMatch();
                break;
        }
    }

    private void CheckMatch()
    {
        if (_firstSelectedCard.CardType == _secondSelectedCard.CardType)
        {
            Debug.Log($"Found a match of type {_firstSelectedCard.CardType}");
            AudioManager.Instance.PlayMatch();
            _firstSelectedCard.FoundMatch();
            _secondSelectedCard.FoundMatch();
            scoreManager.UpdateScore();
        }
        else
        {
            Debug.Log($"No match found between {_firstSelectedCard.CardType} and {_secondSelectedCard.CardType}");
            AudioManager.Instance.PlayMismatch();
            _firstSelectedCard.FlipFaceDown();
            _secondSelectedCard.FlipFaceDown();
        }
        _firstSelectedCard = null;
        _secondSelectedCard = null;
    }

    private void OnDisable()
    {
        foreach (var card in cards)
        {
            card.OnCardSelected -= HandleSelectedCard;
        }
    }
}
