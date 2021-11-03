using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayerController : MonoBehaviour
{
    [SerializeField] Text txtPlayerName;
    [SerializeField] Image imgChip;


    public void SetOtherPlayerName(string name)
    {
        txtPlayerName.text = name;
    }

    public void Called(string param1)
    {
        imgChip.color = Color.green;
        Debug.Log("Called function exe: " + param1);
    }


}
