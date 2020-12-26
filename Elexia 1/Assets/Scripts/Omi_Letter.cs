using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Omi_Letter : MonoBehaviour
{
    public bool selected; // means which is picked by user
    public Omi_slot slot;
    public Vector2 origPosition;
    public Vector2 tempSlotPosition;
    public int updatedSlotsCount = 0;
    private float slotsGap = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        origPosition = this.gameObject.transform.position;
        tempSlotPosition = Omi_Game.inst.tmp_slot.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AssignSlot(Omi_slot new_slot)
    {
        if (new_slot.letter == null)
        { // make sure new slot is empty
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
        Omi_Game.letterPickAudio.clip = (AudioClip)Resources.Load("Audio files/DM-CGS-40");
        Omi_Game.letterPickAudio.Play();
    }

    void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        int score = 0;
        int indexCheck = 0;
        this.gameObject.transform.position = slot.transform.position; // if drop anywhere in free space then move it back
        if (Omi_Game.inst.tmp_slot.over.Equals(true))
        {
            AssignSlot(Omi_Game.inst.tmp_slot);
            print("current letters");
            for (int i = 0; i < Omi_Game.inst.slots.Length; i++)
            {
                if (Omi_Game.inst.slots[i].letter != null)
                {
                    print(Omi_Game.inst.slots[i].letter.GetComponent<SpriteRenderer>().sprite.name);
                    Omi_Game.inst.updatedslots[i - indexCheck] = Omi_Game.inst.slots[i];
                }
                else
                {
                    if (i >= 0)
                    {
                        indexCheck++;
                    }
                    //Destroy(Omi_Game.inst.slots[i].gameObject);
                    Omi_Game.inst.slots[i].gameObject.SetActive(false);
                }
            }

            GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Omi_slot");
            float slotSize = slotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            Vector2 slotposition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 4)) - new Vector3(slotSize * (Omi_Game.inst.slots.Length / 2f) - slotSize / 2 - ((slotsGap - 1) / 2f), 0, 0);
            Vector2 lettersPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 1.89f, Screen.height / 4)) - new Vector3((Omi_Game.inst.slots.Length - 1 / 2f) * slotsGap / 2 - ((slotsGap - Omi_Game.inst.letters.Length - 1) / (slotsGap * 10f)), 0, 0);

            for (int i = 0; i < Omi_Game.inst.slots.Length; i++)
            {
                if (Omi_Game.inst.slots[i].letter != null)
                {
                    Omi_Game.inst.slots[i].transform.position = slotposition;
                    
                    slotposition += new Vector2(slotSize + slotsGap, 0);
                    Omi_Game.inst.slots[i].gameObject.SetActive(false);

                    Omi_Game.inst.slots[i].letter.transform.position = lettersPosition;
                    lettersPosition += new Vector2(slotsGap + slotSize, 0);
                }
            }
            for (int i = 0; i < Omi_Game.inst.slots.Length; i++)
            {
                if (Omi_Game.inst.slots[i].letter != null)
                {
                    print("letter in slot: " + i + " " + Omi_Game.inst.slots[i].letter.GetComponent<SpriteRenderer>().sprite.name);
                }
                //if (Omi_Game.inst.slots[i].letter != null && Omi_Game.inst.slots[i].letter.GetComponent<SpriteRenderer>().sprite.name == Omi_Game.inst.wordList[i])
                //    score++;
            }


            print("length of updated slots: " + Omi_Game.inst.updatedslots.Length);
            print("updated slots letters");
            for (int i = 0; i < Omi_Game.inst.updatedslots.Length; i++)
            {
                if (Omi_Game.inst.updatedslots[i] != null)
                {
                    print(i + ": " + Omi_Game.inst.updatedslots[i].letter.GetComponent<SpriteRenderer>().sprite.name);
                }
            }

            for (int i = 0; i < Omi_Game.inst.updatedslots.Length; i++)
            {
                if (Omi_Game.inst.updatedslots[i] != null && Omi_Game.inst.updatedslots[i].letter.GetComponent<SpriteRenderer>().sprite.name == Omi_Game.inst.wordList[i])
                    score++;
            }
            Omi_Game.inst.checkGameCondition();
        }


        
    }
}
