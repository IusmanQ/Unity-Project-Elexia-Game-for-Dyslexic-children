using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_slot : MonoBehaviour
{
    public bool over;
    public Sub_Letter letter;

    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Sub_Letter>().selected)
            this.over = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        this.over = false;
    }
}
