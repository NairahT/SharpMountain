using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string savePath => Application.persistentDataPath + "/savegame.json";
    
    public void SaveGame(ScoreManager scoreManager, List<Card> cards)
    {
        SaveData data = new SaveData
        {
            score = scoreManager.Score,
            pairsCount = cards.Count / 2,
            cardTypes = new CardType[cards.Count],
            cardStates = new CardState[cards.Count]
        };
        
        for (int i = 0; i < cards.Count; i++)
        {
            data.cardTypes[i] = cards[i].CardType;
            data.cardStates[i] = cards[i].CurrentState;
        }
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved!");
    }
    
    public SaveData LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found");
            return null;
        }
        
        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log("Game loaded!");
        return data;
    }
}

[System.Serializable]
public class SaveData
{
    public int score;
    public int pairsCount;
    public CardType[] cardTypes; 
    public CardState[] cardStates;
}