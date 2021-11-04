using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayerController : MonoBehaviour
{
    [SerializeField] Text txtPlayerName;
    [SerializeField] Text txtBet;

    [SerializeField] Image imgChip;

    public int actorId;

    public void SetOtherPlayerName(string name)
    {
        txtPlayerName.text = name;
    }

    public void SetOtherPlayerBet(int value)
    {
        txtBet.text = "Bet : " + value.ToString();
    }

    public void Called(string param1)
    {
        // imgChip.color = Color.green;
        Debug.Log("Called function exe: " + param1);
    }

    public void ColorSelect(string color)
    {
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
