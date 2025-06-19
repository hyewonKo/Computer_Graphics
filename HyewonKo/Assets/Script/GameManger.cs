using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement; // ★★★ 씬 관리를 위해 추가
using UnityEngine.Audio;

// 스톤의 색깔을 정의하는 열거형(Enum)
public enum StoneType
{
    Red = 1,
    Orange = 2,
    Yellow = 3,
    Green = 4,
    Blue = 5
}

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- 상태 변수 ---
    private bool isGameActive = true;
    private float timer;
 // 예시 체력, 외부에서 이 값을 변경하여 테스트 가능

    [Header("기본 UI 요소")]
    public Image[] stoneUI_Images; // 5개의 스톤 UI 이미지 배열
    public TextMeshProUGUI notification_Text;
    public float notificationDisplayTime = 3f;

    [Header("스톤 색상 설정")]
    public Color initialBlackColor = Color.black;
    public Color[] stoneTypeColors; // 획득 시 적용될 각 스톤의 실제 색상 배열 (빨,주,노,초,파 순서)

    [Header("사운드 설정")]
    public AudioClip CollectStoneSound;
    public AudioClip ambienceSound;
    private AudioSource audioSource;

    // --- ★★★ 게임 종료 UI 관련 변수들 추가 ---
    [Header("게임 클리어 UI")]
    public GameObject gameClear_UI; // 게임 클리어 시 활성화될 패널
    public TextMeshProUGUI clearTime_Text; // 클리어 시간 표시 텍스트
    public Image[] scoreStar_Images; // ★★★ 별점 표시용 이미지 배열 (3개)
    public ParticleSystem GameClearEffect; // 신기록 달성 시 폭죽 파티클

    [Header("게임 오버 UI")]
    public GameObject gameOver_UI; // 게임 오버 시 활성화될 패널

    [Header("게임 안내 문구")]
    public TextMeshProUGUI instructionText;


    // --- 데이터 관리 ---
    private HashSet<StoneType> collectedStones = new HashSet<StoneType>();

    private bool speedBoost1Applied = false;
    private bool speedBoost2Applied = false;
    private bool speedNerfApplied = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (ambienceSound != null)
        {
            audioSource.clip = ambienceSound; // 오디오소스에 배경음악 클립을 지정
            audioSource.loop = true;          // 계속 반복되도록 설정
            audioSource.Play();               // 재생!
        }

        // ★★★ 게임 상태 및 UI 초기화
        isGameActive = true;
        timer = 0f;


        InitializeUI();
        StartCoroutine(ShowInstructionRoutine());
    }

    void Update()
    {
        // 게임이 진행 중일 때만 시간 측정
        if (isGameActive)
        {
            timer += Time.deltaTime;

           
            // 1. 게임 시작 1분(60초) 후: 속도 +0.5
            // 아직 1차 속도 증가가 적용되지 않았고, 타이머가 60초를 넘었다면
            if (!speedBoost1Applied && timer >= 60f)
            {
                speedBoost1Applied = true; // 적용되었다고 표시 (다시는 실행되지 않도록)
                ModifyAllEnemiesSpeed(0.5f); // 모든 적의 속도를 1 올림
                Debug.Log("게임 시간 1분 경과. 모든 적 속도 +0.5");
            }

            // 2. 게임 시작 2분(120초) 후: 속도 +0.5
            if (!speedBoost2Applied && timer >= 120f)
            {
                speedBoost2Applied = true;
                ModifyAllEnemiesSpeed(0.5f); // 모든 적의 속도를 1 또 올림
                Debug.Log("게임 시간 2분 경과. 모든 적 속도 +0.5");
            }

            // 3. 게임 시작 3분(180초) 후: 속도 -0.5
            if (!speedNerfApplied && timer >= 180f)
            {
                speedNerfApplied = true;
                ModifyAllEnemiesSpeed(-0.5f); // 모든 적의 속도를 1 내림
                Debug.Log("게임 시간 3분 경과. 모든 적 속도 -0.5. 속도 조절 종료.");
            }
        }
    }

    void InitializeUI()
    {
        foreach (var img in stoneUI_Images)
        {
            img.color = initialBlackColor;
        }
        notification_Text.gameObject.SetActive(false);

        // ★★★ 게임 종료 UI들은 시작 시 비활성화
        gameClear_UI.SetActive(false);
        gameOver_UI.SetActive(false);
    }

    public void CollectStone(StoneType stoneType)
    {
        if (!isGameActive) return; // 게임이 끝났으면 스톤 획득 불가

        if (collectedStones.Contains(stoneType))
        {
            ShowNotification("This is a stone that has already been acquired..");
        }
        else
        {
            collectedStones.Add(stoneType);
            int uiIndex = (int)stoneType - 1;

            if (uiIndex >= 0 && uiIndex < stoneUI_Images.Length)
            {
                stoneUI_Images[uiIndex].color = stoneTypeColors[uiIndex];
            }

            ShowNotification("You have obtained the " + stoneType.ToString() + " stone!");
            StartCoroutine(PlaySoundAfterDelay(1f));

            // ★5개 모두 모았는지 체크
            if (collectedStones.Count >= 5)
            {
                GameClear();
            }
        }
    }

    
    // 게임 클리어 처리 함수
    private void GameClear()
    {
        isGameActive = false;
        StopAllCoroutines(); // 알림 코루틴 등 모두 정지
        Time.timeScale = 0f;
        notification_Text.gameObject.SetActive(false);
        gameClear_UI.SetActive(true);
        if (GameClearEffect != null)
        {
            // 파티클 재생!
            GameClearEffect.Play();
        }

        // 1. 클리어 시간 표시
        clearTime_Text.text = "Clear Time: " + timer.ToString("F2") + "s";

        
        // 2. 스코어(별 이미지) 계산 및 표시
        int starCount = 0;
        if (timer < 180.0f) starCount = 3;      // 3분 미만: 별 3개
        else if (timer < 240.0f) starCount = 2; // 4분 미만: 별 2개
        else starCount = 1;                     // 그 외: 별 1개

        
        // ★★★ 바로 이 부분이 핵심입니다 ★★★
        for (int i = 0; i < scoreStar_Images.Length; i++)
        {
            // 획득한 별 개수만큼만 이미지를 활성화
            scoreStar_Images[i].gameObject.SetActive(i < starCount);
        }
        
    }


    public void GameOver()
    {
        isGameActive = false;
        StopAllCoroutines();
        notification_Text.gameObject.SetActive(false);
        gameOver_UI.SetActive(true);
        gameClear_UI.SetActive(false);
        Time.timeScale = 0f;
    }


    

   

    // --- 기존 알림 및 사운드 재생 기능 ---
    private void ShowNotification(string message)
    {
        // 게임이 끝나면 새 알림이 뜨지 않도록
        if (!isGameActive) return;

        StopCoroutine("NotificationCoroutine"); // 동일 이름의 코루틴 중복 실행 방지
        StartCoroutine(NotificationCoroutine(message));
    }

    private IEnumerator NotificationCoroutine(string message)
    {
        notification_Text.text = message;
        notification_Text.gameObject.SetActive(true);
        yield return new WaitForSeconds(notificationDisplayTime);
        notification_Text.gameObject.SetActive(false);
    }

    private IEnumerator PlaySoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (CollectStoneSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(CollectStoneSound);
        }
    }

    private IEnumerator ShowInstructionRoutine()
    {
        // instructionText가 연결되어 있을 때만 실행
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(true);  // 1. 텍스트를 화면에 보여준다.
            yield return new WaitForSecondsRealtime(5f);        // 2. 5초를 기다린다.
            instructionText.gameObject.SetActive(false); // 3. 5초가 지나면 텍스트를 숨긴다.
        }
    }
    void ModifyAllEnemiesSpeed(float amount)
    {
        // 새로운 방식인 FindObjectsByType 함수로 교체
        EnemyController[] allEnemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        // 모든 적들을 순회하면서 속도 변경 함수를 호출
        foreach (EnemyController enemy in allEnemies)
        {
            enemy.ModifySpeed(amount);
        }
    }


}