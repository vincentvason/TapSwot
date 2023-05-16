using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;


public class Chat : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject Message;
    public GameObject Content;
    public GameObject chatArea;
    public GameObject notification;
 
    private int messageCount;
    private void Start()
    {
        messageCount = 0;
    }
    public void Update()
    {
       if ( Input.GetKeyDown(KeyCode.Tab))
        {
            if (chatArea.activeInHierarchy == false)
            {
                chatArea.SetActive(true);
            }
            else
            {
                chatArea.SetActive(false);
            }
        }

       if (Input.GetKeyDown(KeyCode.Return))
        {
            if(chatArea.activeInHierarchy == true)
            {
                if (inputField.text != "")
                {
                    SendMessage();
                }
            }
        }
        if (Content.transform.childCount > messageCount && chatArea.activeInHierarchy == false)
        {
     
                notification.SetActive(true);
        
      
        }   
        if(Content.transform.childCount > messageCount && chatArea.activeInHierarchy == true)
        {
            notification.SetActive(false);
            messageCount = Content.transform.childCount;
        }
 
    }

    public void SendMessage()
    {
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All,PhotonNetwork.NickName + " : " + inputField.text);

        inputField.text = "";
      
    }

    [PunRPC]
    public void GetMessage(string msg)
    {
        GameObject M = Instantiate(Message, Vector3.zero, Quaternion.identity, Content.transform);
        M.GetComponent<ChatMessage>().myMessage.text = msg;
    }
}
