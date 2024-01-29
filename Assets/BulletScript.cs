using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class BulletScript : MonoBehaviour
{
    public Material crossSectionMaterial;
    public VelocityEstimator velocityEstimator;

    public LayerMask sliceableLayer;
    public LayerMask bombLayer;
    public LayerMask fruitButtonLayer;

    public float cutforce = 20f;
    public int sliceDistance = 10;

    public Transform bulletStart;
    public Transform bulletEnd;

    //public GameObject yellowParticleEffect;

    // Bullet Fruit Slice Sound logic
    private AudioSource audioSource;
    public AudioClip fruitSliceSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(bulletStart.position, bulletEnd.position, out RaycastHit hit, sliceableLayer);

        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;

            // Add combo score
            GameManager.instance.PlayerCutFruit(target.transform);
            //GameObject splashYellow = Instantiate(yellowParticleEffect, target.transform.position, Quaternion.identity);

            target.GetComponent<FruitScript>().FruitSliced();

            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fruitSliceSound);
            }

            // Slice the target
            Slice(target);
        }

        // Bomb cut logic
        bool bombHit = Physics.Linecast(bulletStart.position, bulletEnd.position, out RaycastHit bombRay, bombLayer);

        if(bombHit)
        {
            GameObject target = bombRay.transform.gameObject;

            target.GetComponent<BombScript>().BombHit();
        }

        // Fruit Button logic
        bool fruitButtonHit = Physics.Linecast(bulletStart.position, bulletEnd.position, out RaycastHit fruitButtonRay, fruitButtonLayer);

        if(fruitButtonHit)
        {
            GameObject target = fruitButtonRay.transform.gameObject;

            target.GetComponent<FruitScript>().FruitSliced();

            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fruitSliceSound);
            }

            target.GetComponent<FruitButtonScript>().StartGame();

            // Slice the target
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(bulletEnd.position - bulletStart.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(bulletEnd.position, planeNormal);

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

    //void OnTriggerEnter(Collider collider)
    //{
    //    // Use bullet's position for Linecast
    //    bool hasHit = Physics.Linecast(transform.position, collider.transform.position, out RaycastHit hit, sliceableLayer);

    //    if (hasHit)
    //    {
    //        GameObject target = hit.transform.gameObject;

    //        //Add combo value
    //        GameManager.instance.PlayerCutFruit(target.transform);

    //        Slice(target);
    //    }

    //    StartCoroutine(DestroyBullet());
    //}

    //private IEnumerator DestroyBullet()
    //{
    //    yield return new WaitForSeconds(3);

    //    Destroy(gameObject);
    //}

    //private void Slice(GameObject target)
    //{
    //    Vector3 velocity = velocityEstimator.GetVelocityEstimate();
    //    // Generate a random slicing plane normal
    //    Vector3 randomPlaneNormal = Random.onUnitSphere * Random.Range(2f, 4f);
    //    Quaternion randomRotation = Random.rotation;

    //    randomPlaneNormal = randomRotation * randomPlaneNormal;

    //    SlicedHull hull = target.Slice(transform.position, randomPlaneNormal);

    //    if (hull != null)
    //    {
    //        // Create upper and lower game objects from the sliced hull
    //        GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
    //        //upperHull.layer = target.layer;

    //        GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
    //        //lowerHull.layer = target.layer;

    //        // Add Rigidbody and Collider to the sliced parts
    //        AddRigidbodyAndCollider(upperHull);
    //        AddRigidbodyAndCollider(lowerHull);

    //        // Add some force to the sliced parts for visual effect
    //        upperHull.GetComponent<Rigidbody>().AddForce(transform.up * cutforce);
    //        lowerHull.GetComponent<Rigidbody>().AddForce(-transform.up * cutforce);

    //        // Destroy the original object
    //        Destroy(target);

    //        StartCoroutine(DestroyHulls(lowerHull, upperHull));
    //    }
    //}

    //private IEnumerator DestroyHulls(GameObject lowerHull, GameObject upperHull)
    //{
    //    // Adjust the delay time as needed
    //    float delayTime = 1f;

    //    // Wait for the specified delay time
    //    yield return new WaitForSeconds(delayTime);

    //    // Destroy the sliced parts
    //    Destroy(lowerHull);
    //    Destroy(upperHull);
    //}

    //private void AddRigidbodyAndCollider(GameObject obj)
    //{
    //    // Add Rigidbody to the sliced part
    //    Rigidbody rb = obj.AddComponent<Rigidbody>();

    //    // Add Collider to the sliced part
    //    MeshCollider collider = obj.AddComponent<MeshCollider>();

    //    collider.convex = true; // Set to true for convex colliders
    //    //collider.isTrigger = true;
    //}
}