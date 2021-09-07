using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector untuk mengatur jarak kamera dengant bola meriam
        Vector3 decPos = new Vector3(-80, 0, 0);

        //Vector untuk mendapatkan nilai akhir posisi kamera setelah ditambah nilai decPos
        Vector3 pos = SimulationData.cannonBall.transform.position + decPos;

        //atur nilai transform camera
        transform.position = pos;
    }
}
