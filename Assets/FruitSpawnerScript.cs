using System.Collections;
using UnityEngine;

public class FruitSpawnerScript : MonoBehaviour
{
    public GameObject[] fruits;
    public float initialSpawnInterval = 4f;
    public float minSpawnInterval = 1.5f;
    public float spawnForce = 5f;
    public float maxOffset = 0.1f;

    public GameObject[] fruitSpawners;

    public Transform player;
    public float constrainedYValue;

    private void Awake()
    {
        StartCoroutine(SpawnFruits());
    }

    private void Update()
    {
        //if (player != null)
        //{
        //    // Follow the parent's position
        //    Vector3 newPosition = player.position;

        //    // Constrain the y-value
        //    newPosition.y = constrainedYValue;
        //    newPosition.z = newPosition.z + 1;

        //    // Apply the new position to the child object
        //    transform.position = newPosition;

        //    // Ensure no rotation on the x-axis
        //    //transform.rotation = Quaternion.Euler(0f, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z);
        //    transform.LookAt(transform.position + player.transform.rotation * Vector3.forward, player.transform.rotation * Vector3.up);
        //}
    }

    IEnumerator SpawnFruits()
    {
        WaitForSeconds wait = new WaitForSeconds(initialSpawnInterval);

        while (true)
        {
            yield return wait;

            // Randomly choose the number of fruits to spawn
            int numFruits = Random.Range(1, 4);

            for (int i = 0; i < numFruits; i++)
            {
                SpawnFruit();
            }

            // Gradually decrease the spawn interval
            initialSpawnInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval - 0.1f);
            wait = new WaitForSeconds(initialSpawnInterval);
        }
    }

    private void SpawnFruit()
    {
        // Instantiate a new fruit at a random spawner position
        GameObject randomFruitPrefab = fruits[Random.Range(0, fruits.Length)];
        Vector3 spawnPosition = fruitSpawners[Random.Range(0, fruitSpawners.Length)].transform.position;

        // Set a random rotation for the fruit
        Quaternion randomRotation = Quaternion.Euler(Random.Range(90, 135), Random.Range(90, 270), Random.Range(-90, -270));

        GameObject newFruit = Instantiate(randomFruitPrefab, spawnPosition, randomRotation);

        // Apply force to the fruit
        Rigidbody fruitRb = newFruit.GetComponent<Rigidbody>();

        if (fruitRb != null)
        {
            // Ensure the force is applied only in the upward direction
            Vector3 upwardForce = Vector3.up * spawnForce;

            // Apply the force to the fruit with the calculated direction and upward force
            fruitRb.AddForce(upwardForce, ForceMode.Impulse);

            // Add a unique random offset to the force direction for each fruit
            fruitRb.AddForce(new Vector3(Random.Range(-maxOffset, maxOffset), 0f, Random.Range(-maxOffset, maxOffset)), ForceMode.Impulse);

            if (newFruit.transform.position.y < -1)
            {
                
            }
        }
    }
}

//void Start()
//{
//    StartCoroutine(SpawnFruits());
//}

//IEnumerator SpawnFruits()
//{
//    while (true)
//    {
//        yield return new WaitForSeconds(Random.Range(spawnInterval - 0.2f, spawnInterval));

//        // Instantiate a new fruit with a small random offset
//        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-maxOffset, maxOffset), 0f, 0);
//        GameObject newFruit = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);

//        // Apply force to the fruit
//        Rigidbody fruitRb = newFruit.GetComponent<Rigidbody>();

//        if (fruitRb != null)
//        {
//            // Ensure the force is applied only in the upward direction
//            Vector3 upwardForce = Vector3.up * spawnForce;

//            // Apply the force to the fruit with the calculated direction and upward force
//            fruitRb.AddForce(upwardForce, ForceMode.Impulse);
//        }
//    }
//}

//private void OnCollisionEnter(Collision collision)
//{
//    if(collision.gameObject == floor)
//    {
//        Destroy(gameObject);
//    }
//}