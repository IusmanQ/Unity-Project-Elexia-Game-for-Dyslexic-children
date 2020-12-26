using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omi_slot : MonoBehaviour
{
    public bool over;
    public Omi_Letter letter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Omi_Letter>().selected)
            this.over = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        this.over = false;
    }
}
