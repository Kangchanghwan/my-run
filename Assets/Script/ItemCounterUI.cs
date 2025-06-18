using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemCounterUI : MonoBehaviour
{
    public static ItemCounterUI Instance { get; private set; }
    
    [Header("UI References")]
    public TextMeshProUGUI counterText; // 또는 public Text counterText;
    public Transform targetTransform; // 아이템이 이동할 목표 위치 (빈 GameObject의 Transform)
    
    [Header("Animation Settings")]
    public float scaleAnimationDuration = 0.3f;
    public float scaleMultiplier = 1.2f;
    public Color flashColor = Color.yellow;
    public float flashDuration = 0.2f;
    
    [Header("Counter Settings")]
    public int currentCount = 0;
    
    private Vector3 originalScale;
    private Color originalColor;
    private Camera mainCamera;
    
    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        mainCamera = Camera.main;
        if (counterText != null)
        {
            originalScale = counterText.transform.localScale;
            originalColor = counterText.color;
        }
        
        UpdateCounterText();
    }
    
    public void CollectItem(CollectibleItem item, int value)
    {
        currentCount += value;
        UpdateCounterText();
        StartCoroutine(PlayCollectAnimation());
    }
    
    public Vector3 GetCounterUIPosition()
    {
        if (targetTransform != null)
        {
            return targetTransform.position;
        }
        
        return Vector3.zero;
    }
    
    private void UpdateCounterText()
    {
        if (counterText != null)
        {
            counterText.text = currentCount.ToString();
        }
    }
    
    private IEnumerator PlayCollectAnimation()
    {
        if (counterText == null) yield break;
        
        // 색상 플래시 효과
        StartCoroutine(FlashColor());
        
        // 크기 애니메이션
        Vector3 targetScale = originalScale * scaleMultiplier;
        float elapsedTime = 0f;
        
        // 확대
        while (elapsedTime < scaleAnimationDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (scaleAnimationDuration / 2f);
            counterText.transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            yield return null;
        }
        
        elapsedTime = 0f;
        
        // 축소
        while (elapsedTime < scaleAnimationDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (scaleAnimationDuration / 2f);
            counterText.transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            yield return null;
        }
        
        counterText.transform.localScale = originalScale;
    }
    
    private IEnumerator FlashColor()
    {
        if (counterText == null) yield break;
        
        float elapsedTime = 0f;
        
        // 플래시 색상으로 변경
        while (elapsedTime < flashDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (flashDuration / 2f);
            counterText.color = Color.Lerp(originalColor, flashColor, progress);
            yield return null;
        }
        
        elapsedTime = 0f;
        
        // 원래 색상으로 복원
        while (elapsedTime < flashDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (flashDuration / 2f);
            counterText.color = Color.Lerp(flashColor, originalColor, progress);
            yield return null;
        }
        
        counterText.color = originalColor;
    }
    
    // 외부에서 카운터를 리셋하고 싶을 때 사용
    public void ResetCounter()
    {
        currentCount = 0;
        UpdateCounterText();
    }
    
    // 현재 카운트 값을 가져오고 싶을 때 사용
    public int GetCurrentCount()
    {
        return currentCount;
    }
}