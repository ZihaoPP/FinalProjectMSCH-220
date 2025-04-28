using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthAndLivesManager : MonoBehaviour
{
    [Header("血量系统设置")]
    public Image healthFillImage;
    public float maxHP = 5f;
    private float currentHP;

    [Header("命数系统设置")]
    public int maxLives = 3;
    private int currentLives;

    [Header("结束场景设置")]
    public string endSceneName = "End";

    void Start()
    {
        currentHP = maxHP;
        currentLives = maxLives;
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();

        if (currentHP <= 0)
        {
            HandlePlayerDeath();
        }
    }

    // ⭐ 受到伤害时调用
    public void TakeDamage(float damageAmount)
    {
        currentHP -= damageAmount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        Debug.Log($"玩家受到了 {damageAmount} 点伤害，当前血量: {currentHP}");
    }

    // ⭐ 碰到敌人时自动触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Meteor") || collision.CompareTag("EnemyBullet"))
        {
            TakeDamage(1f); // 受到1点伤害（你可以根据敌人设定改）
            Destroy(collision.gameObject); // 碰到后销毁敌人或陨石
        }
    }

    private void UpdateHealthBar()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = Mathf.Clamp01(currentHP / maxHP);
        }
    }

    private void HandlePlayerDeath()
    {
        currentLives--;

        if (currentLives > 0)
        {
            currentHP = maxHP;
            UpdateHealthBar();
        }
        else
        {
            if (!string.IsNullOrEmpty(endSceneName))
            {
                SceneManager.LoadScene(endSceneName);
            }
        }
    }
}
