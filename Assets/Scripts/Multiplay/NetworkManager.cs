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
    [SerializeField] GameObject waitingScreen;
    int noPlayerCalled = 0;
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
            //    Debug.Log(PhotonNetwork.PlayerListOthers[i].NickName + " has number: " + i);
            otherPlayers[i].GetComponent<OtherPlayerController>().actorId = PhotonNetwork.PlayerListOthers[i].ActorNumber;
            // Debug.Log(" UniquePlayer ID: " + PhotonNetwork.PlayerListOthers[i].UserId);
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
        //gm.GameResult(true);
        pv.RPC("Call", RpcTarget.OthersBuffered);
        waitingScreen.SetActive(true);
        noPlayerCalled += 1;
        checkEveryoneCalled();
    }

    [PunRPC]
    public void Call(PhotonMessageInfo info)
    {
        // Debug.Log("Actor Number : " + info.Sender.ActorNumber);

        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (info.Sender.ActorNumber == otherPlayers[i].GetComponent<OtherPlayerController>().actorId)
            {
                // Debug.Log("Matching Player found");
                noPlayerCalled += 1;
                otherPlayers[i].GetComponent<OtherPlayerController>().Called();
                break;
            }
        }
        checkEveryoneCalled();
    }

    void checkEveryoneCalled()
    {
        if (noPlayerCalled == PhotonNetwork.PlayerList.Length)
        {
            Debug.Log("Everyone has Called!");
            drawResult();
        }
    }
    void drawResult()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("This is MASTER Client!");
            bool hasRedWon = (Random.value > 0.5f);
            pv.RPC("Results", RpcTarget.AllBuffered, hasRedWon);
        }
    }

    [PunRPC]
    public void Results(bool hasRedWon, PhotonMessageInfo info)
    {
        Debug.Log("Did Red Win: " + hasRedWon);
        gm.GameResult(hasRedWon);
        ChecKOtherPlayersWin(hasRedWon);
        waitingScreen.SetActive(false);
    }

    void ChecKOtherPlayersWin(bool redWin)
    {

        List<string> winners = new List<string>();
        List<string> losers = new List<string>();
        foreach (GameObject p in otherPlayers)
        {
            string card = p.GetComponent<OtherPlayerController>().playerColor;
            if (redWin && card == "red")
            {
                winners.Add(p.GetComponent<OtherPlayerController>().txtPlayerName.text);
            }
            else if (!redWin && card == "green")
            {
                winners.Add(p.GetComponent<OtherPlayerController>().txtPlayerName.text);
            }
            else
            {
                losers.Add(p.GetComponent<OtherPlayerController>().txtPlayerName.text);
            }
        }
        Debug.Log("winners: ");
        foreach (string name in winners)
        {
            Debug.Log(name);
        }
        Debug.Log("losers: ");
        foreach (string name in losers)
        {
            Debug.Log(name);
        }

    }


}
