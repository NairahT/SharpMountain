using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button cardButton;
    [SerializeField] private Image cardImage;
    [SerializeField] private CardType cardType;
    [SerializeField] private float flipDuration = 0.5f;
    public event Action<Card> OnCardSelected;
    public CardType CardType => cardType;
    
    private readonly Color _cardFrontColor = Color.black;
    private readonly Color _cardBackColor = Color.green;
    
    private bool _isFaceUp = false;
    private bool _isFlipping = false;

    private void OnEnable() => cardButton.onClick.AddListener(OnClickCard);
    private void OnDisable() => cardButton.onClick.RemoveListener(OnClickCard);

    public void OnClickCard()
    {
        FlipCard(true);
        OnCardSelected?.Invoke(this);
    }
    
    private void FlipCard(bool faceUp)
    {
        if (_isFlipping) return;
        if(_isFaceUp == faceUp) return;
        StartCoroutine(Flip(faceUp));
    }
    
    private IEnumerator Flip(bool faceUp) 
    {
        Debug.Log($"Flipping card face up: {faceUp}");

        _isFlipping = true;
        
        var timeElapsed = 0f;
        float startAngle = _isFaceUp ? 180 : 0;
        float endAngle = faceUp ? 180 : 0;
        var colorSwapped = false;
    
        while (timeElapsed < flipDuration) 
        {
            var t = timeElapsed / flipDuration;
            var angle = Mathf.Lerp(startAngle, endAngle, t);
            cardImage.transform.rotation = Quaternion.Euler(0, angle, 0);
    
            if (t >= 0.5f && !colorSwapped) 
            {
                cardImage.color = faceUp ? _cardFrontColor : _cardBackColor;
                _isFaceUp = faceUp;
                colorSwapped = true;
            }
    
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _isFlipping = false;
    }
    
    private void FlipFaceUp() => FlipCard(true);
    private void FlipFaceDown() => FlipCard(false);
}
