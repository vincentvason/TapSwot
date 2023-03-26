using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneChange : MonoBehaviour 
{
    public void onSelectQuit()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
   
}
