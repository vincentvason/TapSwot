using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public Photon.Realtime.Player Player { get; private set; }

    public TextMeshProUGUI _nickname;

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _nickname.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (Player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
