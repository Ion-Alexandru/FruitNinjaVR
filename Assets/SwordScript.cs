using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject yellowParticleEffect;

    public Transform bladeStart;
    public Transform bladeEnd;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;

    public Material crossSectionMaterial;
    public float cutforce = 2f;

    private void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(bladeStart.position, bladeEnd.position, out RaycastHit hit, sliceableLayer);

        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;

            // Add combo score
            GameManager.instance.PlayerCutFruit(target.transform);
            GameObject splashYellow = Instantiate(yellowParticleEffect, target.transform.position, Quaternion.identity);

            // Slice the target
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(bladeEnd.position - bladeStart.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(bladeEnd.position, planeNormal);

        if (hull != null)
        {
            // Create upper and lower game objects from the sliced hull
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            //upperHull.layer = target.layer;

            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            //lowerHull.layer = target.layer;

            // Add Rigidbody and Collider to the sliced parts
            AddRigidbodyAndCollider(upperHull);
            AddRigidbodyAndCollider(lowerHull);

            // Add some force to the sliced parts for visual effect
            upperHull.GetComponent<Rigidbody>().AddForce(transform.up * cutforce);
            lowerHull.GetComponent<Rigidbody>().AddForce(-transform.up * cutforce);

            // Destroy the original object
            Destroy(target);

            StartCoroutine(DestroyHulls(lowerHull, upperHull));
        }
    }
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
