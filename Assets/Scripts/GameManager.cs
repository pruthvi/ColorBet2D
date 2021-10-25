using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button btnRed, btnBet;

    public Text txtChip, txtRedChip;

    public int maxChips = 100;
    public int chips;
    public int redValue, greenValue = 0;

    void Start()
    {
        chips = maxChips;
        updateChips();
        updateRedChips();
        Button red = btnRed.GetComponent<Button>();
        red.onClick.AddListener(betRed);
        Button bet = btnBet.GetComponent<Button>();
        bet.onClick.AddListener(betting);
    }
    void chip()
    {
        chips -= 10;
        updateChips();
        if (chips == 0)
        {
            chips = maxChips;
            Debug.Log("Chips refilled");
        }
    }


    void betRed()
    {
        redValue += 10;
        Debug.Log("Red Bet : " + redValue);
        updateRedChips();
        chip();
    }


    void betting()
    {

        bool IsRedWin = (Random.value > 0.5f);
        Debug.Log(" Red Wins : " + IsRedWin);
        if (IsRedWin)
        {
            chips += (redValue * 2);
            updateChips();
            redValue = 0;
            updateRedChips();

        }
        else
        {
            redValue = 0;
            Debug.Log("Player LOST!");
            updateRedChips();

        }

    }


    void updateChips()
    {
        txtChip.text = " Chips : " + chips;
    }
    void updateRedChips()
    {
        txtRedChip.text = "Red Bet : " + redValue;
    }

    void FixedUpdate()
    {

    }

    void Update()
    {

    }
}
