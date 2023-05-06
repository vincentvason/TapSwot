using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class Username : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public string username;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SaveUsername()
    {
        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString("Username", usernameInput.text);

        username = usernameInput.text;

    }
}
