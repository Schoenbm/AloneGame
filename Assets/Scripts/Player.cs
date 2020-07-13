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

    public Button[] buttons;
    public GameObject[] Emotes;

    private Dictionary <string,bool>EmoteLearned;
    private Dictionary <string,Button> buttonsDict;

    // Update is called once per frame
    private void Start()
    {
        buttonsDict.Add(Emotes[0].tag, buttons[0]);
        EmoteLearned.Add(Emotes[0].tag, true);

        for (int k = 1; k< buttons.Length; k++)
        {
            buttonsDict.Add(Emotes[k].tag, buttons[k]);
            EmoteLearned.Add(Emotes[k].tag, false);
        }

    }

    void Update()
    {
        if (canMove)
        {
            float velX = horizontalSpeed * Input.GetAxis("Horizontal");

            if (Input.GetAxis("Horizontal") < 0)
            {
                this.transform.localScale = new Vector3(-1,1,1);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
            }


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
                    buttonsDict[kvp.Key].enabled = kvp.Value;
                    buttonsDict[kvp.Key].tag = kvp.Key;
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
                    buttonsDict[kvp.Key].enabled = false;
                    buttonsDict[kvp.Key].tag = kvp.Key;
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
            buttonsDict[pTag].enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Pnj>() != null)
            activePnj = collision.gameObject.GetComponent<Pnj>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (activePnj != null)
            activePnj = null;
    }
}
