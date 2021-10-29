using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chipAnim : MonoBehaviour
{
    /// <summary>
    /// This is button
    /// </summary>
    public Button btn;

    /// <summary>
    /// 
    /// </summary>
    public GameObject chipIcon;
    public float speed;
    public Transform parent;
    Vector2 pos;
    public Quaternion rotation; void Start()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(btnClicked);
        pos = this.transform.position + new Vector3(2, 2, 0);
    }

    private void Update()
    {
        Debug.Log(transform.position);
    }
    void btnClicked()
    {
        GameObject flyChip = Instantiate(chipIcon, pos, Quaternion.identity);
        flyChip.transform.SetParent(parent);
        flyChip.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        flyChip.name = "Flying Chip";
        //flyChip.GetComponent<flyChip>().Fly(speed);


    }


}
