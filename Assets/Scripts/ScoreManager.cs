using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<int> OnScoreChanged;
    private int _currentScore = 0;
    
    public void UpdateScore()
    {
        _currentScore += 10;
        OnScoreChanged?.Invoke(_currentScore);
    }
}
