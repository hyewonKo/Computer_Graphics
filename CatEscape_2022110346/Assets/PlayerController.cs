using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    float minX = -8.0f;
    float maxX = 8.0f;
    void Start()
    {

    }

    public void LButtonDown()
    {
        //  ���� ��� üũ
        if (transform.position.x > minX)
        {
            transform.Translate(-3, 0, 0);
        }
    }

    public void RButtonDown()
    {
        //  ������ ��� üũ
        if (transform.position.x < maxX)
        {
            transform.Translate(3, 0, 0);
        }
    }



}

