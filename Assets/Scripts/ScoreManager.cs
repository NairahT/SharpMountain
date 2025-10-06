using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<int> OnScoreChanged;
    public int Score => _currentScore;
    
    private int _currentScore = 0;
    
    public void UpdateScore()
    {
        _currentScore += 10;
        OnScoreChanged?.Invoke(_currentScore);
    }
    
    public void SetScore(int score)
    {
        _currentScore = score;
        OnScoreChanged?.Invoke(_currentScore);
    }
}
