using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] Text text;
    public RoomInfo info;

    /// <summary>
    /// Creates Room name in the list of rooms
    /// </summary>
    public void Setup(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }

    public void OnClick()
    {
        ConnectToServer.Instance.JoinRoom(info);
    }

}
