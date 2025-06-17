using UnityEngine;
using UnityEngine.UI;            
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI 연동")]
    public Slider healthSlider;       // HP 바
    public TextMeshProUGUI healthText;           // 숫자 표시
    // public TMP_Text healthText;     // TextMeshPro 사용 시

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

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
    }

    // 숫자 텍스트 업데이트
    private void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = $"HP: {currentHealth}";
    }

    void Die()
    {
        Debug.Log("플레이어 사망");
        // 추가 게임오버 처리
    }
}