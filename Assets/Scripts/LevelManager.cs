using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelBlocks;
    [SerializeField] private Transform blockContainer;
    [SerializeField] private float blockSize;
    [SerializeField] private int initialBlocksAmount;

    private Queue<GameObject> createdBlocks;
    private Vector3 newBlockPosition;


    private void Start()
    {
        createdBlocks = new Queue<GameObject>();
        newBlockPosition = Vector3.back * blockSize;

        for (int i = 0; i < initialBlocksAmount; i++)
        {
            AddBlock(false);
        }
    }

    public void AddBlock(bool animated = true)
    {
        int randomBlockIndex = Random.Range(0, levelBlocks.Length);
        GameObject randomBlock = levelBlocks[randomBlockIndex];
        GameObject newBlock = Instantiate(randomBlock, newBlockPosition, Quaternion.identity, blockContainer);

        newBlockPosition += Vector3.forward * blockSize;

        createdBlocks.Enqueue(newBlock);

        if (Application.isPlaying && animated)
        {
            StartCoroutine(BlockPopUpAnimation(newBlock));
        }
    }

    public void RemoveBlock()
    {
        GameObject block = createdBlocks.Dequeue();
        if (Application.isPlaying)
        {
            Destroy(block);
        }
        else
        {
            DestroyImmediate(block);
        }
    }


    private IEnumerator BlockPopUpAnimation(GameObject block)
    {
        Vector3 blockFinishPosition = block.transform.position;
        Vector3 blockStartPosition = block.transform.position + Vector3.down * 20f;

        block.transform.position = blockStartPosition;

        float t = 0;
        float animationDuration = 1;
        while ((block.transform.position - blockFinishPosition).sqrMagnitude > 0.05f)
        {
            block.transform.position = Vector3.Lerp(blockStartPosition, blockFinishPosition, t / animationDuration);
            t += Time.deltaTime;
            yield return null;
        }
        block.transform.position = blockFinishPosition;
    }
}
