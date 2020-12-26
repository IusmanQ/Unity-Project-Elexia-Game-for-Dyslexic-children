using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Sub_Letter : MonoBehaviour
{
    public bool selected; // means which is picked by user
    public Vector2 origPosition;
    public Vector2 tempSlotPosition;
    public Sub_slot slot;
   

    void Start()
    {
        origPosition = this.gameObject.transform.position;
        tempSlotPosition = Sub_Game.inst.tmp_slot.transform.position;

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignSlot (Sub_slot new_slot)
    {
        if (new_slot.letter == null) { // make sure new slot is empty
            new_slot.letter = this; // assign this letter to new slot
            transform.position = new_slot.transform.position; // assign this position of new slot position
            if (slot)
                slot.letter = null; // empty the space for new letters
            slot = new_slot; // assingning
        }
    }

    void OnMouseDown()
    {

        selected = true;
        //enable all colliders
        Sub_Game.inst.tmp_slot.GetComponent<Collider2D>().enabled = true;
        for (int i = 0; i < Sub_Game.inst.slots.Length; i++)
            Sub_Game.inst.slots[i].GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sortingOrder = 10;

        Sub_Game.letterPickAudio.clip = (AudioClip)Resources.Load("Audio files/DM-CGS-40");
        Sub_Game.letterPickAudio.Play();
    }

    void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    void OnMouseUp()
    {
        int score = 0;
        this.gameObject.transform.position = slot.transform.position; // if drop anywhere in free space then move it back
        if (Sub_Game.inst.tmp_slot.over.Equals(true))
            AssignSlot(Sub_Game.inst.tmp_slot);
        for (int i = 0; i < Sub_Game.inst.slots.Length; i++)
            if (Sub_Game.inst.slots[i].over.Equals(true))
                AssignSlot(Sub_Game.inst.slots [i]);
        
        selected = false; // at the end bcz after logic completes
        //disable all colliders
        Sub_Game.inst.tmp_slot.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < Sub_Game.inst.slots.Length; i++)
            Sub_Game.inst.slots[i].GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sortingOrder = 4;

        for (int i = 0; i < Sub_Game.inst.slots.Length; i++)
        {
            if (Sub_Game.inst.slots[i].letter != null && Sub_Game.inst.slots[i].letter.GetComponent<SpriteRenderer>().sprite.name == Sub_Game.inst.letters[i].GetComponent<SpriteRenderer>().sprite.name)
                score++;
        }

        if(score == Sub_Game.inst.slots.Length)
        {
            Sub_Game.inst.checkGameCondition();
        }
    }
}
