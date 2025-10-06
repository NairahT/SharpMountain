using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private float revealDuration = 5f;
    
    private List<Card> _allCards = new List<Card>();
    private Card _firstSelectedCard;
    private Card _secondSelectedCard;
    
    private int _cardCounter = 0;
    private bool _acceptPlayerInput = false;
    private bool _didWin = false;
    
    private void Start()
    {
        var saveData = saveSystem.LoadGame();
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
        if(!ValidateCards(_allCards)) return;
        
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
        if(!ValidateCards(_allCards)) return;
        
        SubscribeToCards();
        StartCoroutine(InitialRevealSequence());
        Debug.Log("New game initialized");
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

            CheckIfGameIsWon();
        }
        else
        {
            AudioManager.Instance.PlayMismatch();
            _firstSelectedCard.FlipFaceDown();
            _secondSelectedCard.FlipFaceDown();
        }
        _firstSelectedCard = null;
        _secondSelectedCard = null;
    }

    private void CheckIfGameIsWon()
    {
        foreach (var card in _allCards)
        {
            if (card.CurrentState != CardState.Matched) return;
        }
        
        Debug.Log("You won!");
        _didWin = true;
        AudioManager.Instance.PlayWinGame();
        saveSystem.DeleteSave();
        _acceptPlayerInput = false;
    }

    private bool ValidateCards(List<Card> cards)
    {
        if (cards != null && cards.Count != 0) return true;
        Debug.LogError("Failed to spawn cards!");
        return false;
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
        if(!_didWin) saveSystem.SaveGame(scoreManager, _allCards);
    }
}
