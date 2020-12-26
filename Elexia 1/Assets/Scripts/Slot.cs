using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public bool over;
    public Letter letter;

    void Start()
    {
    }

    void Update()
    {
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        this.over=true;
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        this.over = false;
    }
}
