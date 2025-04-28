using UnityEngine;

public class DummyDamageDealer : MonoBehaviour
{
    private PlayerHealthAndLivesManager playerHealthManager;

    void Start()
    {
        playerHealthManager = FindObjectOfType<PlayerHealthAndLivesManager>();
    }

    void Update()
    {
        // 按K键模拟玩家受伤
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (playerHealthManager != null)
            {
                playerHealthManager.TakeDamage(1f);
                Debug.Log("玩家受伤了！当前血量：" + playerHealthManager);
            }
        }
    }
}
