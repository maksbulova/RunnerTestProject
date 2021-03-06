using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private float sideBound;

    [SerializeField] private LevelManager levelManager;

    private void FixedUpdate()
    {
        if(Menu.gameState == Menu.GameState.run)
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

            forwardSpeed += forwardAcceleration * Time.fixedDeltaTime;
            Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.fixedDeltaTime;
            Vector3 newPosition = transform.position + forwardMove + sideMove;
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            levelManager.AddBlock();
            levelManager.RemoveBlock();
        }
    }
}
