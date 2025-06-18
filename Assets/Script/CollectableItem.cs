using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour
{
    [Header("Item Settings")]
    public int itemValue = 1; // 이 아이템의 가치 (몇 개를 추가할지)
    public float moveSpeed = 5f; // UI로 이동하는 속도
    public float fadeDuration = 0.5f; // 페이드 아웃 시간
    
  
    private bool isCollected = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;
    private AudioSource audioSouce;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
        audioSouce = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        
        // 재사용시 상태 초기화가 필요
        isCollected = false;
        if (itemCollider != null)
            itemCollider.enabled = true;
    
        // 스프라이트 상태 복원
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }
    
        // 크기 복원
        transform.localScale = Vector3.one; // 또는 원래 크기
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            CollectItem();
        }
    }
    
    private void CollectItem()
    {
        isCollected = true;
        
        // 콜라이더 비활성화 (중복 수집 방지)
        if (itemCollider != null)
            itemCollider.enabled = false;
        
        // 수집 사운드 재생
        if (GameManager.instance.getCoinClip != null)
        {   
            audioSouce.PlayOneShot(GameManager.instance.getCoinClip);
        }
        
        // UI 매니저에 수집 알림
        if (ItemCounterUI.Instance != null)
        {
            ItemCounterUI.Instance.CollectItem(this, itemValue);
        }
        
        // UI로 이동 시작
        StartCoroutine(MoveToUIAndDestroy());
    }
    
    private IEnumerator MoveToUIAndDestroy()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = ItemCounterUI.Instance.GetCounterUIPosition();
        
        float elapsedTime = 0f;
        float totalTime = Vector3.Distance(startPosition, targetPosition) / moveSpeed;
        
        // UI로 이동하면서 크기 줄이기
        Vector3 originalScale = transform.localScale;
        
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / totalTime;
            
            // 위치 이동 (곡선 경로)
            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, progress);
            currentPos.y += Mathf.Sin(progress * Mathf.PI) * 2f; // 포물선 효과
            transform.position = currentPos;
            
            // 크기 줄이기
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
            
            // 투명도 조절
            // if (spriteRenderer != null)
            // {
            //     Color color = spriteRenderer.color;
            //     color.a = Mathf.Lerp(1f, 0f, progress);
            //     spriteRenderer.color = color;
            // }
            //
            yield return null;
        }
        
        gameObject.SetActive(false);
    }
}