using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraChipController : MonoBehaviour
{
    private GameManager gm;
    public Transform parent;
    public GameObject Chip;
    public Text txtExtra;
    public int extras;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        //  parent = this.gameObject.transform;
    }

    private void Start()
    {
        Debug.Log("Extra Chip Start");

        CheckExtraChips();
    }
    public void OnExtraChipHit()
    {
        GameObject ExtraChipFire = Instantiate(Chip, this.transform.position, Quaternion.identity, parent);
        ExtraChipFire.GetComponent<Image>().color = Color.white;
        ExtraChipFire.GetComponent<ChipController>().StartMoveChip();
        extras -= 10;
        gm.chip();
        UpdateExtrasCount();
        if (extras == 0)
        {
            gm.ActiveExtraChip(false);
        }
    }

    private void UpdateExtrasCount()
    {
        txtExtra.text = extras.ToString();
    }

    public void CheckExtraChips()
    {
        if (extras <= 0)
        {
            Debug.Log("Chips Checks : " + gm.chips);

            if (gm.chips > 100)
            {
                extras = gm.chips - 100;
                UpdateExtrasCount();

            }
        }

    }

}
