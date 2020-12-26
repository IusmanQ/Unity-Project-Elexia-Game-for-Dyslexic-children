using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public Slot slot;
    public Vector2 origPosition;
    void Start() {
        origPosition = this.gameObject.transform.position;
        
    }

    void Update()
    {
        
    }
    public void AssignSlot(Slot new_slot)
    {
        if (new_slot.letter == null)
        { // make sure new slot is empty
            //new_slot.letter = this; // assign this letter to new slot
            transform.position = new_slot.transform.position; // assign this position of new slot position
            //if (slot)
                //slot.letter = null; // empty the space for new letters
            //slot = new_slot; // assingning
        }
    }
    void OnMouseDown()
    {
        for (int i = 0; i < Game.inst.slots.Length; i++)
        {
            if (this == Game.inst.slots[i].letter)
               {
                 Game.inst.slots[i].letter = null;
               }
        }
        Game.letterPickAudio.clip = (AudioClip)Resources.Load("Audio files/DM-CGS-40");
        Game.letterPickAudio.Play();
    }

    void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    void OnMouseUp()
    {
        int filledSlots = 0;
        this.gameObject.transform.position = origPosition; // if drop anywhere in free space then move it back
        for (int i = 0; i < Game.inst.slots.Length; i++)
        {
            if (Game.inst.slots[i].over.Equals(true))
            {
                Game.letterPickAudio.clip = (AudioClip) Resources.Load("Audio files/DM-CGS-03");
                Game.letterPickAudio.Play();
                Debug.Log("i m over");
                Game.inst.slots[i].over = false;
                if (Game.inst.slots[i].letter != null)
                {
                    Game.inst.slots[i].letter.transform.position = Game.inst.slots[i].letter.origPosition;
                    Game.inst.slots[i].letter = null;
                }
                this.gameObject.transform.position = Game.inst.slots[i].transform.position;
                Game.inst.slots[i].letter = this;

                if (filledSlots == Game.target)
                    Game.inst.checkGameCondition();
            }
            else if (Game.inst.slots[i].over.Equals(false))
            {
                Debug.Log(" i m not");
            }
            if (Game.inst.slots[i].letter != null)
                filledSlots++;
        }
        if (filledSlots == Game.target)
            Game.inst.checkGameCondition();
    }
}
