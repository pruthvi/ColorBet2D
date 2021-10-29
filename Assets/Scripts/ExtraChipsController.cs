using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraChipsController : MonoBehaviour
{
    public Text displayText;

    private GameManager gm;
    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void OnExtraChipClicked()
    {
        UpdateDisplayText(10);
        gm.UpdateExtraChipsCount();
    }

    public void UpdateDisplayText(int number)
    {
        displayText.text = number.ToString();
    }
}
