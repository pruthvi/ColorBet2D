using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipController : MonoBehaviour
{
    private float _movementTime = 2f;
    private Vector2 _destinationPosition = Vector2.zero;


    public void OnChipClicked()
    {
        StartCoroutine(MoveChip());
    }

    private IEnumerator MoveChip()
    {
        var currentTime = 0f;
        while (Vector2.Distance(transform.localPosition, _destinationPosition) > 0)
        {
            currentTime += Time.deltaTime;
            transform.localPosition = Vector2.Lerp(transform.localPosition, _destinationPosition, currentTime / _movementTime);
            yield return null;
        }
    }
}
