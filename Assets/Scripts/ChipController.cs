using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to move the chip when clicked
/// </summary>
public class ChipController : MonoBehaviour
{
    private float _movementTime = 4f;
    /// <summary>
    /// Gets destination position of the chip to move, by default it will at (x,y) zero position ie. screen center
    /// </summary>
    private Vector2 _destinationPosition = Vector2.zero;

    private GameManager gm;
    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    /// <summary>
    /// Function updates the chip counter in GameManager and moves the clicked chip
    /// </summary>
    public void OnChipClicked()
    {
        gm.Chip();
        StartMoveChip();
    }
    /// <summary>
    /// Function to call the coroutine for moving the chip
    /// </summary>
    public void StartMoveChip()
    {
        StartCoroutine(MoveChip());
    }

    /// <summary>
    /// Function to actually moving the chip
    /// </summary>
    /// <returns>Returns Null</returns>
    private IEnumerator MoveChip()
    {
        float currentTime = 0f;
        float dist = Vector2.Distance(transform.localPosition, _destinationPosition);   // Finds the distance between the chip and destination

        while (dist > 0)
        {
            currentTime += Time.deltaTime;
            transform.localPosition = Vector2.Lerp(transform.localPosition, _destinationPosition, currentTime / _movementTime);     // Moves the chip using Time

            if (new Vector2(transform.localPosition.x, transform.localPosition.y) == _destinationPosition)
            {
                Destroy(this.gameObject);               // If the chip reaches the destination position, it will be destroyed
                //this.gameObject.SetActive(false);
            }
            yield return null;

        }
    }

}

