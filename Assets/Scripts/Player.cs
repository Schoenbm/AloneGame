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
    public GameObject[] emotes;

    private Dictionary<string, bool> emoteLearned = new Dictionary<string, bool>();
    private Dictionary<string, Button> buttonsDict = new Dictionary<string, Button>();
    private Dictionary<string, GameObject> emotesDict = new Dictionary<string, GameObject>();

    // Update is called once per frame
    private void Start()
    {
        buttonsDict.Add(emotes[0].tag, buttons[0]);
        emoteLearned.Add(emotes[0].tag, true);

        for (int k = 1; k< buttons.Length; k++)
        {
            buttonsDict.Add(emotes[k].tag, buttons[k]);
            emoteLearned.Add(emotes[k].tag, false);
        }

        canMove = true;
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
                foreach (KeyValuePair<string, bool> kvp in emoteLearned)
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
                foreach (KeyValuePair<string, bool> kvp in emoteLearned)
                {
                    buttonsDict[kvp.Key].enabled = false;
                    buttonsDict[kvp.Key].tag = kvp.Key;
                }
                canMove = true;
            }
        }
    }

    void sendEmote(string pString)
    {
        canMove = true;
        GameObject emote = GameObject.Instantiate(this.emotesDict[pString],this.dialogueBox.transform);
        if(activePnj != null)
            activePnj.answer(this, this.emotesDict[pString]);
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
        if(!emoteLearned[pTag])
        {
            emoteLearned[pTag] = true;
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
