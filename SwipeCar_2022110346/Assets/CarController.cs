using UnityEngine;

public class CarController : MonoBehaviour
{
    float speed = 0;
    Vector2 startPos; //�����Ӱ� ���� ���� (vetor 2��� �޼���(����ü)) ���
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60; //��ǥ ������ �ӵ��� 60fps�� ����
    }

    // Update is called once per frame
    void Update()
    {
        //�������� ���̸� ����
        if (Input.GetMouseButtonDown(0)) //���콺�� Ŭ���ϸ�
        {
            //���콺�� Ŭ���� ��ǥ
            this.startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //���콺 ��ư���� �հ����� �������� ��ǥ
            Vector2 endPos = Input.mousePosition;
            float swipeLength = endPos.x - this.startPos.x;

            this.speed = swipeLength / 500.0f;

            //ȿ���� �����
            GetComponent<AudioSource>().Play();
        }

            transform.Translate(this.speed, 0, 0);//�̵�(x������ �̵�)
             this.speed *= 0.98f; //����   

    }
}
