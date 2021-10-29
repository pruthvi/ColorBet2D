using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button btnBet, btnCall;

    public Text txtChip, txtHouseChip, txtBetColor, txtOutOfChips, txtResult;
    public int chips;
    public int maxChips = 100;
    public int betValue = 0;
    public float delay = 2f;
    public bool betRed, betGreen, playerWin = false;

    private cardFlip cf;
    public GameObject selectionCanvas, outofChips, result;
    public Image betColor;


    void Awake()
    {
        Button red = btnBet.GetComponent<Button>();
        red.onClick.AddListener(bet);
        Button call = btnCall.GetComponent<Button>();
        call.onClick.AddListener(calling);
    }


    void Start()
    {
        chips = PlayerPrefs.GetInt("Chips", maxChips);
        updateChips();
        updateHouseChips();

    }
    void chip()
    {
        chips -= 10;
        updateChips();
        if (chips == 0)
        {
            chips = maxChips;
            //    Debug.Log("Chips refilled");
            outofChips.SetActive(true);
            txtChip.text = " Chips : " + chips;
        }
    }


    void bet()
    {
        betValue += 10;
        // Debug.Log("Bet : " + betValue);
        updateHouseChips();
        chip();
    }


    void calling()
    {

        bool drawResult = (Random.value > 0.5f);
        cf = GameObject.FindGameObjectWithTag("Card").GetComponent<cardFlip>();
        cf.FlipCard();

        if (drawResult && betRed || betGreen)
        {
            playerWin = true;
            // Debug.Log("Player Wins!");
            chips += (betValue * 2);
            updateChips();
            betValue = 0;
            updateHouseChips();
        }
        else
        {
            playerWin = false;
            betValue = 0;
            // Debug.Log("Player LOST!");
            updateHouseChips();
        }
        StartCoroutine(showResult(delay));


    }


    void updateChips()
    {
        txtChip.text = " Chips : " + chips;
    }

    void updateHouseChips()
    {
        txtHouseChip.text = "Bet : " + betValue;
    }

    public void playerBetsRed()
    {
        betRed = true;
        betGreen = false;
        betColor.GetComponent<Image>().color = Color.red;
        txtBetColor.text = "You are betting on RED!";
        disableSelection();

    }

    public void playerBetsGreen()
    {
        betRed = false;
        betGreen = true;
        betColor.GetComponent<Image>().color = Color.green;
        txtBetColor.text = "You are betting on Green!";
        disableSelection();

    }

    void disableSelection()
    {
        selectionCanvas.SetActive(false);
    }

    public void outofChipMsg()
    {
        outofChips.SetActive(false);

    }


    IEnumerator showResult(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        result.SetActive(true);
        if (playerWin)
        {
            txtResult.text = " Horray! You have won!";
        }
        else
        {
            txtResult.text = " Sorry! You have lost. Wanna try Again?";
        }


    }
    public void reloadScene()
    {
        PlayerPrefs.SetInt("Chips", chips);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void exit()
    {
        Application.Quit();
    }
}
