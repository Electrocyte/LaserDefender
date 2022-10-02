using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] AudioClip shootingClip;
    [SerializeField] [Range(0f, 1f)] float shootingVolume = 1f;

    [Header("Damage")]
    [SerializeField] AudioClip damageClip;
    [SerializeField] [Range(0f, 1f)] float damageVolume = 1f;

    static AudioPlayer instance;

    private void Awake() {
        string currSceneName = SceneManager.GetActiveScene().name;
        ManageSingleton(currSceneName);
    }

    private void ManageSingleton(string currSceneName)
    {
        // if (currSceneName.Contains("Level")) {
        //     ;
        // } else {
            // Debug.Log(currSceneName);
            // Debug.Log(currSceneName.Contains("Level"));
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // } 
    }

    public void PlayShootingClip() {
        PlayClip(shootingClip, shootingVolume);
    }

    public void PlayDamageClip() {
        PlayClip(damageClip, damageVolume);
    }

    void PlayClip(AudioClip clip, float volume) {
        if (clip != null) {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, 
                                        cameraPos, 
                                        volume);
        }
    }
}
