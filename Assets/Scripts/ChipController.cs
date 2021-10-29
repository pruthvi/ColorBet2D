using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChipController : MonoBehaviour
{
    private float _movementTime = 4f;
    private Vector2 _destinationPosition = Vector2.zero;

    private GameManager gm;
    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void OnChipClicked()
    {
        gm.chip();
        StartMoveChip();
    }

    public void StartMoveChip()
    {
        StartCoroutine(MoveChip());

    }

    private IEnumerator MoveChip()
    {
        var currentTime = 0f;
        float dist = Vector2.Distance(transform.localPosition, _destinationPosition);
        while (dist > 0)
        {
            currentTime += Time.deltaTime;
            transform.localPosition = Vector2.Lerp(transform.localPosition, _destinationPosition, currentTime / _movementTime);
            //  Debug.Log("distance" + dist);

            if (new Vector2(transform.localPosition.x, transform.localPosition.y) == _destinationPosition)
            {
                //Debug.Log("chip is in zero poisition");
                Destroy(this.gameObject);
                this.gameObject.SetActive(false);
            }
            yield return null;

        }
    }

}

