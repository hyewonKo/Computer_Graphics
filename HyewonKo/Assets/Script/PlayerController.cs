using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 8f;
    public float jumpPower = 1f;
    public float gravity = 20f;
    public float mouseSensitivity = 250f;
    private int jumpCount = 0;
    private int maxJumps = 2;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    public Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // --- 여기부터가 바닥 높이에 맞춰 시작 위치를 조정하는 코드 ---

        // 1. Raycast를 쏠 시작점 (플레이어 현재 위치)과 방향 (아래) 설정
        Vector3 rayStartPoint = transform.position;

        // 2. Raycast 실행
        // rayStartPoint에서 아래(Vector3.down) 방향으로 100미터 길이의 레이저를 쏴서
        // 부딪힌 물체가 있으면 그 정보를 'hit' 변수에 담아라.
        if (Physics.Raycast(rayStartPoint, Vector3.down, out RaycastHit hit, 100f))
        {
            // 3. Raycast가 땅에 부딪혔다면, 부딪힌 위치로 플레이어 위치를 옮김
            // hit.point는 레이저가 부딪힌 정확한 지점의 좌표(Vector3)입니다.
            // 캐릭터 컨트롤러의 발이 땅에 파고들지 않도록 살짝 위로 띄워줍니다.
            transform.position = hit.point + Vector3.up * 0.1f;
        }
    }

    void Update()
    {
        // 땅에 닿았는지 먼저 확인
        if (controller.isGrounded)
        {
            jumpCount = 0;
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        // 마우스 회전
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.Rotate(Vector3.up * mouseX);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 키보드 이동 입력
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;

        // 점프
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * -gravity);
            jumpCount++;
        }

        // 중력 적용
        velocity.y -= gravity * Time.deltaTime;

        // === 최종 이동 적용 (핵심!) ===
        // 키보드 이동(move)과 중력/점프(velocity)를 합쳐서 Move를 '한 번만' 호출
        controller.Move((move * speed + velocity) * Time.deltaTime);
    }
}