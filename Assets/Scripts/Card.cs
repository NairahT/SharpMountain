using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button cardButton;
    [SerializeField] private Image cardImage;
    [SerializeField] private float flipDuration = 0.5f;
    [SerializeField] private CardData card;
    
    public event Action<Card> OnCardSelected;
    public CardType CardType => card.type;
    
    private Color _cardFrontColor;
    private readonly Color _cardBackColor = Color.green;
    
    private bool _isFaceUp = false;
    private bool _isFlipping = false;
    private bool _isMatched = false;

    private void OnEnable() => cardButton.onClick.AddListener(OnClickCard);
    private void OnDisable() => cardButton.onClick.RemoveListener(OnClickCard);

    private void Start()
    {
        cardImage.color = _isFaceUp ? _cardFrontColor : _cardBackColor;
        _cardFrontColor = card.cardColor;
    }

    private void OnClickCard()
    {
        FlipCard(true);
        OnCardSelected?.Invoke(this);
    }
    
    private void FlipCard(bool faceUp)
    {
        if (_isFlipping) return;
        if(_isMatched)  return;
        if(_isFaceUp == faceUp) return;
        StartCoroutine(Flip(faceUp));
    }
    
    private IEnumerator Flip(bool faceUp) 
    {
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

    public void FoundMatch()
    {
        _isMatched = true;
    }
    
    private void FlipFaceUp() => FlipCard(true);
    public void FlipFaceDown() => FlipCard(false);
}
