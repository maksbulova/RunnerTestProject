using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed;
    public float sideBound;


    private void Update()
    {
        Vector3 sideMove = Vector3.zero;

        if (Input.touchCount > 0)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            float normalizedPos = touchPosition.x / Screen.width;
            float targetPos = Mathf.Lerp(-sideBound, +sideBound, normalizedPos);
            // Between left bound and right bound;
            sideMove = Vector3.right * (targetPos - transform.position.x);
        }
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + forwardMove + sideMove;
        transform.position = newPosition;
    }

}
