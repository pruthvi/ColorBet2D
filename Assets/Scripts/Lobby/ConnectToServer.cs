using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    public static ConnectToServer Instance;
    [SerializeField] InputField inputPlayerName;
    [SerializeField] InputField roomNameInput;
    [SerializeField] Text txtRoomName;
    [SerializeField] int maxPlayers = 6;

    [SerializeField] GameObject btnStartGame;
    [SerializeField] Text txtError;

    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;


    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();        // Connects to the Master
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();        // Connected to Master
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("Title");         // Joins the game to Lobby
        PhotonNetwork.NickName = "Player" + Random.Range(1, 10).ToString("00");
    }

    public void SetPlayerName()
    {
        if (string.IsNullOrEmpty(inputPlayerName.text))
        {
            Debug.Log("Player Name is empty");
            return;
        }
        PhotonNetwork.NickName = inputPlayerName.text;
    }

    // #region Create Room
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            Debug.Log("Room name is empty");
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayers;
        PhotonNetwork.CreateRoom(roomNameInput.text, options);

        LoadingMenu();   //prevents player from clicking anything while room is being created at server
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        txtError.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("Error");
    }
    // #endregion

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
        }
    }


    // #region Join Room
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        LoadingMenu();
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Room");
        txtRoomName.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        if (PhotonNetwork.PlayerList.Length == maxPlayers)
        {
            StartGame();
        }
        btnStartGame.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        btnStartGame.SetActive(PhotonNetwork.IsMasterClient);
    }

    // #endregion


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        LoadingMenu();
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Title");
    }


    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Main");
    }


    /// <summary>
    /// Opens Loading menu. It prevents player from clicking anything while network is connecting and processing.
    /// </summary>
    void LoadingMenu()
    {
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void ExiGame()
    {
        Application.Quit();
    }

}
