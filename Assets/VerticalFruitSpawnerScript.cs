using System.Collections;
using UnityEngine;

public class VerticalFruitSpawnerScript : MonoBehaviour
{
    public float spawnForce;
    public GameObject[] fruitPrefabs;

    public float initialSpawnInterval = 4f;
    public float minSpawnInterval = 1.5f;

    public GameObject[] fruitSpawners;

    public GameObject player;

    public void SpawnFruit()
    {
        for(int i = 0; i < fruitSpawners.Length; i++)
        {
            int fruitType = Random.Range(1, fruitPrefabs.Length);
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType], fruitSpawners[i].transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

            // Apply force to the fruit
            Rigidbody fruitRb = newFruit.GetComponent<Rigidbody>();

            if (fruitRb != null)
            {
                // Ensure the force is applied only in the upward direction
                Vector3 forwardForce = -Vector3.forward * spawnForce;

                // Apply the force continuously to the fruit
                StartCoroutine(ApplyContinuousForce(fruitRb, forwardForce, newFruit));
                fruitRb.useGravity = false;
            }
        }
    }

    IEnumerator ApplyContinuousForce(Rigidbody rb, Vector3 force, GameObject newFruit)
    {
        while (rb != null)
        {
            // Apply force in each FixedUpdate iteration
            rb.AddForce(force, ForceMode.Impulse);
            yield return new WaitForFixedUpdate();
        }
    }
}