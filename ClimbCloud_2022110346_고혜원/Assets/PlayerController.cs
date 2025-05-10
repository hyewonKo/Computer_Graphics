using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce=680.0f;
    float walkForce=30.0f;
    float maxWalkSpeed=2.0f;


    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    void Update()
    {

        

        //점프
        if (Input.GetKeyDown(KeyCode.Space)&& this.rigid2D.linearVelocityY == 0)
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.AddForce(transform.up * this.jumpForce);

        }


        //좌우이동
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        //플레이어 속도
        float speedx = Mathf.Abs(this.rigid2D.linearVelocity.x);

        //속도제한
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }

        //움직임 방향에 따라 캐릭터 뒤집기
        if (key != 0)
        {
            transform.localScale = new Vector3(key,1,1);
        }

        //플레이어의 속도에 맞춰서 애니메이션 속도 조절
        if (this.rigid2D.linearVelocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }


        //플레이어가 화면 밖으로 나가면 처음부터
        if (transform.position.y < -10.0f)
        {
            SceneManager.LoadScene("SampleScene");
        }

        
    }

    //골 도착
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("골");
        SceneManager.LoadScene("ClearScene");
    }
}
