using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Morph : MonoBehaviour
{
    //used to controll the morphing ability outside of the playerController as new units are generated with each death / power used

    //mana bar
    public Slider manaSlider;

    float cooldown = 7f;
    public int mana = 9;
    float timer;
    float spelllockDuration = 2f;

    playerController playerController;

    public void changeMana(int value)
    {
        mana += value;
        manaSlider.value = mana;
    }

    private void Start()
    {
        manaSlider.value = mana;
        InvokeRepeating("findController", 0, 0.1f);
        timer = Time.time;
    }

    private void findController()
    {
        if (FindObjectOfType<playerController>())
        {
            playerController = FindObjectOfType<playerController>();
            CancelInvoke();
        }
    }

    public void Spelllock()
    {
        timer = Time.time + spelllockDuration;
        StartCoroutine(changeColor());
    }

    IEnumerator changeColor()
    {
        playerController.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(spelllockDuration);
        playerController.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        checkMorph();
    }

    IEnumerator loopCharacters()
    {
        for (int i = 0; i < 3; i++)
        {

            playerController.Death();
            InvokeRepeating("findController", 0, 0.1f);
            yield return new WaitForSeconds(2f);
        }
    }

    private void checkMorph()
    {
        if (Input.GetKey(KeyCode.Q) && timer < Time.time && mana > 0)
        {
            StartCoroutine(loopCharacters());

            timer = Time.time + cooldown;
            changeMana(-1);
            manaSlider.value = mana;
            
        }
    }
}
