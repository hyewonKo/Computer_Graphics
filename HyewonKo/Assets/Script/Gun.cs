using UnityEngine;

public class Gun : MonoBehaviour
{
    private void Awake()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;  // 트리거 모드로 설정
    }

    private void OnTriggerEnter(Collider other)
    {
        var gunCtrl = other.GetComponentInChildren<GunController>();
        if (gunCtrl != null && !gunCtrl.hasGun)
        {
            gunCtrl.PickupGun();
            Destroy(gameObject);  // 아이템 삭제
        }
    }
}