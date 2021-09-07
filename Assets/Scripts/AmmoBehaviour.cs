using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class AmmoBehaviour : MonoBehaviour
{
    //variabel waktu
    [HideInInspector] public static float t = 0f;
    [SerializeField] public GameObject parentBallLog;
    [SerializeField] public GameObject ballLog;

    // Start is called before the first frame update
    void Start()
    {
        SimulationData.parentBallLog = parentBallLog;
        SimulationData.ballLog = ballLog;
    }

    // Update is called once per frame
    void Update()
    {
        //call fungsi Bergerak tanpa parameter
        Bergerak();
    }

    private void Bergerak()
    {
        if (SimulationData.startMove == true) //cek kondisi variabel apakah true/false
        {
            //deklrasai ketiga nilai x y dan z, untuk x dan y menggunakan fungsi posisiX dan posisiY dengan parameter
            float posX = PosisiX(t, SimulationData.kecepatanPeluru, SimulationData.sudutTembak, SimulationData.pengaruhAngin, SimulationData.nilaiAngin);
            float posY = PosisiY(t, SimulationData.kecepatanPeluru, SimulationData.sudutTembak);
            float posZ = transform.position.z;

            //inisialisasi variabel vector3 pos
            Vector3 pos = Vector3.zero;

            //cek kondisi peluru apakah belum menyentuh tanah RWT atau belum
            if (transform.position.y + posY > SimulationData.posisiTarget.y)
            {
                //deklarasi nilai variabel vector3 pos dengan data data pada deklarasai posX posY dan posZ sebelumnya
                pos = new Vector3(transform.position.x + posX, transform.position.y + posY, posZ);
                //Debug.Log(posX + ", " + posY + ", " + posZ + "\n\n" + pos);

                //realtime data pada text ui
                SimulationData.infoText.text = "Nama Target: " + SimulationData.namaTarget +
                    "\nPosisi Peluru: " + posX.ToString() + ", " + posY.ToString() + ", " + posZ.ToString() +
                    "\nKecepatan Angin: " + SimulationData.nilaiAngin.ToString();

                //perubahan posisi objek
                transform.position = pos;

                //instantiate atau generate prefab/3d ballLog sebagai indikator perjalanan peluru
                GameObject newLog = Instantiate(ballLog, pos, Quaternion.identity);
                newLog.transform.parent = parentBallLog.transform;

                //increment 0.01f untuk variabel waktu
                t += 0.01f;
            } else
            {
                //kondisi peluru apakah sudah menyentuh tanah RWT atau belum
                //ubah nilai SimulationData.startMove false agar peluru tidak berlanjut ke perulangan selanjutnya
                SimulationData.startMove = false;
                Debug.LogWarning("Simulasi Selesai");
            }
        }
    }

    float PosisiX(float t, float v, float sudut, bool angin, float nilaiAngin)
    {
        /* t = waktu
         * v = kecepatan peluru
         * sudut = sudut tembak
         * =$B$2*A8*COS($B$1*PI()/180)
         *      $B$2 = V0 (m/s) atau `v`
         *      A8 = waktu (s) atau `t`
         *      $B$1 = sudut teta (degree) atau `sudut`
         * pengaruh angin berlaku untuk posisi X, dengan referensi jurnal https://ifory.id/proceedings/2019/zx2pyYReP/skf_2019_dela_maratul_kamilah_mib5qxow4c.pdf
         */
        float result = 0;
        if(angin)
        {
            result = v * t * Mathf.Cos(sudut * Mathf.PI / 180) + (nilaiAngin * t);
        } else
        {
            result = v * t * Mathf.Cos(sudut * Mathf.PI / 180);
        }
        return result;
    }

    float PosisiY(float t, float v, float sudut)
    {
        /* t = waktu
         * v = kecepatan peluru
         * sudut = sudut tembak
         * =($B$2*A8*SIN($B$1*PI()/180))-(0.5*$B$3*POWER(A8,2))
         *      $B$2 = V0 (m/s) atau `v`
         *      A8 = waktu (s) atau `t`
         *      $B$1 = sudut teta (degree) atau `sudut`
         */
        return (v * t * Mathf.Sin(sudut * Mathf.PI / 180)) - (0.5f * SimulationData.gravitasi * Mathf.Pow(t, 2));
    }
}
