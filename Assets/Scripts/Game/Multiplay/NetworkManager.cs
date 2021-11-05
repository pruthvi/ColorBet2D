using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager Instance;

    [Header("Other Player Panel")]
    public Transform playerPanel;
    public GameObject otherPlayerPrefab;
    public GameObject[] otherPlayers;

    [Header("Player Name")]
    public Text txtPlayerName;


    private PhotonView pv;
    private GameManager gm;

    [Header("Waiting Screen")]
    [SerializeField] GameObject waitingScreen;
    int noPlayerCalled = 0; // Counted to check how many players called

    void Awake()
    {
        Instance = this;
        pv = PhotonView.Get(this);
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Start()
    {
        txtPlayerName.text = PhotonNetwork.NickName;                    // Sets player name

        otherPlayers = new GameObject[PhotonNetwork.PlayerListOthers.Length];

        // Creates the player list on the right sidebar
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            otherPlayers[i] = Instantiate(otherPlayerPrefab, playerPanel);
            otherPlayers[i].GetComponent<OtherPlayerController>().SetOtherPlayerName(PhotonNetwork.PlayerListOthers[i].NickName);
            otherPlayers[i].GetComponent<OtherPlayerController>().actorId = PhotonNetwork.PlayerListOthers[i].ActorNumber;
        }

        SendBet(gm.betValue);
    }

    /// <summary>
    /// Sends the information about the Player Color Selection to the network
    /// </summary>
    /// <param name="color">Color Selected by the player</param>
    public void ColorSelection(string color)
    {
        pv.RPC("CardColor", RpcTarget.OthersBuffered, color);
    }

    // Gets the RPC call about a player selected color
    [PunRPC]
    public void CardColor(string color, PhotonMessageInfo info)
    {
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (info.Sender.ActorNumber == otherPlayers[i].GetComponent<OtherPlayerController>().actorId)   // Finds which player send the RPC call
            {
                otherPlayers[i].GetComponent<OtherPlayerController>().ColorSelect(color);   // Sets their chip color to the respective color that player is betting on
                break;
            }
        }
    }


    /// <summary>
    /// Sends the RPC call to the network updated bets player did
    /// </summary>
    /// <param name="chips">New Bet value</param>
    public void SendBet(int chips)
    {
        pv.RPC("GetBetValue", RpcTarget.OthersBuffered, chips);
    }

    // Receives Bet value from the network
    [PunRPC]
    public void GetBetValue(int betVal, PhotonMessageInfo info)
    {
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (info.Sender.ActorNumber == otherPlayers[i].GetComponent<OtherPlayerController>().actorId)   // Finds player whose bet value was updated
            {
                otherPlayers[i].GetComponent<OtherPlayerController>().SetOtherPlayerBet(betVal);    // Updates the value next to that player
                break;
            }
        }
    }


    // Function to execute when player clicks on call button
    public void CallButtonPressed()
    {
        pv.RPC("Call", RpcTarget.OthersBuffered);   // Sends information to the network that this player has called.
        waitingScreen.SetActive(true);              // Opens waiting screen, while other players are still betting
        noPlayerCalled += 1;                        // Updates call counter 
        CheckEveryoneCalled();
    }

    [PunRPC]
    public void Call(PhotonMessageInfo info)
    {
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (info.Sender.ActorNumber == otherPlayers[i].GetComponent<OtherPlayerController>().actorId)
            {
                noPlayerCalled += 1;
                otherPlayers[i].GetComponent<OtherPlayerController>().Called();
                break;
            }
        }
        CheckEveryoneCalled();
    }
    /// <summary>
    /// Function which checks if all the players have called
    /// </summary>
    void CheckEveryoneCalled()
    {
        if (noPlayerCalled == PhotonNetwork.PlayerList.Length)
        {
            Debug.Log("Everyone has Called!");
            DrawResult();
        }
    }


    /// <summary>
    /// Draws probablity result on the MasterClient and send the result to everyone on the network
    /// </summary>
    void DrawResult()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            bool hasRedWon = (Random.value > 0.5f);                 // Determines probability of Red color winning
            pv.RPC("Results", RpcTarget.AllBuffered, hasRedWon);
        }
    }


    // Receives result from the network
    [PunRPC]
    public void Results(bool hasRedWon, PhotonMessageInfo info)
    {
        gm.GameResult(hasRedWon);               // Executes result on the Card in the center of the game
        ChecKOtherPlayersWin(hasRedWon);
        waitingScreen.SetActive(false);         // Disables the waiting screen
    }

    /// <summary>
    /// Checks which other players have won and lost
    /// </summary>
    /// <param name="redWin">Did red color win?</param>
    void ChecKOtherPlayersWin(bool redWin)
    {
        // Creates list of winners and losers
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
