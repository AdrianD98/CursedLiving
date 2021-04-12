using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manaSpawn : MonoBehaviour
{
    public GameObject mana;

    //platforms list
    List<Vector2> spawnPos = new List<Vector2>();


    private void Start()
    {
        InvokeRepeating("spawnMana", 0f, 10f);

        spawnPos.Add(new Vector2(5.2f, 7.8f));
        spawnPos.Add(new Vector2(12.2f, 1.3f));
        spawnPos.Add(new Vector2(-26f, 1.3f));
        spawnPos.Add(new Vector2(-19f,7.5f));
    }

    void spawnMana()
    {
        if (!GameObject.FindGameObjectWithTag("mana_crystal"))
        {
            int r = Random.Range(0, spawnPos.Count - 1);
            Instantiate(mana, spawnPos[r], transform.rotation);
        }
    }
}
