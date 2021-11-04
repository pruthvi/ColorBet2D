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

    public void SetOtherPlayerName(string name)
    {
        txtPlayerName.text = name;
    }

    public void SetOtherPlayerBet(int value)
    {
        txtBet.text = "Bet : " + value.ToString();
    }

    public void Called()
    {
        //  Debug.Log("A Player called! ");
        var tempColor = imgBG.color;
        tempColor.a = 1f;
        imgBG.color = tempColor;
        playerCalled = true;
        //imgBG.color.a = 1f;
        // imgBG.canvasRenderer.SetAlpha(0.01f);
        // imgBG.CrossFadeAlpha(1, 2.0f, false);

    }

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



}
