// EnemyController.cs (수정된 최종 코드)
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHealth = 100;
    private int currentHealth;
    public Slider EnemyhpSlider;

    [Header("추적 및 워크 설정")]
    public Transform target;
    public float detectionRange = 50f;
    public float attackRange = 5f;
    public float speed = 3f;

    [Header("랜덤 워크 설정")]
    public float wanderRadius = 50f;
    public float wanderInterval = 5f;

    [Header("공격 설정")]
    public int attackDamage = 10;
    public float attackCooldown = 3f;

    [Header("사운드/이벤트 설정")]
    public AudioClip attackSound;
    public AudioClip enemyDieSound;
    public GameObject EnemyDeathEffectPrefab;

    [Header("아이템 드랍 설정")]
    public GameObject healthItemPrefab;
    public GameObject ammoItemPrefab;
    public GameObject attackUpItemPrefab;
    public StoneType holdingStone; // ★ 게임 클리어 핵심 아이템

    private Vector3 wanderDestination;
    private float wanderTimer = 0f;
    private float nextAttackTime = 0f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ChooseNewWanderDestination();
        UpdateHpUI();
    }

    void Update()
    {
        if (target == null) return; // 목표(플레이어)가 없으면 아무것도 안 함

        float distToTarget = Vector3.Distance(transform.position, target.position);

        if (distToTarget <= detectionRange)
        {
            ChaseTarget();
            if (distToTarget <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            RandomWander();
        }
    }

    // 데미지를 받는 함수
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHpUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ★★★ 모든 죽음 관련 로직을 하나로 통합하고 순서를 바로잡은 함수 ★★★
    void Die()
    {
        // 1. 죽음 사운드 재생
        // (AudioSource 컴포넌트가 파괴되기 전에 먼저 재생해야 소리가 남)
        AudioSource.PlayClipAtPoint(enemyDieSound, transform.position);

        // 2. 죽음 이펙트 생성
        if (EnemyDeathEffectPrefab != null)
        {
            GameObject effect = Instantiate(EnemyDeathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f); // 2초 뒤에 이펙트 자동 파괴
        }

        // 3. 게임 클리어에 필요한 '스톤' 드랍 처리
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectStone(holdingStone);
            Debug.Log(holdingStone + " 스톤을 얻었습니다!");
        }

        // 4. 랜덤 아이템 드랍 (체력, 탄약 등)
        DropRandomItem();

        // 5. HP 슬라이더 UI 파괴
        if (EnemyhpSlider != null)
        {
            // 슬라이더 자체만 파괴하거나, 비활성화하는 것이 더 안전함
            EnemyhpSlider.gameObject.SetActive(false);
        }

        // 6. 모든 처리가 끝난 후, 마지막으로 적 자기 자신을 파괴
        Destroy(gameObject);
    }

    // 함수 이름을 더 명확하게 변경
    void DropRandomItem()
    {
        float dropChance = Random.Range(0f, 1f);
        GameObject itemToDrop = null;

        if (dropChance <= 0.3f) { itemToDrop = healthItemPrefab; }
        else if (dropChance <= 0.5f) { itemToDrop = ammoItemPrefab; }
        else if (dropChance <= 0.7f) { itemToDrop = attackUpItemPrefab; }

        if (itemToDrop != null)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }

    // --- 이하 이동 및 공격 로직은 잘 작성되었으므로 그대로 유지 ---

    void ChaseTarget()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void RandomWander()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval)
        {
            wanderTimer = 0f;
            ChooseNewWanderDestination();
        }
        Vector3 dir = (wanderDestination - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    void ChooseNewWanderDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        wanderDestination = new Vector3(transform.position.x + randomPoint.x, transform.position.y, transform.position.z + randomPoint.y);
    }

    void Attack()
    {
        PlayerHealth ph = target.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(attackDamage);
            audioSource.PlayOneShot(attackSound);
        }
    }

    void UpdateHpUI()
    {
        if (EnemyhpSlider != null)
        {
            EnemyhpSlider.value = (float)currentHealth / maxHealth;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void ModifySpeed(float amount)
    {
        speed += amount;
        // 속도가 잘 변경되었는지 확인하기 위한 디버그 로그
        Debug.Log("적 속도 변경! 현재 속도: " + speed);
    }
}