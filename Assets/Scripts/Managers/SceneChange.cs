using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour 
{

    public void LoadMainMenu()
    {
        
    }
    public void OnSelectCreateRoom()
    {
        SceneManager.LoadScene(3);
        SelectionManager.instance.SetPlayerSelection("Create");
    }
    public void OnSelectJoinRoom()
    {
        SceneManager.LoadScene(3);
        SelectionManager.instance.SetPlayerSelection("Join");
    }

    public void OnSelectEditProfile()
    {
        SceneManager.LoadScene(2);
        Debug.Log("Player profile screen");
    }

    public void onSelectQuit()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    public void onSelectBacktoMainMenu()

    {
        SceneManager.LoadScene(1);
        Debug.Log("Back to Main menu screen");
    }

   
}
