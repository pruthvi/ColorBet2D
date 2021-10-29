using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to check and create extra chips if player posses over/under 100 chips
/// </summary>
public class ExtraChipController : MonoBehaviour
{
    private GameManager gm;
    public Transform parent;    // To create new chip under GameCanvas
    public GameObject Chip;     // Gets prefab for new chip
    public Text txtExtra;   // To updates number on the Extra Chip

    /// <summary>
    /// Number of Extra Chips
    /// </summary>
    public int extras;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Start()
    {
        CheckExtraChips();  // Checks if extra chips are there from previous gameplay.
    }

    /// <summary>
    /// Function executes when Player clicks on the last extra chip.
    /// </summary>
    public void OnExtraChipHit()
    {
        GameObject ExtraChipFire = Instantiate(Chip, this.transform.position, Quaternion.identity, parent); // Creates a new chip which moves to the center card
        ExtraChipFire.GetComponent<Image>().color = Color.white;
        ExtraChipFire.GetComponent<ChipController>().StartMoveChip();
        extras -= 10;   // Updates the Extra Chip counter
        UpdateExtrasCount();
        gm.Chip();      // Updates Chip counter in GameManager

        if (extras == 0)    // If there is no extra chip, this gameobject will be disabled from the GameManager
        {
            gm.ActiveExtraChip(false);
        }
    }

    /// <summary>
    /// Updates number of extra chips on the last chip
    /// </summary>
    private void UpdateExtrasCount()
    {
        txtExtra.text = extras.ToString();
    }

    /// <summary>
    /// Function to check if extra chips over 100 chips exists. If they do, than it will find how many extra chips are there and updates Extra Chip Counts.
    /// </summary>
    public void CheckExtraChips()
    {
        if (extras <= 0)
        {
            if (gm.chips > 100)
            {
                extras = gm.chips - 100;
                UpdateExtrasCount();
            }
        }

    }

}
