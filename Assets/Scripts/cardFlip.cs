using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Script to flip card when "Call' button is clicked and reveals outcome card. Scripts uses animation to flip card and Instantiate a new card with outcome color.
/// </summary>
public class CardFlip : MonoBehaviour
{
    private Animator anim;      // Gets Animator component to change its parameters
    public GameObject blankCard;    // Gets a blank card prefab for new card
    private GameManager gm;
    [SerializeField] Transform parent;
    void Awake()
    {
        anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    /// <summary>
    /// Function to Flip the Card with animation
    /// </summary>
    public void FlipCard()
    {
        anim.SetBool("Flip", true);     // Sets boolen parameter value in the animator so that transition occurs and Flip animation is played.
    }

    /// <summary>
    /// Functions creates a new card based on the outcome color of red or green
    /// </summary>
    void GetWinnerCard()
    {
        GameObject winnerCard = Instantiate(blankCard, this.transform.position, Quaternion.Euler(0, 90, 0), parent);    // Creates new card at 90 so that it is not visible
        winnerCard.GetComponent<CardFlip>().enabled = false;    // Disables this script in the new card

        if (gm.playerWin && gm.betRed)      // Checks whether Player won or not, what color player bet on and creates the new card accordingly
        {
            WinCard(winnerCard, Color.red);
        }
        else if (gm.playerWin && gm.betGreen)
        {
            WinCard(winnerCard, Color.green);

        }
        else if (!gm.playerWin && gm.betRed)
        {
            WinCard(winnerCard, Color.green);
        }
        else
        {
            WinCard(winnerCard, Color.red);
        }
        Destroy(this.gameObject);       // Destroys this GameObject as this card is already been fliped
    }

    /// <summary>
    /// Function to assign color values to new card and play flip back animation 
    /// </summary>
    /// <param name="col">Parameter contains color value to be assigned to new card </param> 
    void WinCard(GameObject newCard, Color col)
    {
        SpriteRenderer sr = newCard.GetComponent<SpriteRenderer>();
        sr.color = col;
        sr.flipY = true;        // Flips Y axis of the card for perfect animation
        newCard.GetComponent<Animator>().SetBool("FlipBack", true);     // Plays FlipBack animation to turn the card facing front
    }
}
