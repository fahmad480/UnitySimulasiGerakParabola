using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InfinityCode.RealWorldTerrain;

public class SimulationData
{
    public static bool startMove = false; //boolean untuk memulai pergerakan cannonBall

    public static GameObject meriam; //GameObject senjata/meriam
    public static GameObject moncongMeriam; //GameObject moncong senjata/meriam
    public static Vector3 posisiMeriam; //posisi WorldPosition senjata/meriam
    public static GameObject cannonBall; //GameObject peluru senjata/meriam

    public static float kecepatanPeluru = 7.7f; //kecepatan peluru dalam satuan m/s
    public static float gravitasi = 9.8f; //nilai gravitiasi, default 10
    public static float sudutTembak = 0f; //sudut tembak dalam derajat

    public static string namaTarget = "Gedung 1"; //nama target gedung
    public static double targetLatitude = -6.8972920; //latitude dunia nyata posisi gedung
    public static double targetLongitude = 107.6363552; //longitude dunia nyata posisi gedung
    public static Vector3 posisiTarget; //posisi WorldPosition gedung/target penembakan

    public static bool pengaruhAngin = true; //boolean pengaruh angin atau tidak, nilai random
    public static float nilaiAngin = Random.Range(-2f, 2f); //generate random float untuk kecepatan angin dalam satuan m/s dengan range dari -2 m/s sampai 2 m/s
                                                            //pengaruh angin hanya untuk vector X saja

    public static Text infoText; //object text pada UI unity

    public static RealWorldTerrainContainer container = GameObject.FindObjectOfType<RealWorldTerrainContainer>(); //GameObject RealWordTerrain, sudah di ada default

    public static GameObject ballLog; //object untuk log perjalanan peluru
    public static GameObject parentBallLog; //object parent untuk semua object log perjalanan peluru
}
