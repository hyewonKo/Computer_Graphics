using UnityEngine;
using TMPro; //TextMeshPro를 사용하는데 필요하다.

public class GameDirector : MonoBehaviour
{
    GameObject car;
    GameObject flag;
    GameObject distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //car오브젝트와 연결 " 실제 오브젝트 이름 작성"
        this.car = GameObject.Find("car");
        this.flag = GameObject.Find("flag");
        this.distance = GameObject.Find("Distance");
    }

    // Update is called once per frame
    void Update()
    {
        //깃발과 차의 위치 차이 계산
        float length = this.flag.transform.position.x - this.car.transform.position.x;
        if (length >= 0)
        {
            this.distance.GetComponent<TextMeshProUGUI>().text = "Distance" + length.ToString("F2") + "m" + " 뿡~뿡~뿡~뿡~";
        }
        else
        {
            //distance 오브젝트의 Text 컴포넌트 가져와서 = length를 소수점 둘째자리까지 실수형을 문자형으로 바꾼다.
            this.distance.GetComponent<TextMeshProUGUI>().text = "GameOver";
        }
    }
}
