using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [Header("护盾生命周期设置")]
    public float activeDuration = 20f;
    public float windowDuration = 5f;
    public float fadeOutDuration = 2f;
    public int flashCount = 5;
    public float flashInterval = 0.3f;

    private SpriteRenderer spriteRenderer;
    private Collider2D shieldCollider;
    private float timer = 0f;
    private bool isShieldActive = true;
    private bool isWaitingToRegen = false;
    private Coroutine fadeCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shieldCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null || shieldCollider == null)
        {
            Debug.LogError("[ShieldController] 缺少必要组件！");
            enabled = false;
            return;
        }

        shieldCollider.enabled = true;
        spriteRenderer.enabled = true;
        SetAlpha(1f);
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isShieldActive && timer >= activeDuration)
        {
            if (fadeCoroutine == null)
            {
                fadeCoroutine = StartCoroutine(FlashAndFadeOut());
            }
        }
        else if (isWaitingToRegen && timer >= activeDuration + windowDuration)
        {
            RegenerateShield();
        }
    }

    IEnumerator FlashAndFadeOut()
    {
        isShieldActive = false;
        shieldCollider.enabled = false;

        // 闪烁阶段
        for (int i = 0; i < flashCount; i++)
        {
            SetAlpha(0f);
            yield return new WaitForSeconds(flashInterval / 2f);
            SetAlpha(1f);
            yield return new WaitForSeconds(flashInterval / 2f);
        }

        // 慢慢淡出阶段
        float elapsedFadeTime = 0f;
        float startAlpha = spriteRenderer.color.a;

        while (elapsedFadeTime < fadeOutDuration)
        {
            elapsedFadeTime += Time.deltaTime;
            float fadePercent = Mathf.Clamp01(elapsedFadeTime / fadeOutDuration);
            float alpha = Mathf.Lerp(startAlpha, 0f, fadePercent);
            SetAlpha(alpha);
            yield return null;
        }

        // 完全透明后，让护盾SpriteRenderer和Collider都禁用（但物体活着）
        SetAlpha(0f);
        spriteRenderer.enabled = false;
        shieldCollider.enabled = false;

        isWaitingToRegen = true;
        fadeCoroutine = null;
    }

    void RegenerateShield()
    {
        spriteRenderer.enabled = true;
        shieldCollider.enabled = true;
        SetAlpha(1f);

        timer = 0f;
        isShieldActive = true;
        isWaitingToRegen = false;
    }

    void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
