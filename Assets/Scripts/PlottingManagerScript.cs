using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using InfinityCode.RealWorldTerrain;
[System.Serializable]

public class ObjectList
{
    //inisialisasi class ObjectList untuk menampung kelompok variabel dengan tipe ObjectList
    public string nama;
    public double _lattitude;
    public double _longitude;
    public Vector3 _pos;
    public GameObject _prefab;
}

public class PlottingManagerScript : MonoBehaviour
{
    [Serializable]
    public class DataSet
    {
        //inisialisasi class DataSet untuk menampung lagi variabel variabel dengan class ObjectList dalam bentuk array/list
        public ObjectList[] ObjectList;
    }

    //deklarasi berkas text atau *.txt untuk diinput pada unity
    [SerializeField] public TextAsset dataList;

    //deklarasi list/array kumpulan variabel ObjectList
    [SerializeField] List<ObjectList> ObjectList = new List<ObjectList>();

    // Start is called before the first frame update
    void Start()
    {
        //read data json pada file *.txt
        DataSet ObjectData = JsonUtility.FromJson<DataSet>(dataList.text);

        //perulangan berdasarkan panjang path json
        for (int i = 0; i < ObjectData.ObjectList.Length; i++)
        {
            // inisialisasi class ObjectList dengan variabel objectInfo
            ObjectList objectInfo = new ObjectList()
            {
                nama = ObjectData.ObjectList[i].nama,
                _lattitude = ObjectData.ObjectList[i]._lattitude,
                _longitude = ObjectData.ObjectList[i]._longitude
            };

            // konversi lattitude dan longitude jadi world position berdasarkan container RWT
            Vector3 pos = Vector3.zero;
            SimulationData.container.GetWorldPosition(objectInfo._longitude, objectInfo._lattitude, out pos);
            objectInfo._pos = pos;

            // konversi string prefab ke GameObject prefab
            GameObject prefab = GameObject.Find(ObjectData.ObjectList[i]._prefab.ToString());
            objectInfo._prefab = prefab;

            // push data ke class
            ObjectList.Add(objectInfo);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}