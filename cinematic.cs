using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cinematic : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(endCinematic());
    }

    IEnumerator endCinematic()
    {
        yield return new WaitForSeconds(21);
            SceneManager.LoadScene(1);
        }
    }

