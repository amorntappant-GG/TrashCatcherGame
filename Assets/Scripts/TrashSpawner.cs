using System.Collections;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RectTransform trashContainer;
    [SerializeField] private TrashItem[] trashPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float sidePadding = 60f;
    [SerializeField] private float spawnOffset = 100f;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (gameManager != null && !gameManager.IsGameOver)
            {
                SpawnTrash();
            }
        }
    }

    private void SpawnTrash()
    {
        if (trashContainer == null ||
            trashPrefabs == null ||
            trashPrefabs.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, trashPrefabs.Length);

        TrashItem newTrash = Instantiate(
            trashPrefabs[randomIndex],
            trashContainer
        );

        float halfWidth = trashContainer.rect.width * 0.5f;
        float halfHeight = trashContainer.rect.height * 0.5f;

        float randomX = Random.Range(
            -halfWidth + sidePadding,
            halfWidth - sidePadding
        );

        RectTransform trashRect =
            newTrash.GetComponent<RectTransform>();

        trashRect.anchoredPosition = new Vector2(
            randomX,
            halfHeight + spawnOffset
        );

        float destroyY = -halfHeight - spawnOffset;

        newTrash.Initialize(gameManager, destroyY);
    }
}