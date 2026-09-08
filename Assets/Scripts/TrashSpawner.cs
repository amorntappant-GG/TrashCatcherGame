using System.Collections;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RectTransform trashContainer;
    [SerializeField] private RectTransform trashBin;
    [SerializeField] private TrashItem[] trashPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private int trashPerSpawn = 5;

    [Header("Position")]
    [SerializeField] private float spawnY = 600f;
    [SerializeField] private float floorY = -500f;
    [SerializeField] private float minX = -400f;
    [SerializeField] private float maxX = 400f;

    private IEnumerator Start()
    {
        while (true)
        {
            if (gameManager != null &&
                !gameManager.IsGameOver)
            {
                StartCoroutine(SpawnTrashRain());
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    private IEnumerator SpawnTrashRain()
    {
        for (int i = 0; i < trashPerSpawn; i++)
        {
            SpawnTrash();
            
            yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));
        }
    }

    private void SpawnTrash()
    {
        if (trashContainer == null) return;

        if (trashPrefabs == null ||
            trashPrefabs.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(
            0,
            trashPrefabs.Length
        );

        TrashItem newTrash = Instantiate(
            trashPrefabs[randomIndex],
            trashContainer
        );

        RectTransform trashRect =
            newTrash.GetComponent<RectTransform>();
        
        float randomX = Random.Range(
            minX,
            maxX
        );
        
        float randomY = spawnY + Random.Range(-100f, 100f);

        trashRect.anchoredPosition = new Vector2(
            randomX,
            randomY
        );

        newTrash.Initialize(
            gameManager,
            trashBin,
            floorY
        );
    }
}