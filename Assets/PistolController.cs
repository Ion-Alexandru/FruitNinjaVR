using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using EzySlice;
using Unity.Mathematics;

public class PistolController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletForce = 10f;
    public float fireRate = 0.75f;
    public GameObject shotParticle;
    public AudioClip shootSound;
    private AudioSource audioSource;
    public GameObject muzzleFlashPrefab;
    private float nextFireTime = 0.2f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Shoot()
    {
        // Play the shoot sound and show the muzzle flash
        audioSource.PlayOneShot(shootSound);

        MuzzleFlash();

        // Update the next allowed fire time
        nextFireTime = Time.time + 1f / fireRate;

        // Instantiate the bullet prefab at the shoot point position and rotation
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Access the Rigidbody component of the bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Instantiate the particle system
        Instantiate(shotParticle, shootPoint.position, Quaternion.identity);

        // Check if the bullet has a Rigidbody component
        if (bulletRb != null)
        {
            // Apply force to the bullet in the forward direction of the shoot point
            bulletRb.AddForce(shootPoint.right * bulletForce, ForceMode.VelocityChange);
        }
    }

    void MuzzleFlash()
    {
        // Instantiate the muzzle flash
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation);
        muzzleFlash.transform.parent = shootPoint;

        // Destroy the muzzle flash after a short delay
        Destroy(muzzleFlash, 0.05f);
    }
}