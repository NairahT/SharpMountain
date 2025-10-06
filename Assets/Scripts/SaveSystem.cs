using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string savePath => Application.persistentDataPath + "/savegame.json";
    
    public void SaveGame(ScoreManager scoreManager, List<Card> cards)
    {
        var data = new SaveData
        {
            score = scoreManager.Score,
            pairsCount = cards.Count / 2,
            cardTypes = new CardType[cards.Count],
            cardIsMatched = new bool[cards.Count]
        };
        
        for (int i = 0; i < cards.Count; i++)
        {
            data.cardTypes[i] = cards[i].CardType;
            data.cardIsMatched[i] = cards[i].CurrentState == CardState.Matched;
        }
        
        var json = JsonUtility.ToJson(data, true);
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
        
        var json = File.ReadAllText(savePath);
        var data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log("Game loaded!");
        return data;
    }
    
    public void DeleteSave()
    {
        if (!File.Exists(savePath)) return;
        File.Delete(savePath);
        Debug.Log("Save deleted");
    }
}

[System.Serializable]
public class SaveData
{
    public int score;
    public int pairsCount;
    public CardType[] cardTypes; 
    public bool[] cardIsMatched; 
}