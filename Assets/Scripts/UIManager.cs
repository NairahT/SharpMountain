using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private ScoreManager scoreManager;
    
    private void OnEnable() => scoreManager.OnScoreChanged += UpdateScoreUI;
    private void OnDisable() => scoreManager.OnScoreChanged -= UpdateScoreUI;
    private void UpdateScoreUI(int score) =>scoreText.text = score.ToString();
}
