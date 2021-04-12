using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvanceLevel : MonoBehaviour
{
    public Animator animator;

    //goal
    public GameObject target;



    // Start is called before the first frame update
    private void Update()
    {

        if(!target)
        StartCoroutine(nextScene());

    }

    IEnumerator nextScene()
    {
        animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(2);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
