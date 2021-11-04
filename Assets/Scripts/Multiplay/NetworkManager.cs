using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager Instance;

    public GameObject otherPlayerPrefab;
    public Transform playerPanel;
    public GameObject[] otherPlayers;
    public Text txtPlayerName;
    private PhotonView pv;
    private GameManager gm;
    void Awake()
    {
        Instance = this;
        pv = PhotonView.Get(this);
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

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
            otherPlayers[i].GetComponent<OtherPlayerController>().actorId = PhotonNetwork.PlayerListOthers[i].ActorNumber;
            Debug.Log(" UniquePlayer ID: " + PhotonNetwork.PlayerListOthers[i].UserId);
            //otherPlayers[i].GetComponent<OtherPlayerController>().SetOtherPlayerBet(0);

            //otherPlayer[i] = PhotonNetwork.PlayerListOthers[i];
        }

        SendBet(gm.betValue);
    }

    public void ColorSelection(string color)
    {
        pv.RPC("CardColor", RpcTarget.OthersBuffered, color);
    }

    [PunRPC]
    public void CardColor(string color, PhotonMessageInfo info)
    {
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (info.Sender.ActorNumber == otherPlayers[i].GetComponent<OtherPlayerController>().actorId)
            {
                otherPlayers[i].GetComponent<OtherPlayerController>().ColorSelect(color);
                break;
            }
        }

        /*        Debug.Log("Actor Number : " + info.Sender.ActorNumber);
                // info.Sender.UserId
                int playerNo;
                if (info.Sender.ActorNumber == 1)
                {
                    playerNo = 0;
                }
                else
                {
                    playerNo = info.Sender.ActorNumber - 2;
                }
                Debug.Log("Player Number : " + playerNo);
          
        otherPlayers[playerNo].GetComponent<OtherPlayerController>().ColorSelect(color);
*/
    }


    public void SendBet(int chips)
    {
        pv.RPC("GetBetValue", RpcTarget.OthersBuffered, chips);
    }

    [PunRPC]
    public void GetBetValue(int betVal, PhotonMessageInfo info)
    {

        // Debug.Log("Actor Number : " + info.Sender.ActorNumber);
        // int playerNo;
        // if (info.Sender.ActorNumber == 1)
        // {
        //     playerNo = 0;
        // }
        // else
        // {
        //     playerNo = info.Sender.ActorNumber - 1;
        // }
        // Debug.Log("Player Number : " + playerNo);

        // otherPlayers[playerNo].GetComponent<OtherPlayerController>().SetOtherPlayerBet(betVal);

        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (info.Sender.ActorNumber == otherPlayers[i].GetComponent<OtherPlayerController>().actorId)
            {
                otherPlayers[i].GetComponent<OtherPlayerController>().SetOtherPlayerBet(betVal);
                break;
            }
        }



    }


    public void CallButtonPressed()
    {
        Debug.Log("Call button pressed");

        pv.RPC("Call", RpcTarget.OthersBuffered, "some data");
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
