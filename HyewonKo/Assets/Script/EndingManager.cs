using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    // '다시 시작' 버튼이 호출할 함수
    public void RestartGame()
    {
        // 게임 씬의 이름을 정확하게 적어줘야 합니다.
        // 연늘님의 경우 "GameScene" 입니다.
        SceneManager.LoadScene("GameScene");
    }

    // '종료' 버튼이 호출할 함수
    public void QuitGame()
    {
        Application.Quit();
    }
}
