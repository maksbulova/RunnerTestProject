using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubesManager : MonoBehaviour
{
    [SerializeField] private Transform cubeHolder;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject startingCube;

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
        foreach (GameObject cube in cubesStack)
        {
            float obstacleElevation = obstacle.transform.TransformPoint(transform.position).y;
            float cubeElevation = cube.transform.TransformPoint(transform.position).y;
            if (Mathf.Floor(obstacleElevation) == Mathf.Floor(cubeElevation))
            {
                cube.transform.SetParent(null, true);
                cubesStack.Remove(cube);
                break;
            }
        }

        shakeEvent.Invoke();

        if (cubesStack.Count == 0)
        {
            // Game over
        }
    }
}
