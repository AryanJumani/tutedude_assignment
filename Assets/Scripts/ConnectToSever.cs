using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

// Used PunCallbacks to enable Photon
public class ConnectToSever : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // Initializing PUN using default options
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // once it has initialized PUN, it will make the client join a Lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // once it has joined the lobby, the client can then go onto the Lobby scene so as to connect to the correct room.
        SceneManager.LoadScene("Lobby");
    }
}
