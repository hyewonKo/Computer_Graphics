using UnityEngine;
using UnityEngine.UI;            
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI 연동")]
    public Slider healthSlider;       // HP 바
    public TextMeshProUGUI healthText;           // 숫자 표시
    public TextMeshProUGUI notificationText; // 아이템 획득 알림용 텍스트
    public float notificationDuration = 2f;  // 알림이 떠 있는 시간

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (notificationText != null)
            notificationText.gameObject.SetActive(false); // 시작할 때 알림 텍스트 숨기기
    
        UpdateHealthText();

    }

    // 데미지를 입었을 때 호출
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        UpdateHealthText();

        if (currentHealth <= 0)
            Die();
    }

    // HP 회복 시에도 이 메서드를 호출하면 됩니다.
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        UpdateHealthText();
        ShowNotification("Health +" + amount);
    }

    // 숫자 텍스트 업데이트
    private void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = $"HP: {currentHealth}";
    }

    // 알림을 보여주는 공용 함수 (새로 추가)
    public void ShowNotification(string message)
    {
        StartCoroutine(ShowNotificationCoroutine(message));
    }
    // 알림 코루틴 (새로 추가)
    IEnumerator ShowNotificationCoroutine(string message)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);
            yield return new WaitForSeconds(notificationDuration);
            notificationText.gameObject.SetActive(false);
        }
    }
    void Die()
    {
        Debug.Log("플레이어 사망");
        GameManager.Instance.GameOver();
        // 추가 게임오버 처리
    }
}