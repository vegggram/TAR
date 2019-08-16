using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public void ExitGame()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
