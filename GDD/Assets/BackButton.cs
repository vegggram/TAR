using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public AudioSource buttonAS;

    public void Back()
    {
        buttonAS.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + -2);
    }
}
