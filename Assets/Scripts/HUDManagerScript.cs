using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InfinityCode.RealWorldTerrain;

/// <summary>
/// script untuk handel semua data yang ada di hud
/// koneksi variable ke class lainnya
/// </summary>

public class HUDManagerScript : MonoBehaviour
{
    [HideInInspector] public float kecepatanPeluru;
    [HideInInspector] public float gravitasi;
    [HideInInspector] public float sudutTembak;
    [HideInInspector] public string namaTarget;
    [HideInInspector] public double _latittude;
    [HideInInspector] public double _longitude;
    [HideInInspector] public float kecepatanAngin;
    [HideInInspector] public bool pengaruhAngin;
    [HideInInspector] public int camChange = 0;

    [SerializeField] InputField _ifKecepatanPeluru;
    [SerializeField] InputField _ifGravitasi;
    [SerializeField] InputField _ifSudutTembak;
    [SerializeField] InputField _ifNamaTarget;
    [SerializeField] InputField _ifLongitude;
    [SerializeField] InputField _ifLatittude;
    [SerializeField] Toggle _tglPengaruhAngin;
    [SerializeField] InputField _ifKecepatanAngin;
    [SerializeField] GameObject meriam;
    [SerializeField] GameObject moncongMeriam;
    [SerializeField] GameObject cannonBall;
    [SerializeField] RealWorldTerrainContainer container;
    [SerializeField] AudioClip cannonShotAudioClip;
    [SerializeField] AudioSource cannonShotAudioSource;
    [SerializeField] Text infoText;

    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject mapCamera;
    [SerializeField] GameObject followCamera;

    // Start is called before the first frame update
    void Start()
    {
        //Prepare awal kamera, atur mainCamera aktif, sisanya tidak
        mainCamera.SetActive(true);
        mapCamera.SetActive(false);
        followCamera.SetActive(false);

        //Inisialisasi data dari hud ke class SimulationData
        SimulationData.cannonBall = cannonBall;
        SimulationData.container = container;
        SimulationData.meriam = meriam;
        SimulationData.moncongMeriam = moncongMeriam;

        //input data default dari class ke UI
        _ifKecepatanPeluru.text = SimulationData.kecepatanPeluru.ToString();
        _ifGravitasi.text = SimulationData.gravitasi.ToString();
        _ifSudutTembak.text = SimulationData.sudutTembak.ToString();
        _ifNamaTarget.text = SimulationData.namaTarget;
        _ifLatittude.text = SimulationData.targetLatitude.ToString();
        _ifLongitude.text = SimulationData.targetLongitude.ToString();
        _ifKecepatanAngin.text = SimulationData.nilaiAngin.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCamera()
    {
        //nilai kondisi untuk pengaturan kamera
        camChange += 1;

        //jika nilai camChange 3 maka kembali ke nilai 0
        if (camChange == 3)
        {
            camChange = 0;
        }
        if(camChange == 0)
        {
            //aktifkan kamera utama
            mainCamera.SetActive(true);
            mapCamera.SetActive(false);
            followCamera.SetActive(false);
        }
        else if(camChange == 1)
        {
            //aktifkan kamera peta
            mainCamera.SetActive(false);
            mapCamera.SetActive(true);
            followCamera.SetActive(false);
        } else
        {
            //aktifkan kamera pengikut
            mainCamera.SetActive(false);
            mapCamera.SetActive(false);
            followCamera.SetActive(true);
        }
    }

    public void Tembak()
    {
        // store nilai dari input field ke variable
        kecepatanPeluru = float.Parse(_ifKecepatanPeluru.text);
        gravitasi = float.Parse(_ifGravitasi.text);
        sudutTembak = float.Parse(_ifSudutTembak.text);
        namaTarget = _ifNamaTarget.text;
        _latittude = float.Parse(_ifLatittude.text);
        _longitude = float.Parse(_ifLongitude.text);
        kecepatanAngin = float.Parse(_ifKecepatanAngin.text);
        pengaruhAngin = _tglPengaruhAngin.isOn;

        //reset rotasi moncong meriam
        SimulationData.moncongMeriam.transform.Rotate(0f, -SimulationData.sudutTembak, 0f);

        //call fungsi TampilkanInformasi()
        TampilkanInformasi();

        //konversi longlat target ke V3
        SimulationData.posisiTarget = ConversHelper.ConvertToWorldPos(SimulationData.targetLongitude, SimulationData.targetLatitude, SimulationData.container);

        //konversi linglat meriam
        SimulationData.posisiMeriam = ConversHelper.GetWorldPosFromGameObject(SimulationData.meriam);

        //menghitung jarak meriam ke musuh/target
        double Xmax = ConversHelper.UkurJarak(SimulationData.posisiTarget, SimulationData.posisiMeriam);

        //menghitung sudut elevasi meriam
        double sudut = ConversHelper.ConvertToDegree((float)Xmax, 472);

        Debug.Log("Posisi Musuh: " + SimulationData.posisiTarget +
            "\nPosisi Meriam : " + SimulationData.posisiMeriam +
            "\nJarak: " + Xmax +
            "\nSudut Elevasi: " + sudut);

        //LookAt meriam ke target
        SimulationData.meriam.transform.LookAt(SimulationData.posisiTarget);

        //atur rotasi moncong meriam sesuai sudutTembak / derajat tembak
        SimulationData.moncongMeriam.transform.Rotate(0f, SimulationData.sudutTembak, 0f);

        //set Cannon Ball Position
        SimulationData.cannonBall.transform.position = ConversHelper.GetWorldPosFromGameObject(GameObject.Find("MoncongMeriam"));
        Debug.Log(SimulationData.cannonBall.transform.position);

        //LookAt CannonBall ke targe
        SimulationData.cannonBall.transform.LookAt(SimulationData.posisiTarget);

        //delete semua child ballLog
        Transform parentBallLogTransform = SimulationData.parentBallLog.transform;
        foreach (Transform child in parentBallLogTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //Mulai gerakan bola meriam
        AmmoBehaviour.t = 0; //jika variabel t sudah berisi sebelumnya, maka untuk memastikan simulasi mulai dari awal, nilai dikembalikan jadi 0
        SimulationData.startMove = true; //merubah nilai SimulationData.startMove jadi true agar simulasi/peluru dapat bergerak
        cannonShotAudioSource.PlayOneShot(cannonShotAudioClip, 1f); //memutar sfx tembakan
    }

    private void TampilkanInformasi()
    {
        // simpan semua variabel ke dalam class SimulationData
        SimulationData.kecepatanPeluru = kecepatanPeluru;
        SimulationData.gravitasi = gravitasi;
        SimulationData.sudutTembak = sudutTembak;
        SimulationData.namaTarget = namaTarget;
        SimulationData.targetLatitude = _latittude;
        SimulationData.targetLongitude = _longitude;
        SimulationData.pengaruhAngin = pengaruhAngin;
        SimulationData.infoText = infoText;
        SimulationData.nilaiAngin = kecepatanAngin;

        Debug.Log("Kecepatan peluru:" + kecepatanPeluru + 
            "\ngravitasi:" + gravitasi + 
            "\nsudut tembak:" + sudutTembak + 
            "\npengaruh angin:" + pengaruhAngin);
        Debug.Log("nama target:" + namaTarget +
            "\nlatittude target: " + _latittude +
            "\nlongitude target:" + _longitude);
    }

}
