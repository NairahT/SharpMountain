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
    public CardState CurrentState { get; private set; } = CardState.FaceDown;
    
    private CardData _card;
    private Sprite _cardFrontImg;
    private bool _isFlipping = false;
    
    private bool IsCurrentlyUp => CurrentState is CardState.FaceUp or CardState.Matched;

    private void OnEnable() => cardButton.onClick.AddListener(OnClickCard);
    private void OnDisable() => cardButton.onClick.RemoveListener(OnClickCard);

    public void SetCardData(CardData cardData)
    {
        _card = cardData;
        cardImage.sprite = cardBackSprite;
        _cardFrontImg = _card.Image;
        CurrentState = CardState.FaceDown;
    }
    
    private void OnClickCard()
    {
        if (IsCurrentlyUp) return;
        if(_isFlipping) return;
        
        FlipCard(true);
        OnCardSelected?.Invoke(this);
    }
    
    private void FlipCard(bool faceUp)
    {
        if(_isFlipping) return;
        
        if(IsCurrentlyUp == faceUp) return;
        
        StartCoroutine(Flip(faceUp));
    }
    
    private IEnumerator Flip(bool faceUp) 
    {
        _isFlipping = true;
        
        AudioManager.Instance.PlayFlip();
        
        var timeElapsed = 0f;
        float startAngle = IsCurrentlyUp ? 180 : 0;
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
                if (CurrentState != CardState.Matched)
                {
                    CurrentState = faceUp ? CardState.FaceUp : CardState.FaceDown;
                }
                colorSwapped = true;
            }
    
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _isFlipping = false;
    }

    public void FoundMatch()
    {
        CurrentState = CardState.Matched;
        ScaleCardUp();
    }

    public void SetCardState(CardState state)
    {
        CurrentState = state;

        switch (state)
        {
            case CardState.Matched:
                cardImage.sprite = _cardFrontImg;
                cardImage.transform.rotation = Quaternion.Euler(0, 180, 0);
                cardImage.transform.localScale = Vector3.one * 1.3f; 
                break;
            case CardState.FaceDown:
            default:
                cardImage.sprite = cardBackSprite;
                cardImage.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
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
