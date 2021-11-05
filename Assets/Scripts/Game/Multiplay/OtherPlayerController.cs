using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayerController : MonoBehaviour
{
    public Text txtPlayerName;
    [SerializeField] Text txtBet;

    [SerializeField] Image imgChip;
    [SerializeField] Image imgBG;

    public bool playerCalled = false;
    public string playerColor;

    public int actorId;

    #region Player Name
    /// <summary>
    /// Sets the name of this player
    /// </summary>
    public void SetOtherPlayerName(string name)
    {
        txtPlayerName.text = name;
    }
    #endregion

    #region Player Color
    /// <summary>
    /// Sets Color on which this player is betting on
    /// </summary>
    public void ColorSelect(string color)
    {
        playerColor = color;
        if (color == "red")
        {
            imgChip.color = Color.red;
        }
        else if (color == "green")
        {
            imgChip.color = Color.green;
        }
        else { Debug.Log("Error in Color Selection RPC"); }
    }
    #endregion

    #region Player Bet
    /// <summary>
    /// Sets the Bet value of this player
    /// </summary>
    /// <param name="value">Bet value</param>
    public void SetOtherPlayerBet(int value)
    {
        txtBet.text = "Bet : " + value.ToString();
    }
    #endregion

    #region Player Called
    /// <summary>
    /// Creates black background on the player who called
    /// </summary>
    public void Called()
    {
        var tempColor = imgBG.color;
        tempColor.a = 1f;
        imgBG.color = tempColor;
        playerCalled = true;
    }
    #endregion


}
