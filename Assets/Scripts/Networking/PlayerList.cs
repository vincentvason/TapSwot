using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayerList : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerInfo _playerlist;

    private List<PlayerInfo> _listing = new List<PlayerInfo> ();


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PlayerInfo listing = Instantiate(_playerlist, _content);
       
   
            listing.SetPlayerInfo(newPlayer);
            _listing.Add(listing);
            
       
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        int index = _listing.FindIndex(x => x.Player == otherPlayer);
        if(index != -1)
        {
            Destroy(_listing[index].gameObject);
            _listing.RemoveAt(index);
        }

    }

}
