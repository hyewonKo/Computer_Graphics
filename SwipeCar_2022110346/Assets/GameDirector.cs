using UnityEngine;
using TMPro; //TextMeshPro�� ����ϴµ� �ʿ��ϴ�.

public class GameDirector : MonoBehaviour
{
    GameObject car;
    GameObject flag;
    GameObject distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //car������Ʈ�� ���� " ���� ������Ʈ �̸� �ۼ�"
        this.car = GameObject.Find("car");
        this.flag = GameObject.Find("flag");
        this.distance = GameObject.Find("Distance");
    }

    // Update is called once per frame
    void Update()
    {
        //��߰� ���� ��ġ ���� ���
        float length = this.flag.transform.position.x - this.car.transform.position.x;
        if (length >= 0)
        {
            this.distance.GetComponent<TextMeshProUGUI>().text = "Distance" + length.ToString("F2") + "m" + " ��~��~��~��~";
        }
        else
        {
            //distance ������Ʈ�� Text ������Ʈ �����ͼ� = length�� �Ҽ��� ��°�ڸ����� �Ǽ����� ���������� �ٲ۴�.
            this.distance.GetComponent<TextMeshProUGUI>().text = "GameOver";
        }
    }
}
