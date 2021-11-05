using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class PlayerListItem : MonoBehaviourPunCallbacks
{
    Player player;
    public Text text;

    /// <summary>
    /// Create player name in the list
    /// </summary>
    public void Setup(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    /// <summary>
    /// Removes the player from the list, when player leaves
    /// </summary>
    /// <param name="otherPlayer">Player who left</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

}
