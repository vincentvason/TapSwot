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

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _nickname.text = player.NickName;
    }
}
