using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using EzySlice;

public class PistolController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletForce = 10f;
    public float fireRate = 0.5f;

    private float nextFireTime = 0f;

    private void Shoot()
    {
        // Update the next allowed fire time
        nextFireTime = Time.time + 1f / fireRate;

        // Instantiate the bullet prefab at the shoot point position and rotation
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // Access the Rigidbody component of the bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Check if the bullet has a Rigidbody component
        if (bulletRb != null)
        {
            // Apply force to the bullet in the forward direction of the shoot point
            bulletRb.AddForce(shootPoint.forward * bulletForce, ForceMode.VelocityChange);
        }
    }
}