using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        // 이 오브젝트가 항상 메인 카메라를 "바라보게" 만듭니다.
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }
}
