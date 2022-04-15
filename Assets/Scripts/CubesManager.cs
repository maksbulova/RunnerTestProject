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
    [SerializeField] private GameObject popUpText;
    [SerializeField] private AnimationCurve popUpCurve;
    [SerializeField] private float popUpduration;
    [SerializeField] private TrailRenderer trail;

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

    const float jumpHeight = 2f;
    private void AddCube(GameObject newCube)
    {
        // TODO jump animation
        player.transform.Translate(Vector3.up * jumpHeight);

        newCube.gameObject.tag = "Untagged";
        newCube.transform.position = player.transform.position + (Vector3.down * 1.5f);
        newCube.transform.SetParent(cubeHolder);

        GameObject popUpEffect = Instantiate(popUpText, newCube.transform.position, Quaternion.identity);
        StartCoroutine(AnimatePopUp(popUpEffect));

        cubesStack.Add(newCube);
    }

    private IEnumerator AnimatePopUp(GameObject effect)
    {
        float t = 0;
        while (t < popUpduration)
        {
            t += Time.deltaTime;

            Vector3 moveDir = new Vector3(1, 1, 0) * popUpCurve.Evaluate(t / popUpduration);
            effect.transform.Translate(moveDir * Time.deltaTime);

            yield return null;
        }
        Destroy(effect);
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

        trail.emitting = false;
        Invoke("StartTrail", 1f);
    }

    private void StartTrail()
    {
        trail.emitting = true;
    }

    public void PlayerDeath()
    {
        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        playerRB.constraints = RigidbodyConstraints.None;
        playerRB.AddForce(Vector3.forward * 5);

        menu.EndLevel();
    }

}
