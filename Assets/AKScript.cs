using System.Collections;
using UnityEngine;
using EzySlice;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Xml.Serialization;

public class AKScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletForce = 10f;
    public float fireRate = 10f; // Adjust the fire rate for the rifle

    private bool isShooting = false;
    private bool isCoroutineRunning = false;

    public void StartShoot()
    {
        isShooting = true;
        if (!isCoroutineRunning)
        {
            StartCoroutine(FireRoutine());
        }
    }

    public void StopShoot()
    {
        isShooting = false;
    }

    private IEnumerator FireRoutine()
    {
        isCoroutineRunning = true;
        while (isShooting)
        {
            // Instantiate the bullet prefab at the shoot point position and rotation
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            // Access the Rigidbody component of the bullet
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            // Check if the bullet has a Rigidbody component
            if (bulletRb != null)
            {
                // Apply force to the bullet in the forward direction of the shoot point
                bulletRb.AddForce(shootPoint.forward * bulletForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(1f / fireRate);
        }
        isCoroutineRunning = false;
    }
}