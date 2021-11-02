using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public Text txtPlayerName;

    public PhotonView photonView;
    public GameObject canvas;
    Camera cam;
    private void Awake()
    {
        // GameObject CamGB = GameObject.FindGameObjectWithTag("Camera");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas.GetComponent<Canvas>().worldCamera = cam;

    }
    void Start()
    {
        if (photonView.IsMine)
        {
            txtPlayerName.text = PhotonNetwork.NickName;
            // GameObject gameCam = GameObject.FindGameObjectWithTag("MainCamera");
            // gameCam.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
