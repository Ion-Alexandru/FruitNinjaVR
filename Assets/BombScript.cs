using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip bombExplosionSound;
    public GameObject bombExplosionPlarticle;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;

        if (gameObject.transform.position.y <= -1)
        {
            Destroy(gameObject);
        } else if(gameObject.transform.position.z < camera.transform.position.z - 1)
        {
            Destroy(gameObject);
        }
    }

    public void BombHit()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        audioSource.PlayOneShot(bombExplosionSound);

        Instantiate(bombExplosionPlarticle, transform.position, Quaternion.identity);

        StartCoroutine(BombExploded());
    }

    IEnumerator BombExploded()
    {
        ScoreScreenScript scoreScreenScript = FindAnyObjectByType<ScoreScreenScript>();

        yield return new WaitForSeconds(1.5f);

        GameManager.instance.AddFailures();
        GameManager.instance.AddFailures();
        GameManager.instance.AddFailures();

        if(scoreScreenScript != null)
        {
            scoreScreenScript.DestroyScoreScreen();
        }

        Destroy(gameObject);
    }
}
