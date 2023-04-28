using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public Photon.Realtime.Player Player { get; private set; }

    public TextMeshProUGUI _nickname;
    public Image _playerColorImg;
    public GameObject isHostIcon;
    //public Image playerAvatar;
    public Sprite[] allAvatars;

    Hashtable playerProperties = new();

    private void Awake()
    {
        //playerAvatar = gameObject.transform.GetChild(0).GetComponent<Image>();
    }


    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _nickname.text = player.NickName;
        _playerColorImg.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        

        if (Player.IsMasterClient)
        {
            isHostIcon.gameObject.SetActive(true);
        }
        else
        {
            isHostIcon.gameObject.SetActive(false);
        }
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
