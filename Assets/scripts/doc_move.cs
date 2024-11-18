using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doc_move : MonoBehaviour
{
    public float speed = 5.0f;
    private float forwardTime = 1.6f;
    private float turnTime = 1f;
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < forwardTime)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (elapsedTime < forwardTime + turnTime)
        {
            transform.Rotate(Vector3.up, -90f * Time.deltaTime);
        }
    }
}
