using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class spawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        // spawns a player in the center of the screen on the start of the MainScene scene
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }
}
