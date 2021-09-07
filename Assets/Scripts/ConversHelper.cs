using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfinityCode.RealWorldTerrain;

public class ConversHelper 
{
    public static float xMax(float v, float sudut, float gravitasi)
    {
        /*
         * Mendapatkan jangkauan maksimum
         * =(2*POWER(B2,2)*SIN(B1*PI()/180)*COS(B1*PI()/180))/B3
         */
        return (2 * Mathf.Pow(v, 2) * Mathf.Sin(sudut * Mathf.PI / 180) * Mathf.Cos(sudut * Mathf.PI / 180)) / gravitasi;
    }

    public static float hMax(float v, float sudut, float gravitasi)
    {
        /*
         * Mendapatkan ketinggian maksimum
         * =(POWER(B2,2)*POWER(SIN(B1*PI()/180),2))/(2*B3)
         */
        return (Mathf.Pow(v, 2) * Mathf.Pow(Mathf.Sin(sudut * Mathf.PI / 180), 2)) / (2 * gravitasi);
    }

    public static float timeHMax(float v, float sudut, float gravitasi)
    {
        /*
         * Mendapatkan waktu saat ketinggian maksimum
         * =($B$2*SIN($B$1*PI()/180)/$B$3)
         */
        return (v * Mathf.Sin(sudut * Mathf.PI / 180)) / gravitasi;
    }

    public static float travelingTime(float v, float sudut, float gravitasi)
    {
        /*
         * Mendapatkan lama waktu tempuh
         * =($B$2*SIN($B$1*PI()/180)/$B$3)
         */
        return timeHMax(v, sudut, gravitasi) * 2;
    }

    public static Vector3 GetWorldPosFromGameObject(GameObject _GameObject)
    {
        /*
         * Mendapatkan nilai World Pos dari GameObject
         */

        return _GameObject.transform.position;
    }
    public static Vector3 ConvertToWorldPos(double _long,double _lat, RealWorldTerrainContainer _container)
    {
        /*
         * Mendapatkan nilai WorldPos berdasarkan longitude dan latitude dengan syarat RWT
         */

        Vector3 pos = Vector3.zero;

        _container.GetWorldPosition(_long, _lat, out pos);

        return pos;
    }

    public static Vector2 ConvertToGeoPos(Vector3 _v3, RealWorldTerrainContainer _container)
    {
        /*
         * Ubah nilai vector3 jadi nilai longitude dan latitude dengan syarat RWT
         */

        Vector2 pos = Vector2.zero;
        _container.GetCoordinatesByWorldPosition(_v3, out pos);
        return pos;
    }

    public static float ConvertToDegree(float _distance, float _kecepatan)
    {
        /*
         * Ubah jarak dan kecepatan menjadi derajat
         */
        return (Mathf.Asin(_distance * 10 / (Mathf.Pow(_kecepatan, 2)) * Mathf.Rad2Deg)) / 2;
    }

    public static double ConvertToDegree(float _rad)
    {
        /*
         * ubah radius jadi derajat
         */
        return Mathf.Asin(_rad) * Mathf.Rad2Deg;
    }

    public static double UkurJarak(Vector3 a, Vector3 b)
    {
        /*
         * Untuk mendapatkan jarak antara a dan b
         */ 
        float _a = b.x - a.x;
        float _b = a.y - a.y;

        return Mathf.Sqrt(Mathf.Pow(_a, 2) + Mathf.Pow(_b, 2));
    }
}
