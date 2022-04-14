using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelBlocks;
    [SerializeField] private Transform blockContainer;
    [SerializeField] private float blockSize;

    private Queue<GameObject> createdBlocks;
    private Vector3 newBlockPosition;

    private void Awake()
    {
        ResetParams();
    }

    [ContextMenu("Add level block.")]
    public void AddBlock()
    {
        int randomBlockIndex = Random.Range(0, levelBlocks.Length);
        GameObject randomBlock = levelBlocks[randomBlockIndex];
        GameObject newBlock = Instantiate(randomBlock, newBlockPosition, Quaternion.identity, blockContainer);

        newBlockPosition += Vector3.forward * blockSize;

        createdBlocks.Enqueue(newBlock);

        if (Application.isPlaying)
        {
            StartCoroutine(BlockPopUpAnimation(newBlock));
        }
    }

    [ContextMenu("Remove level block.")]
    public void RemoveBlock()
    {
        GameObject block = createdBlocks.Dequeue();
        Destroy(block);
    }

    [ContextMenu("Reset params.")]
    public void ResetParams()
    {
        createdBlocks = new Queue<GameObject>();
        newBlockPosition = Vector3.zero;
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
