using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;
    
    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float varFiringRate = 0f;
    [SerializeField] float minFiringRate = 0.1f;
    [SerializeField] Vector3 offsetFiring = new Vector3(0f,0f,0f);

    [HideInInspector] public bool isFiring; 

    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;

    void Awake() {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (isFiring && firingCoroutine == null) {
            firingCoroutine = StartCoroutine(FireContinuously());
        } else if (!isFiring && firingCoroutine != null) {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            if (offsetFiring == new Vector3(0f,0f,0f))
            {
                GameObject instance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                ProjectileSpeed(instance);
            }
            else {
                GameObject instance = Instantiate(projectilePrefab, transform.position + offsetFiring, Quaternion.identity);
                ProjectileSpeed(instance);
            }

            float timeToNextProj = Random.Range(baseFiringRate - varFiringRate, 
                                            baseFiringRate + varFiringRate);
            timeToNextProj = Mathf.Clamp(timeToNextProj, minFiringRate, float.MaxValue);

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProj);
        }
    }

    private void ProjectileSpeed(GameObject instance)
    {
        Rigidbody2D rb2d = instance.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity = transform.up * projectileSpeed;
        }

        Destroy(instance, projectileLifetime);
    }
}
