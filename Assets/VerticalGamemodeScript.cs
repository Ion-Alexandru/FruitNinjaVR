using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalGamemodeScript : MonoBehaviour
{
    public GameObject[] verticalSpawners;
    public float initialSpawnInterval = 4f;
    public float minSpawnInterval = 1.5f;

    public void Awake()
    {
        StartCoroutine(VerticalFruitSpawner());
    }

    IEnumerator VerticalFruitSpawner()
    {
        WaitForSeconds wait = new WaitForSeconds(initialSpawnInterval);

        while (true)
        {
            yield return wait;

            // Randomly choose the number of fruits to spawn
            int numSpawners = Random.Range(1, verticalSpawners.Length); // this can be changed

            // get the VerticalFruitSpawnerScript of verticalSpawners[numSpawners]
            verticalSpawners[numSpawners].GetComponent<VerticalFruitSpawnerScript>().SpawnFruit();

            // Gradually decrease the spawn interval
            initialSpawnInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval - 0.1f);
            wait = new WaitForSeconds(initialSpawnInterval);
        }
    }
}
