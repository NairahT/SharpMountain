using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float revealDuration = 5f;
    [SerializeField] private SaveSystem saveSystem;
    
    private List<Card> _allCards = new List<Card>();
    private Card _firstSelectedCard;
    private Card _secondSelectedCard;
    private int _cardCounter = 0;
    private bool _acceptPlayerInput = false;

    private void Start()
    {
        SaveData saveData = saveSystem.LoadGame();

        if (saveData != null)
        {
            LoadFromSave(saveData);
        }
        else
        {
            InitializeNewGame();
        }
    }
    
    private void LoadFromSave(SaveData data)
    {
        cardSpawner.SetPairsToSpawn(data.pairsCount);
        _allCards = cardSpawner.SpawnCardsFromSave(data.cardTypes);
        
        if (_allCards == null || _allCards.Count == 0)
        {
            Debug.LogError("Failed to spawn cards!");
            return;
        }
        
        for (int i = 0; i < _allCards.Count; i++)
        {
            if (data.cardIsMatched[i])
            {
                _allCards[i].SetCardState(CardState.Matched);
            }
        }
        
        scoreManager.SetScore(data.score);
        SubscribeToCards();
        _acceptPlayerInput = true;
        Debug.Log("Game restored from save");
    }
    
    
    private void InitializeNewGame()
    {
        _allCards = cardSpawner.ShuffleAndSpawnCards();
        if (_allCards == null || _allCards.Count == 0)
        {
            Debug.LogError("Failed to spawn cards!");
            return;
        }

        SubscribeToCards();
        StartCoroutine(InitialRevealSequence());
    }
    
    private IEnumerator InitialRevealSequence()
    {
        _acceptPlayerInput = false;
        foreach (var card in _allCards)
        {
            card.FlipFaceUp();
        }
        
        yield return new WaitForSeconds(revealDuration);
        
        foreach (var card in _allCards)
        {
            card.FlipFaceDown();
        }
        
        yield return new WaitForSeconds(0.6f);
        _acceptPlayerInput = true;
    }

    private void SubscribeToCards()
    {
        foreach (var card in _allCards)
        {
            card.OnCardSelected += HandleSelectedCard;
        }
    }

    private void HandleSelectedCard(Card card)
    {
        if (!_acceptPlayerInput) return;
        
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
        foreach (var card in _allCards)
        {
            card.OnCardSelected -= HandleSelectedCard;
        }
    }
    
    private void OnApplicationQuit()
    {
        saveSystem.SaveGame(scoreManager, _allCards);
    }
}
