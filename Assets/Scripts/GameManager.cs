using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Declarations
    [Header("UI Elements")]
    public Button btnCall;
    public Text txtChip, txtHouseChip, txtResult, txtOutOfChips, txtBetColor;   // UI Text elements to change their value according to gameplay
    /// <summary>
    /// Color bar which shows on which color player is betting
    /// </summary>
    public Image betColor;

    [Header("Chip Counts")]

    /// <summary>
    /// Number of Chips player has
    /// </summary>
    public int chips;
    public int startChips = 100;

    /// <summary>
    /// Total number bets in the current gameplays
    /// </summary>
    public int betValue = 0;

    [SerializeField]
    private float delay = 2f;

    [Header("Bet and Win switches")]
    public bool betRed, betGreen, playerWin = false;

    /// <summary>
    /// Card Flip script on the middle Card gameobject.
    /// </summary>
    [Header("Required Game Objects")]
    public GameObject selectionCanvas, outofChips, result, extraChips;

    private CardFlip cf;

    #endregion

    void Awake()
    {
        Button call = btnCall.GetComponent<Button>();
        call.onClick.AddListener(calling);
    }

    void Start()
    {
        chips = PlayerPrefs.GetInt("Chips", startChips);      // Get the chip number from previous gameplay, if there is none than it will set it to start chips
        if (chips < startChips)     // If chips are less than 100in previous gameplay, then it will topup chips
        {
            chips = startChips;
        }
        UpdateChips();
        UpdateHouseChips();
        ExtraChipCount();
    }

    #region Functions for Card Selection

    // Functions to set player card as Red or Green when player clicks on respective card in the Selection Screen
    public void PlayerBetsRed()
    {
        betRed = true;
        betGreen = false;
        betColor.GetComponent<Image>().color = Color.red;
        txtBetColor.text = "You are betting on RED!";
        DisableSelection();
    }

    public void PlayerBetsGreen()
    {
        betRed = false;
        betGreen = true;
        betColor.GetComponent<Image>().color = Color.green;
        txtBetColor.text = "You are betting on Green!";
        DisableSelection();
    }
    /// <summary>
    /// Disables Selection Canvas so that player can see the Game canvas for betting
    /// </summary>
    void DisableSelection()
    {
        selectionCanvas.SetActive(false);
    }

    #endregion

    #region Functions for Chips
    /// <summary>
    /// Function to update Player Chips counter. Call only when player clicks on a chip and when chip value is reduced.
    /// </summary>
    public void Chip()
    {
        Bet();
        chips -= 10;        // Reduces chip value by 10
        UpdateChips();
        if (chips == 0)     // If Player does not have chips, this will top up with startChips(ie. 100) and displays the message to the player with a canvas
        {
            chips = startChips;
            outofChips.SetActive(true);
            txtChip.text = " Chips : " + chips;
        }
        else if (chips > 100)   // If player has more than 100 chips, this will active ExtraChip
        {
            ActiveExtraChip(true);
            extraChips.GetComponent<ExtraChipController>().CheckExtraChips();
        }
    }

    /// <summary>
    /// Updates Chips text on the gameplay screen
    /// </summary>
    void UpdateChips()
    {
        txtChip.text = " Chips : " + chips;
    }
    /// <summary>
    /// Updates Bet values.
    /// </summary>
    void Bet()
    {
        betValue += 10;
        UpdateHouseChips();
    }

    /// <summary>
    /// Updates Bet text on the gameplay screen
    /// </summary>
    void UpdateHouseChips()
    {
        txtHouseChip.text = "Bet : " + betValue;
    }

    /// <summary>
    /// Function to activate Out of Chips message screen
    /// </summary>
    public void OutOfChipMsg()
    {
        outofChips.SetActive(false);
    }


    /// <summary>
    /// Function to check if extra chips is needed or not
    /// </summary>
    void ExtraChipCount()
    {
        if (chips <= 100)
        {
            ActiveExtraChip(false);
        }
        else if (chips > 100)
        {
            ActiveExtraChip(true);
            extraChips.GetComponent<ExtraChipController>().CheckExtraChips();
        }
    }

    /// <summary>
    /// Function to activate and deactivate extra chips gameobject
    /// </summary>
    public void ActiveExtraChip(bool active)
    {
        extraChips.SetActive(active);
    }
    #endregion

    #region Functions on Call
    // Function when call button is pressed
    void calling()
    {
        bool drawResult = (Random.value > 0.5f);        // Draws random true, false value for winning
        cf = GameObject.FindGameObjectWithTag("Card").GetComponent<CardFlip>();
        cf.FlipCard();

        if (drawResult && betRed || betGreen)   // Checks if player has won
        {
            playerWin = true;
            chips += (betValue * 2);    // Double the chip value
            UpdateChips();
            betValue = 0;               // Resets bet value for next gameplay
            UpdateHouseChips();
        }
        else
        {
            playerWin = false;
            betValue = 0;
            UpdateHouseChips();
        }
        StartCoroutine(showResult(delay));  // Shows result screen
    }

    /// <summary>
    /// Coroutine function for displaying Result Screen canvas
    /// </summary>
    /// <param name="delay">Delay time when Result screen should be displayed. Value set based on Card flip animation.</param>
    /// <returns></returns>
    IEnumerator showResult(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        result.SetActive(true);     // Activates the canvas
        if (playerWin)
        {
            txtResult.text = "Horray! You have won!";
        }
        else
        {
            txtResult.text = "Sorry! You have lost. Wanna try Again?";
        }
    }
    #endregion

    // Reloads the Scene when Play Again button is clicked
    public void ReloadScene()
    {
        PlayerPrefs.SetInt("Chips", chips);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // Function to Exit the game
    public void Exit()
    {
        Application.Quit();
    }

}
