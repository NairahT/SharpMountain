using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private Transform cardContainerParent;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<CardData> availableCardTypes;
    [SerializeField] private int amountOfPairsToSpawn = 6;
    
    private readonly List<Card> _spawnedCards = new List<Card>();
    
    public List<Card> ShuffleAndSpawnCards()
    {
        if (amountOfPairsToSpawn > availableCardTypes.Count)
        {
            Debug.LogError($"Not enough card types");
            return null;
        }
        
        ClearCards();
        
        var cardDataList = new List<CardData>();
        for (var i = 0; i < amountOfPairsToSpawn; i++)
        {
            cardDataList.Add(availableCardTypes[i]);
            cardDataList.Add(availableCardTypes[i]); 
        }
        
        ShuffleCards(cardDataList);
        
        
        foreach (var cardData in cardDataList)
        {
            var cardObj = Instantiate(cardPrefab, cardContainerParent);
            var card = cardObj.GetComponent<Card>();
            
            if (card != null)
            {
                 card.SetCardData(cardData);
                _spawnedCards.Add(card);
            }
        }
        
        Debug.Log($"Spawned {_spawnedCards.Count} cards ({amountOfPairsToSpawn} pairs)");
        return _spawnedCards;
    }
    
    private void ShuffleCards(List<CardData> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var randomIndex = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
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
}
