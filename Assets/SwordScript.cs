using EzySlice;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    //public GameObject yellowParticleEffect;
    public Transform bladeStart;
    public Transform bladeEnd;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    public LayerMask bombLayer;
    public LayerMask fruitButtonLayer;

    public Material crossSectionMaterial;
    public float cutforce = 2f;

    // Sword sound logic
    public float minSwingVelocity = 7f;
    public AudioClip swingSound;
    public AudioClip fruitSliceSound;
    private bool isSwingSoundPlaying = false;
    private AudioSource audioSource;
    private UnityEngine.Vector3 previousSwingDirection;
    private float swingDirectionChangeThreshold = 200f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
                    // Add combo score
                    GameManager.instance.PlayerCutFruit(fruit.transform);
                    //GameObject splashYellow = Instantiate(yellowParticleEffect, target.transform.position, Quaternion.identity);

                    fruit.GetComponent<FruitScript>().FruitSliced();

                    if(!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(fruitSliceSound);
                    }

                    // Create upper and lower game objects from the sliced hull
                    GameObject upperHull = hull.CreateUpperHull(fruit, fruit.GetComponent<MeshRenderer>().material);
                    GameObject lowerHull = hull.CreateLowerHull(fruit, fruit.GetComponent<MeshRenderer>().material);

                    // Add Rigidbody and Collider to the sliced parts
                    AddRigidbodyAndCollider(upperHull);
                    AddRigidbodyAndCollider(lowerHull);

                    // Add some force to the sliced parts for visual effect
                    UnityEngine.Vector3 forceDirection = velocity.normalized;
                    upperHull.GetComponent<Rigidbody>().AddForce(forceDirection * cutforce, ForceMode.Impulse);
                    lowerHull.GetComponent<Rigidbody>().AddForce(-forceDirection * cutforce, ForceMode.Impulse);

                    // Destroy the original object
                    Destroy(fruit);

                    StartCoroutine(DestroyHulls(lowerHull, upperHull));
                }
            }
        }

        // bool hasHit = Physics.Linecast(bladeStart.position, bladeEnd.position, out RaycastHit hit, sliceableLayer);

        // if (hasHit)
        // {
        //     GameObject target = hit.transform.gameObject;

        //     // Add combo score
        //     GameManager.instance.PlayerCutFruit(target.transform);
        //     //GameObject splashYellow = Instantiate(yellowParticleEffect, target.transform.position, Quaternion.identity);

        //     target.GetComponent<FruitScript>().FruitSliced();

        //     if(!audioSource.isPlaying)
        //     {
        //         audioSource.PlayOneShot(fruitSliceSound);
        //     }

        //     // Slice the target
        //     Slice(target);
        // }

        // Bomb cut logic
        bool bombHit = Physics.Linecast(bladeStart.position, bladeEnd.position, out RaycastHit bombRay, bombLayer);

        if(bombHit)
        {
            GameObject target = bombRay.transform.gameObject;

            target.GetComponent<BombScript>().BombHit();
        }

        // Fruit Button logic
        bool fruitButtonHit = Physics.Linecast(bladeStart.position, bladeEnd.position, out RaycastHit fruitButtonRay, fruitButtonLayer);

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

        // Get the current swing direction
        UnityEngine.Vector3 currentSwingDirection = velocityEstimator.GetVelocityEstimate().normalized;

        // Check if the swing sound should be played
        if (swingSound != null && ShouldPlaySwingSound(currentSwingDirection))
        {
            audioSource.PlayOneShot(swingSound);
        }

        // Update the previous swing direction
        previousSwingDirection = currentSwingDirection;
    }

    // Function to determine if the swing sound should be played based on the change in swing direction
    private bool ShouldPlaySwingSound(UnityEngine.Vector3 currentSwingDirection)
    {
        // Check if the swing sound is not already playing and the change in swing direction exceeds the threshold
        if (!isSwingSoundPlaying && UnityEngine.Vector3.Angle(currentSwingDirection, previousSwingDirection) > swingDirectionChangeThreshold)
        {
            // Set the flag to indicate that the swing sound is now playing
            isSwingSoundPlaying = true;
            return true; // Return true to play the swing sound
        }

        // If the swing direction change is not significant or the swing sound is already playing, return false
        return false;
    }

    // Reset the flag when the swing sound finishes playing
    // private void Update()
    // {
    //     if (!audioSource.isPlaying && isSwingSoundPlaying)
    //     {
    //         isSwingSoundPlaying = false; // Reset the flag since the swing sound has finished playing
    //     }
    // }

    public void Slice(GameObject target)
    {
        UnityEngine.Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        UnityEngine.Vector3 planeNormal = UnityEngine.Vector3.Cross(bladeEnd.position - bladeStart.position, velocity);
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
