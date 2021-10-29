using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardFlip : MonoBehaviour
{
    Animator anim;
    public GameObject blankCard;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FlipCard()
    {
        anim.SetBool("Flip", true);
    }

    void WinCard(GameObject newCard, Color col)
    {
        SpriteRenderer sr = newCard.GetComponent<SpriteRenderer>();
        sr.color = col;
        sr.flipY = true;
        newCard.GetComponent<Animator>().SetBool("FlipBack", true);

    }
    void GetWinnerCard()
    {
        GameObject winnerCard = Instantiate(blankCard, this.transform.position, Quaternion.Euler(0, 90, 0));
        winnerCard.GetComponent<cardFlip>().enabled = false;

        GameManager gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (gm.playerWin && gm.betRed)
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

        Destroy(this.gameObject);
    }
}
