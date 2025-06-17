using UnityEngine;

public class SpiderController : MonoBehaviour

{
    [Header("체력 설정")]
    public int maxHealth = 100;         // 최대 체력
    private int currentHealth;          // 현재 체력

    [Header("추적 및 워크 설정")]
    public Transform target;             // 플레이어 트랜스폼
    public float detectionRange = 10f;   // 플레이어 탐지 범위
    public float attackRange = 2f;       // 공격 범위
    public float speed = 3f;             // 이동 속도

    [Header("랜덤 워크 설정")]
    public float wanderRadius = 8f;      // 랜덤 워크 반경
    public float wanderInterval = 5f;    // 방향 전환 주기 (초)

    [Header("공격 설정")]
    public int attackDamage = 10;        // 한 번 공격 데미지
    public float attackCooldown = 3f;  // 공격 간 최소 간격 (초)

    private Vector3 wanderDestination; //“랜덤 워크”할 때 목표 지점(어디로 걸어갈지) 좌표를 저장
    private float wanderTimer = 0f;
    private float nextAttackTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        ChooseNewWanderDestination();
    }

    void Update()
    {
        float distToTarget = Vector3.Distance(transform.position, target.position);

        if (distToTarget <= detectionRange)
        {
            // 1) 플레이어를 향해 추적
            ChaseTarget();

            // 2) 공격 범위 내라면 공격 시도
            if (distToTarget <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            // 탐지 범위 밖 → 랜덤 워크
            RandomWander();
        }
    }

    // 플레이어 쫓아가기
    void ChaseTarget()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        // 바라보는 방향 설정 (선택 사항)
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = look * Quaternion.Euler(0f, 180f, 0f);
    }

    // 랜덤 워크
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
        Quaternion wanderLook = Quaternion.LookRotation(dir);
        transform.rotation = wanderLook * Quaternion.Euler(0f, 180f, 0f);
    }

    // 새로운 목표 지점 계산
    void ChooseNewWanderDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        wanderDestination = new Vector3(
            transform.position.x + randomPoint.x,
            transform.position.y,
            transform.position.z + randomPoint.y
        );
    }

    // 공격 로직
    void Attack()
    {
        PlayerHealth ph = target.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(attackDamage);
            // 이펙트나 사운드 여기서 재생 가능
            Debug.Log("거미가 공격! 플레이어 남은 HP: " + ph.currentHealth);
        }
    }

    // **총알에 맞았을 때 호출될 메서드**
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"거미 데미지: {amount}, 남은 HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        // TODO: 이펙트, 사운드 재생
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
