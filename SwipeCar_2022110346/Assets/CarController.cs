using UnityEngine;

public class CarController : MonoBehaviour
{
    float speed = 0;
    Vector2 startPos; //위차ㅣ값 저장 목적 (vetor 2라는 메서드(구조체)) 사용
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60; //목표 프레임 속도를 60fps로 설정
    }

    // Update is called once per frame
    void Update()
    {
        //스와이프 길이를 구함
        if (Input.GetMouseButtonDown(0)) //마우스를 클릭하면
        {
            //마우스를 클릭한 좌표
            this.startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //마우스 버튼에서 손가락을 떼었을때 좌표
            Vector2 endPos = Input.mousePosition;
            float swipeLength = endPos.x - this.startPos.x;

            this.speed = swipeLength / 500.0f;

            //효과음 재생함
            GetComponent<AudioSource>().Play();
        }

            transform.Translate(this.speed, 0, 0);//이동(x축으로 이동)
             this.speed *= 0.98f; //감속   

    }
}
