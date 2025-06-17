using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GunController : MonoBehaviour
{
    [Header("총 설정")]
    public int bulletDamage = 20;    // ← 한 발당 데미지
    public float fireRate = 0.5f; // 발사 간격(초)
    public float range = 50f; // 사정거리

    [Header("UI 연동")]
    public TextMeshProUGUI ammoText; // 남은 탄약을 표시할 UI 텍스트

    private int currentAmmo; // 현재 남은 탄약
    private float nextFireTime = 0f; // 다음 발사 가능 시간
    public bool hasGun = false;  // 총 획득 여부 플래그

    void Start()
    {
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
        currentAmmo = 10;
        UpdateAmmoUI();
    }

    void Shoot()
    {
        currentAmmo--; // 탄약 1 감소
        Debug.Log("Shoot 함수 호출! 현재 탄약: " + currentAmmo);
        UpdateAmmoUI();

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            var spider = hit.collider.GetComponent<SpiderController>();
            if (spider != null)
            {
                spider.TakeDamage(bulletDamage);  // ← 여기서 20 데미지
                // 이펙트/사운드 추가 가능
            }
        }
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