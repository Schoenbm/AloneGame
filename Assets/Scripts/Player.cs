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
        buttons[0].gameObject.SetActive(false);
        buttonsDict.Add(emotes[0].tag, buttons[0]);
        emoteLearned.Add(emotes[0].tag, true);
        emotesDict[emotes[0].tag] = emotes[0];

        for (int k = 1; k< buttons.Length; k++)
        {
            buttons[k].gameObject.SetActive(false);
            buttonsDict.Add(emotes[k].tag, buttons[k]);
            emoteLearned.Add(emotes[k].tag, false);
            emotesDict[emotes[k].tag] = emotes[k];
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
                rb.velocity = new Vector2(0,0);
                foreach (KeyValuePair<string, bool> kvp in emoteLearned)
                {
                    buttonsDict[kvp.Key].gameObject.SetActive(kvp.Value);
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
                    buttonsDict[kvp.Key].gameObject.SetActive(false);
                }
                canMove = true;
            }
        }
    }

    public void sendEmote(string pString)
    {
        foreach (KeyValuePair<string, bool> kvp in emoteLearned)
        {
            buttonsDict[kvp.Key].gameObject.SetActive(false);
        }
        canMove = true;
        GameObject emote = GameObject.Instantiate(this.emotesDict[pString],this.dialogueBox.transform);
        StartCoroutine(Wait(emote,pString));
    }

    private IEnumerator Wait(GameObject pEmote, string pString)
    {
        yield return new WaitForSeconds(timeEmote);
        Destroy(pEmote);
        this.canMove = true;
        if (activePnj != null)
            Debug.Log("emote Sent");
        activePnj.answer(this, this.emotesDict[pString]);
    }

    public void learn(string pTag)
    {
        if (!emoteLearned.ContainsKey(pTag))
        {
            return;
        }
        if(!emoteLearned[pTag])
        {
            emoteLearned[pTag] = true;
            buttonsDict[pTag].enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponent<Pnj>() != null) {
            activePnj = collision.gameObject.GetComponent<Pnj>();
            activePnj.transform.localScale = 1.1f * activePnj.transform.localScale;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (activePnj != null)
        {
            activePnj.transform.localScale = activePnj.transform.localScale / 1.1f;
            activePnj = null;
        }
    }
}
