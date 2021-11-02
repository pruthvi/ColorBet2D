using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public InputField inputName;
    public Button btnName;


    // public static LobbyManager instance;

    // private void Awake()
    // {
    //     if (instance != null && instance != this)
    //     {
    //         gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
        Debug.Log("Room Created!");
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Multiplay");
    }
    // public override void OnConnectedToMaster()
    // {
    //     base.OnConnectedToMaster();
    // }

    public void SetName()
    {
        PhotonNetwork.NickName = inputName.text;
        Debug.Log("NickName Set to :" + PhotonNetwork.NickName);
    }
    public void ActivateSetNameButton()
    {
        if (inputName.text.Length > 2)
        {
            btnName.interactable = true;
        }
        else
        {
            btnName.interactable = false;
        }
    }

}
