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
    [SerializeField] Text txtError;

    [Header("Room Screen")]
    [SerializeField] Text txtRoomName;
    [SerializeField] int maxPlayers = 6;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject btnStartGame;

    [Header("Player Screen")]
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

    #region Server Connects 
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();        // Connected to Master
        PhotonNetwork.AutomaticallySyncScene = true;        // Sync Scene for all players in the room
    }

    public override void OnJoinedLobby()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))      // Checks if player has set name before
        {
            MenuManager.Instance.OpenMenu("Player Name");   // Opens menu to set player name
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");   // Get playername stored in PlayerPrefs
            MenuManager.Instance.OpenMenu("Title");
        }
    }

    public void SetPlayerName()
    {
        if (string.IsNullOrEmpty(inputPlayerName.text))     // Error handling if player name is blank
        {
            Error("Player Name can not be blank! ");
            return;
        }
        PhotonNetwork.NickName = inputPlayerName.text;              // Sets Photon player name
        PlayerPrefs.SetString("PlayerName", inputPlayerName.text);  // Sets PlayerPrefs
        MenuManager.Instance.OpenMenu("Title");
    }
    #endregion

    #region Create Room
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            Error("Room Name is empty ");       // Error handling if room name is empty
            return;
        }

        RoomOptions options = new RoomOptions();    // Creates room options for maximum players in the room
        options.MaxPlayers = (byte)maxPlayers;

        PhotonNetwork.CreateRoom(roomNameInput.text, options);  // Creates room

        LoadingMenu();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Error("Room Creation Failed: " + message);  // Error handling if Room is failed to create
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)    // Clears Room List
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)    // Updates room list if remove is not longer available
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);   // Creates Room List
        }
    }

    #endregion

    #region Join Room
    public void JoinRoom(RoomInfo info)     // Join button event
    {
        PhotonNetwork.JoinRoom(info.Name);
        LoadingMenu();
    }


    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Room");
        txtRoomName.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;    // Gets all the players in the room
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);  // Removes player in the list when left
        }

        for (int i = 0; i < players.Length; i++)        // Creates List of players
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        if (PhotonNetwork.PlayerList.Length == maxPlayers)  // Checks if room is full
        {
            Debug.Log("Room is full");
        }
        btnStartGame.SetActive(PhotonNetwork.IsMasterClient);   // If this player is MasterClient than StartGame button is displayed
    }

    // If MasterClient leaves the room, MasterClient is switched to another player
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        btnStartGame.SetActive(PhotonNetwork.IsMasterClient);      // Enables StartGame button, if the player is the new MasterClient
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);   // Adds player to the list of players in the room
    }

    #endregion

    #region Leave Room
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();  // Updates the network that the player has left the room
        LoadingMenu();
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Title");     // Opens main menu screen when player leaves the room
    }
    #endregion

    #region Common Functions
    /// <summary>
    /// Load Game Scene
    /// </summary>
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Main");
    }


    /// <summary>
    /// Opens Error Menu to display error handling
    /// </summary>
    /// <param name="errorMsg">Custom Message to display</param>
    void Error(string errorMsg)
    {
        txtError.text = errorMsg;
        MenuManager.Instance.OpenMenu("Error");
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
    #endregion

}
