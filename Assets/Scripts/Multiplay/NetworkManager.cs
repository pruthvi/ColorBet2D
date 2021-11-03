using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{


    public GameObject otherPlayerPrefab;
    public Transform playerPanel;
    public GameObject[] otherPlayers;
    public Text txtPlayerName;

    void Start()
    {
        // Debug.Log("Photon Players: " + PhotonNetwork.PlayerListOthers.Length);

        txtPlayerName.text = PhotonNetwork.NickName;

        otherPlayers = new GameObject[PhotonNetwork.PlayerListOthers.Length];

        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            otherPlayers[i] = Instantiate(otherPlayerPrefab, playerPanel);
            otherPlayers[i].GetComponent<OtherPlayerController>().SetOtherPlayerName(PhotonNetwork.PlayerListOthers[i].NickName);
            Debug.Log(PhotonNetwork.PlayerListOthers[i].NickName + " has number: " + i);
            //otherPlayer[i] = PhotonNetwork.PlayerListOthers[i];
        }

        // foreach (PhotonPlayer p in PhotonNetwork.playerList)
        // {
        //     string nickName = p.NickName;
        // }


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallButtonPressed()
    {
        Debug.Log("Call button pressed");
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Call", RpcTarget.OthersBuffered, "some data");
    }

    [PunRPC]
    public void Call(string param1, PhotonMessageInfo info)
    {
        int playerNo = info.Sender.ActorNumber - 1;
        Debug.Log("Actor Number : " + info.Sender.ActorNumber);
        Debug.Log("Player Number : " + playerNo);

        Debug.Log("Actor Name : " + info.Sender.NickName);


        otherPlayers[playerNo].GetComponent<OtherPlayerController>().Called(param1);

    }


}
