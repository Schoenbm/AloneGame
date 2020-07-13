using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float verticalFall;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float maxY;
    public float timeEmote;
    public Rigidbody2D rb;
    public GameObject dialogueBox;
    public Pnj activePnj;


    private bool canMove = true;

    public Dictionary <string,bool>EmoteLearned;
    public Dictionary <string,Button> buttons;

    // Update is called once per frame
    private void Start()
    {
        // Active les bouttons en fonctions de ceux appris de base
        foreach (KeyValuePair<string,bool> kvp in EmoteLearned)
        {
            buttons[kvp.Key].enabled = false;
            buttons[kvp.Key].tag = kvp.Key;
        }
    }

    void Update()
    {
        if (canMove)
        {
            float velX = horizontalSpeed * Input.GetAxis("Horizontal");
            float velY = verticalSpeed * Input.GetAxis("Vertical") - verticalFall;

            if (this.transform.position.y > this.maxY)
            {
                velY = verticalSpeed * Input.GetAxis("Vertical") - verticalFall * 10;
            }

            rb.velocity = new Vector2(velX, velY);

            if (Input.GetAxis("Submit") >0)
            {
                foreach (KeyValuePair<string, bool> kvp in EmoteLearned)
                {
                    buttons[kvp.Key].enabled = kvp.Value;
                    buttons[kvp.Key].tag = kvp.Key;
                }
                canMove = false;
            }
        }
        else
        {
            if(Input.GetAxis("Cancel") > 0)
            {
                foreach (KeyValuePair<string, bool> kvp in EmoteLearned)
                {
                    buttons[kvp.Key].enabled = false;
                    buttons[kvp.Key].tag = kvp.Key;
                }
                canMove = true;
            }
        }
    }

    void sendEmote(string pString, GameObject pEmote)
    {
        canMove = true;
        GameObject emote = GameObject.Instantiate(pEmote,this.dialogueBox.transform);
        if(activePnj != null)
            activePnj.answer(this, pEmote);
        StartCoroutine(Wait(emote));
    }

    private IEnumerator Wait(GameObject pEmote)
    {
        yield return new WaitForSeconds(timeEmote);
        Destroy(pEmote);
        this.canMove = true;
    }

    public void learn(string pTag)
    {
        if(!EmoteLearned[pTag])
        {
            EmoteLearned[pTag] = true;
            buttons[pTag].enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        activePnj = collision.gameObject.GetComponent<Pnj>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == activePnj.gameObject)
            activePnj = null;
    }
}
