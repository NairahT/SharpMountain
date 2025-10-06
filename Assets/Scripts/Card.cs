using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public event Action<Card> OnCardSelected;
    
    [SerializeField] private Button cardButton;
    [SerializeField] private Image cardImage;
    [SerializeField] private float flipDuration = 0.5f;
    [SerializeField] private Sprite cardBackSprite;
    
    public CardType CardType => _card.Type;
    
    private CardData _card;
    private Sprite _cardFrontImg;
    private bool _isFaceUp = false;
    private bool _isFlipping = false;
    private bool _isMatched = false;

    private void OnEnable() => cardButton.onClick.AddListener(OnClickCard);
    private void OnDisable() => cardButton.onClick.RemoveListener(OnClickCard);

    private void Start()
    {
        cardImage.sprite = _isFaceUp ? _card.Image : cardBackSprite;
        _cardFrontImg = _card.Image;
    }

    public void SetCardData(CardData cardData) => _card = cardData;
    
    private void OnClickCard()
    {
        if(_isMatched) return;
        if(_isFlipping) return;
        if(_isFaceUp) return;
        
        FlipCard(true);
        OnCardSelected?.Invoke(this);
    }
    
    private void FlipCard(bool faceUp)
    {
        if(_isFlipping) return;
        if(_isFaceUp == faceUp) return;
        StartCoroutine(Flip(faceUp));
    }
    
    private IEnumerator Flip(bool faceUp) 
    {
        _isFlipping = true;
        
        AudioManager.Instance.PlayFlip();
        
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
                cardImage.sprite = faceUp ? _cardFrontImg : cardBackSprite;
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
        ScaleCardUp();
    } 

    private void ScaleCardUp() => StartCoroutine(ScaleCard(0.1f));
    
    private IEnumerator ScaleCard(float timeToScale)
    {
        var startScale = cardImage.transform.localScale;
        var targetScale = startScale * 1.3f;
        var elapsedTime = 0f;

        while (elapsedTime < timeToScale)
        {
            var t = elapsedTime / timeToScale;
            cardImage.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cardImage.transform.localScale = targetScale;
    }
    
    public void FlipFaceUp() => FlipCard(true);
    public void FlipFaceDown() => StartCoroutine(FlipDownWithDelay());
    
    private IEnumerator FlipDownWithDelay()
    {
        yield return new WaitForSeconds(1f);
        FlipCard(false);
    }
}
