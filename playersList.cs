using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playersList : MonoBehaviour
{
    //list of different prefabs that the user can use
    public List<GameObject> players = new List<GameObject>();

    public int counter;
    public int maxCounter = 0;

    public void increaseCounter()
    {
        maxCounter++;
        counter = maxCounter - 1;
    }

    public void setCounter()
    {
        counter++;
        if (counter > maxCounter) counter = 0;
    }
}
