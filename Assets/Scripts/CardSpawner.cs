using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private Transform cardContainerParent;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<CardData> availableCardTypes;
    [SerializeField] private int amountOfPairsToSpawn = 6;
    [SerializeField] private GridLayoutConfig gridConfig;
    [SerializeField] private int gridRows = 4;
    [SerializeField] private int gridColumns = 3;
    
    private readonly List<Card> _spawnedCards = new List<Card>();
    
    public List<Card> ShuffleAndSpawnCards()
    {
        if (amountOfPairsToSpawn > availableCardTypes.Count)
        {
            Debug.LogError($"Not enough card types");
            return null;
        }
        
        var cardDataList = new List<CardData>();
        for (var i = 0; i < amountOfPairsToSpawn; i++)
        {
            cardDataList.Add(availableCardTypes[i]);
            cardDataList.Add(availableCardTypes[i]); 
        }
        ShuffleCards(cardDataList);
        return SpawnCards(cardDataList);
    }

    public List<Card> SpawnCardsFromSave(CardType[] savedCardTypes)
    {
        var cardDataList = new List<CardData>();
        foreach (var cardType in savedCardTypes)
        {
            var matchingData = availableCardTypes.Find(data => data.Type == cardType);
            cardDataList.Add(matchingData);
        }
        return SpawnCards(cardDataList);
    }
    private List<Card> SpawnCards(List<CardData> cardDataList)
    {
        if (!IsCardConfigurationValid()) return null;
    
        ClearCards();
        gridConfig.ConfigureGrid(gridRows, gridColumns);
    
        foreach (var cardData in cardDataList)
        {
            SpawnCardWithData(cardData);
        }
    
        Debug.Log($"Spawned {_spawnedCards.Count} cards");
        return _spawnedCards;
    }
    
    private void SpawnCardWithData(CardData cardData)
    {
        var card = Instantiate(cardPrefab, cardContainerParent).GetComponent<Card>();
        card.SetCardData(cardData);
        _spawnedCards.Add(card);
    }
    
    private void ShuffleCards(List<CardData> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var randomIndex = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private bool IsCardConfigurationValid() 
    {
        var isConfigValid = (amountOfPairsToSpawn * 2) == (gridRows * gridColumns);
        if (!isConfigValid) Debug.LogError($"Game cannot spawn cards given {gridRows} rows and {gridColumns} columns. Please use a configuration that equals to {amountOfPairsToSpawn * 2}");
        return isConfigValid;
    }
    
    private void ClearCards()
    {
        foreach (var card in _spawnedCards)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }
        }
        _spawnedCards.Clear();
    }
    
    public void SetPairsToSpawn(int pairs) => amountOfPairsToSpawn = pairs;
}
