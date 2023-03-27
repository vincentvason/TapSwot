using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public Photon.Realtime.Player Player { get; private set; }

    public TextMeshProUGUI _nickname;

    bool calledOnce = false;

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _nickname.text = player.NickName;
    }

    //[PunRPC]
    //void LoadLevel()
    //{   if (calledOnce) return;
    //    if(SceneManager.GetActiveScene().name != "MainGame2")
    //    {
    //        PhotonNetwork.LoadLevel("MainGame2");
    //        calledOnce = true;
    //    }
    //}
}
