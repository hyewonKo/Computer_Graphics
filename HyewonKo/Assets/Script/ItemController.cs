using UnityEngine;

public class ItemController : MonoBehaviour
{
    // 아이템의 종류를 Inspector에서 선택할 수 있게 하는 열거형(Enum)
    public enum ItemType
    {
        Ammo,
        Health,
        Speed,
        AttackUp,
        Stone // 스톤 추가
    }

    public ItemType type; // 이 아이템의 종류
    public int value;     // 아이템의 값 (탄약 수, 회복량 등)

 

    public GameObject pickupEffectPrefab; // 획득 시 나타날 이펙트
    public AudioClip pickupSound;         // 획득 시 재생될 사운드

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힌 오브젝트가 'Player' 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            // 기본적으로 픽업은 실패 상태로 시작
            bool pickupSuccessful = false;

            // 아이템 종류에 따라 다른 기능 호출
            switch (type)
            {
                case ItemType.Ammo:
                    // 플레이어에게서 GunController를 찾아봄
                    GunController gunCtrl_ammo = other.GetComponent<GunController>();
                    // GunController가 있고, 총을 가지고 있을 때만 획득 성공
                    if (gunCtrl_ammo.hasGun)
                    {
                        gunCtrl_ammo.AddAmmo(value);
                        pickupSuccessful = true;
                    }
                    else
                    {
                        // 총이 없을 경우 알림 메시지만 표시
                        other.GetComponent<PlayerHealth>().ShowNotification("First, You need to find a gun.");

                    }
                    break;
                case ItemType.Health:
                    // 플레이어에게서 PlayerHealth를 찾아 Heal 함수 호출
                    other.GetComponent<PlayerHealth>().Heal(value);
                    pickupSuccessful = true;
                    break;
                case ItemType.Speed:
                    // 플레이어에게서 PlayerController를 찾아 ApplySpeedBoost 함수 호출
                    other.GetComponent<PlayerController>().ApplySpeedBoost(value);
                    pickupSuccessful = true;
                    break;
                case ItemType.AttackUp:
                    // 플레이어에게서 GunController를 찾아 ApplyAttackBoost 함수 호출
                    other.GetComponent<GunController>().ApplyAttackBoost(value);
                    pickupSuccessful = true;
                    break;

                
            }

            // 획득 이펙트 및 사운드 처리
            if (pickupSuccessful)
            {
                if (pickupEffectPrefab != null)
                {
                    // Y값을 원하는 만큼 내린다
                    Vector3 spawnPosition = transform.position;
                    spawnPosition.y -= 0.5f;


                    GameObject effect = Instantiate(pickupEffectPrefab, spawnPosition, Quaternion.identity);
                    Destroy(effect, 3f); // 2초 후에 이펙트 삭제
                }

                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                Destroy(gameObject);
            }
        }
    }
}
