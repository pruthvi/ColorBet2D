using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyChip : MonoBehaviour
{
    public float speed = 5f;

    public Vector3 Direction = Vector3.zero;

    Vector3 zero;
    void Start()
    {
        zero = new Vector3(0, 0, 0);

        //   move();
        // if (transform. != zero)
        // {
        // transform.Translate(zero * 0.5f * Time.deltaTime);
        // Debug.Log("flying");
        // }
    }

    // public void Fly(float speed)
    // {
    //     this.speed = speed;
    // }

    void Update()
    {
        float translation = speed * Time.deltaTime;
        transform.Translate(Direction * translation);
    }


    void FixedUpdate()
    {

        // Transform ThisTransform = GetComponent<Transform>();
        // Debug.Log(ThisTransform);
        //Update position in specified direction by speed
        // ThisTransform.position += Direction.normalized * 0.05f * Time.deltaTime;


        // if (new Vector3(transform.position.x, transform.position.y) != zero)
        // {
        //     move();
        //     Debug.Log("move!!!!");
        // }
    }

    void move()
    {
        transform.Translate(zero * 0.5f * Time.deltaTime);
    }


}
