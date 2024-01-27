using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    public float recoilAmount = 1f;
    public float recoilSpeed = 2f;
    private float recoil = 0f;

    void Update()
    {
        // Apply the recoil effect
        if (recoil > 0)
        {
            Quaternion maxRecoil = Quaternion.Euler(-recoil, 0, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0;
            Quaternion minRecoil = Quaternion.Euler(0, 0, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, minRecoil, Time.deltaTime * recoilSpeed);
        }
    }

    public void AddRecoil()
    {
        recoil += recoilAmount;
    }
}
