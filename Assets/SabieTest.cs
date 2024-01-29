using UnityEngine;
using EzySlice;
using System.Collections;

public class SabieTest : MonoBehaviour
{
    public LayerMask sliceableLayer; // assign the Fruit layer in the Inspector
    public GameObject sword;
    public VelocityEstimator velocityEstimator; // assign the VelocityEstimator in the Inspector
    public float minSwingVelocity = 7f;
    public Transform bladeStart;
    public Transform bladeEnd;

    private void FixedUpdate()
{
    UnityEngine.Vector3 velocity = velocityEstimator.GetVelocityEstimate();

    // Only perform slicing if the sword is moving fast enough
    if (velocity.magnitude >= minSwingVelocity)
    {
        // Define the plane for slicing (position and normal)
        UnityEngine.Vector3 planePosition = bladeStart.position;
        UnityEngine.Vector3 planeNormal = UnityEngine.Vector3.Cross(bladeEnd.position - bladeStart.position, velocity).normalized;

        // Get all colliders in the scene
        Collider[] allColliders = Physics.OverlapSphere(transform.position, 0.9f, sliceableLayer);

        foreach (Collider collider in allColliders)
        {
            GameObject fruit = collider.gameObject;

            // Perform the slicing
            SlicedHull hull = fruit.Slice(planePosition, planeNormal);

            if (hull != null)
            {
                // Create upper and lower game objects from the sliced hull
                GameObject upperHull = hull.CreateUpperHull(fruit, fruit.GetComponent<MeshRenderer>().material);
                GameObject lowerHull = hull.CreateLowerHull(fruit, fruit.GetComponent<MeshRenderer>().material);

                // Add Rigidbody and Collider to the sliced parts
                AddRigidbodyAndCollider(upperHull);
                AddRigidbodyAndCollider(lowerHull);

                // Add some force to the sliced parts for visual effect
                Vector3 forceDirection = velocity.normalized;
                upperHull.GetComponent<Rigidbody>().AddForce(forceDirection * 5);
                lowerHull.GetComponent<Rigidbody>().AddForce(-forceDirection * 5);

                // Destroy the original object
                Destroy(fruit);

                StartCoroutine(DestroyHulls(lowerHull, upperHull));
            }
        }
    }
}

    // void Update()
    // {
    //     // Define the plane for slicing (position and direction)
    //     Vector3 planePosition = sword.transform.position;
    //     Vector3 planeDirection = velocityEstimator.GetVelocityEstimate().normalized; // use the sword's velocity

    //     // Get all colliders in the scene
    //     Collider[] allColliders = Physics.OverlapSphere(transform.position, 1, fruitLayer);

    //     foreach (Collider collider in allColliders)
    //     {
    //         GameObject fruit = collider.gameObject;

    //         // Perform the slicing
    //         SlicedHull hull = fruit.Slice(planePosition, planeDirection);

    //         if (hull != null)
    //         {
    //             // Create upper and lower game objects
    //             GameObject upperGameObject = hull.CreateUpperHull(fruit, fruit.GetComponent<MeshRenderer>().material);
    //             GameObject lowerGameObject = hull.CreateLowerHull(fruit, fruit.GetComponent<MeshRenderer>().material);
                
    //             AddRigidbodyAndCollider(upperGameObject);
    //             AddRigidbodyAndCollider(lowerGameObject);

    //             // Add some force to the sliced parts for visual effect
    //             upperGameObject.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * 5f, ForceMode.Impulse);
    //             lowerGameObject.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * 5f, ForceMode.Impulse);

    //             // Destroy the original fruit
    //             Destroy(fruit);

    //             StartCoroutine(DestroyHulls(lowerGameObject, upperGameObject));
    //         }
    //     }
    // }

    private IEnumerator DestroyHulls(GameObject lowerHull, GameObject upperHull)
    {
        // Adjust the delay time as needed
        float delayTime = 1f;

        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Destroy the sliced parts
        Destroy(lowerHull);
        Destroy(upperHull);
    }

    private void AddRigidbodyAndCollider(GameObject obj)
    {
        // Add Rigidbody to the sliced part
        Rigidbody rb = obj.AddComponent<Rigidbody>();

        // Add Collider to the sliced part
        MeshCollider collider = obj.AddComponent<MeshCollider>();
        collider.convex = true; // Set to true for convex colliders

        // Adjust other collider properties if needed
        // collider.isTrigger = true; // Uncomment this line if you want the collider to be a trigger

        // You might need to adjust the size and position of the collider based on your specific mesh
        // collider.sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
        // collider.inflateMesh = true;

        // Set the layer of the sliced part if needed
        // obj.layer = LayerMask.NameToLayer("YourLayerName");
    }
}