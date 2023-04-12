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
