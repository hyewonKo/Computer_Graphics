using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float rotSpeed = 0;//ȸ���ӵ�
    bool isStopping = false;//���������� Ȯ���ϴ� ����
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

        /*basic(�����ð��� ��� ����)
       //Ŭ���ϸ� ȸ���ӵ��� �����Ѵ�
       if(Input.GetMouseButtonDown(0))//���콺 Ŭ����
       {
           this.rotSpeed = 10;//ȸ���ӵ� ����
       }
       //ȸ���ӵ� ��ŭ �귿�� ȸ����Ų��
       transform.Rotate(0, 0, this.rotSpeed);//�귿 ȸ��


       //ȸ���ӵ��� ���δ� (������ 0.96�� ���ؼ� ����)
       this.rotSpeed *= 0.96f;
        */





        /*
        //1.���콺 ���� Ŭ���ϸ� �귿�� ���ư���, ���콺 ���� ��ư�� Ŭ���ϸ� ������ ���߰� �ϼ���.
        if(Input.GetMouseButtonDown(2))//���콺 �� Ŭ����
        {
            this.rotSpeed = 10;//ȸ���ӵ� ����
        }
        
        transform.Rotate(0, 0, this.rotSpeed);//�귿 ȸ��

        if(Input.GetMouseButtonDown(0))//���콺 ���� Ŭ����
        {
            isStopping = true;       
        }
     
        if(isStopping)//�������̸�
        {
            this.rotSpeed *= 0.96f;//ȸ���ӵ��� ���δ� (������ 0.96�� ���ؼ� ����)
        }

        //����: if(Input.GetMouseButtonDown(0){ tis.rotSpeed = 0; }�� ȸ���ӵ��� 10->9.6�� �� �ѹ��� �����.Ŭ�� �������� true�� �Ǳ⿡
        */



        /*
        //2.���콺 ������ ��ư�� Ŭ���� ���� �귿�� ���ư���, ���� ���� ������ ���߰� �ϼ���.
        if (Input.GetMouseButton(1))
        {
            this.rotSpeed = 10;//ȸ���ӵ� ����
        }

        transform.Rotate(0, 0, this.rotSpeed);//�귿 ȸ��

        if (Input.GetMouseButton(0))//���콺 ���� Ŭ����
        {
            isStopping = true;
        }
        transform.Rotate(0, 0, this.rotSpeed);//�귿 ȸ��
        this.rotSpeed *= 0.96f;
    }
        */





        //3.�귿�� ȸ�� ������ �ݴ�� �ϼ���.

        if (Input.GetMouseButtonDown(0))
        {
            this.rotSpeed = -10;
        }
        transform.Rotate(0, 0, this.rotSpeed);


        this.rotSpeed *= 0.96f;

    }


    



}
