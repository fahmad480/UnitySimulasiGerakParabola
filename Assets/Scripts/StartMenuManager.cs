using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSceneToSimulation()
    {
        //Ubah scene ke scene simulasi
        SceneManager.LoadScene("SimulationScene");
    }

    public void closeApp()
    {
        //keluar dari aplikasi
        Application.Quit();
    }
}
