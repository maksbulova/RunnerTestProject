using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubesManager : MonoBehaviour
{
    [SerializeField] private Transform cubeHolder;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject startingCube;
    public Menu menu;

    private List<GameObject> cubesStack;
    public UnityEvent shakeEvent;


    private void Start()
    {
        cubesStack = new List<GameObject>() { startingCube };
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Collectable":
                AddCube(other.gameObject);
                break;
            case "Obstacle":
                RemoveCube(other.gameObject);
                break;
            default:
                break;
        }

    }

    private void AddCube(GameObject newCube)
    {
        // TODO jump animation
        player.transform.Translate(Vector3.up * 2f);

        newCube.gameObject.tag = "Untagged";
        newCube.transform.position = player.transform.position + (Vector3.down * 1.5f);
        newCube.transform.SetParent(cubeHolder);

        cubesStack.Add(newCube);
    }

    private void RemoveCube(GameObject obstacle)
    {
        float playerElevation = Mathf.Floor(player.transform.TransformPoint(transform.position).y);
        float obstacleElevation = Mathf.Floor(obstacle.transform.TransformPoint(transform.position).y);

        if (obstacleElevation == playerElevation)
        {
            PlayerDeath();
        }

        // Stop controlling cubes stopped by obstacle.
        foreach (GameObject cube in cubesStack)
        {
            float cubeElevation = Mathf.Floor(cube.transform.TransformPoint(transform.position).y);

            if (obstacleElevation == cubeElevation)
            {
                cube.transform.SetParent(null, true);
                cubesStack.Remove(cube);

                // Check cube under character.
                if (cubeElevation == playerElevation - 1)
                {
                    PlayerDeath();
                }
                break;
            }
        }

        Handheld.Vibrate();
        shakeEvent.Invoke();
    }

    public void PlayerDeath()
    {
        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        playerRB.constraints = RigidbodyConstraints.None;
        playerRB.AddForce(Vector3.forward * 5);

        menu.EndLevel();
    }

}
