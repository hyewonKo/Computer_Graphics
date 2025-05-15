using UnityEngine;

public class BamsongiController : MonoBehaviour

{
    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }

    //다른 물체에 충돌시, 중력(물리법칙)을 무효로 만든다
    private void OnCollisionEnter(Collision other)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ParticleSystem>().Play();
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        //Shoot(new Vector3(0,200,2000));
    }

   
    void Update()
    {
        
    }
}
