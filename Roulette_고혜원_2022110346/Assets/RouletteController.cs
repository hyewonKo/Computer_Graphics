using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float rotSpeed = 0;//회전속도
    bool isStopping = false;//정지중인지 확인하는 변수
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

        /*basic(수업시간에 배운 내용)
       //클릭하면 회전속도를 설정한다
       if(Input.GetMouseButtonDown(0))//마우스 클릭시
       {
           this.rotSpeed = 10;//회전속도 설정
       }
       //회전속도 만큼 룰렛을 회전시킨다
       transform.Rotate(0, 0, this.rotSpeed);//룰렛 회전


       //회전속도를 줄인다 (감쇠계수 0.96을 곱해서 감속)
       this.rotSpeed *= 0.96f;
        */





        /*
        //1.마우스 휠을 클릭하면 룰렛이 돌아가고, 마우스 왼쪽 버튼을 클릭하면 서서히 멈추게 하세요.
        if(Input.GetMouseButtonDown(2))//마우스 휠 클릭시
        {
            this.rotSpeed = 10;//회전속도 설정
        }
        
        transform.Rotate(0, 0, this.rotSpeed);//룰렛 회전

        if(Input.GetMouseButtonDown(0))//마우스 왼쪽 클릭시
        {
            isStopping = true;       
        }
     
        if(isStopping)//정지중이면
        {
            this.rotSpeed *= 0.96f;//회전속도를 줄인다 (감쇠계수 0.96을 곱해서 감속)
        }

        //참고: if(Input.GetMouseButtonDown(0){ tis.rotSpeed = 0; }는 회전속도는 10->9.6만 딱 한번만 실행됨.클릭 순간에만 true가 되기에
        */



        /*
        //2.마우스 오른쪽 버튼을 클릭한 동안 룰렛이 돌아가고, 손을 떼면 서서히 멈추게 하세요.
        if (Input.GetMouseButton(1))
        {
            this.rotSpeed = 10;//회전속도 설정
        }

        transform.Rotate(0, 0, this.rotSpeed);//룰렛 회전

        if (Input.GetMouseButton(0))//마우스 왼쪽 클릭시
        {
            isStopping = true;
        }
        transform.Rotate(0, 0, this.rotSpeed);//룰렛 회전
        this.rotSpeed *= 0.96f;
    }
        */





        //3.룰렛의 회전 방향을 반대로 하세요.

        if (Input.GetMouseButtonDown(0))
        {
            this.rotSpeed = -10;
        }
        transform.Rotate(0, 0, this.rotSpeed);


        this.rotSpeed *= 0.96f;

    }


    



}
