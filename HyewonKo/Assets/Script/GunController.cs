using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(AudioSource))]
public class GunController : MonoBehaviour
{
    [Header("총 설정")]
    public int bulletDamage = 15;    // ← 한 발당 데미지
    public float fireRate = 0.5f; // 발사 간격(초)
    public float range = 50f; // 사정거리

    [Header("이펙트 및 사운드 설정")] 
    public GameObject impactEffectPrefab;   // 피격 이펙트 프리팹
    public AudioClip bulletSound;             // 발사 사운드
    public AudioClip PickupGunSound;


    [Header("UI 연동")]
    public TextMeshProUGUI ammoText; // 남은 탄약을 표시할 UI 텍스트

    private int currentAmmo; // 현재 남은 탄약
    private float nextFireTime = 0f; // 다음 발사 가능 시간
    public bool hasGun = false;  // 총 획득 여부 플래그
    private AudioSource audioSource;        // 사운드 재생기

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentAmmo = 0;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (!hasGun) return;

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    public void PickupGun()
    {
        hasGun = true;
        audioSource.PlayOneShot(PickupGunSound);
        currentAmmo = 10;
        UpdateAmmoUI();
        GetComponent<PlayerHealth>().ShowNotification("You got a gun.");
    }

    void Shoot()
    {
        currentAmmo--; // 탄약 1 감소
        UpdateAmmoUI();

        // 1. 사운드 재생
        if (bulletSound != null)
        {
            audioSource.PlayOneShot(bulletSound);
        }

        // 화면의 정중앙 좌표(x, y)를 계산합니다.
        Vector3 screenCenterPoint = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        // 화면 정중앙에서부터 월드 공간으로 뻗어나가는 레이(Ray)를 생성합니다.
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);


        // Raycast를 발사하여 부딪히는 물체가 있는지 확인합니다.
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            // 피격 이펙트 생성
            if (impactEffectPrefab != null)
            {
                // 부딪힌 위치(hit.point)에, 부딪힌 표면의 방향(hit.normal)으로 이펙트를 생성합니다.
                Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // 부딪힌 오브젝트에서 EnemyController를 찾아 데미지를 줍니다.
            var enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
        }
    }

    // 탄약 추가 함수
    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        UpdateAmmoUI();
        // PlayerHealth 스크립트를 찾아서 알림을 띄워달라고 요청
        GetComponentInParent<PlayerHealth>().ShowNotification("Ammo +" + amount);
    }

    // 공격력 강화 함수 
    public void ApplyAttackBoost(int amount)
    {
        bulletDamage += amount;
        // PlayerHealth 스크립트를 찾아서 알림을 띄워달라고 요청
        GetComponentInParent<PlayerHealth>().ShowNotification("Attack Damage +" + amount);
    }

    // UI 텍스트에 남은 탄약 표시 업데이트
    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(hasGun);  // 총이 있을 때만 보이기

            if (hasGun)

                ammoText.text = $"Ammo : {currentAmmo}";
        }
    }
}