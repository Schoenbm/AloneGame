using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public CameraTransform cameraTransform;

    private bool canMove = true;
    private bool inMenu = false;

    public Button[] buttons;
    public GameObject[] emotes;

    private Dictionary<string, bool> emoteLearned = new Dictionary<string, bool>();
    private Dictionary<string, Button> buttonsDict = new Dictionary<string, Button>();
    private Dictionary<string, GameObject> emotesDict = new Dictionary<string, GameObject>();

    private int currentButton;
    public Cursor activeCursor;

    // Update is called once per frame
    private void Start()
    {
        activeCursor.gameObject.SetActive(false);

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
        inMenu = false;
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
                activeCursor.gameObject.SetActive(true);
                currentButton = 0;
                this.activeCursor.changePosition(this.buttons[currentButton].gameObject);
                rb.velocity = new Vector2(0,0);
                foreach (KeyValuePair<string, bool> kvp in emoteLearned)
                {
                    buttonsDict[kvp.Key].gameObject.SetActive(kvp.Value);
                }
                EventSystem.current.SetSelectedGameObject(this.buttons[0].gameObject);
                canMove = false;
                inMenu = true;
            }
        }
        else if (inMenu) // IF IT CANNOT MOVE
        {
            if(Input.GetKeyDown("left") || Input.GetKeyDown("q"))
            {
                for(int i = currentButton - 1; i >= 0; i--)
                {
                    if (buttons[i].gameObject.activeSelf) {
                        EventSystem.current.SetSelectedGameObject(this.buttons[i].gameObject);
                        currentButton = i;
                        break;
                    }
                }
                this.activeCursor.changePosition(this.buttons[currentButton].gameObject);
            }
            else if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
            {
                for (int i = currentButton + 1; i < buttons.Length; i++)
                {
                    if (buttons[i].gameObject.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(this.buttons[i].gameObject);
                        currentButton = i;
                        break;
                    }
                }
                this.activeCursor.changePosition(this.buttons[currentButton].gameObject);
            }


            if (Input.GetAxis("Cancel") > 0)
            {
                activeCursor.gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                foreach (KeyValuePair<string, bool> kvp in emoteLearned)
                {
                    buttonsDict[kvp.Key].gameObject.SetActive(false);
                }
                canMove = true;
                inMenu = false;

            }
        }
    } // UPDATE ---------------------------

    public void sendEmote(string pString)
    {
        activeCursor.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        foreach (KeyValuePair<string, bool> kvp in emoteLearned)
        {
            buttonsDict[kvp.Key].gameObject.SetActive(false);
        }
        inMenu = false;
        GameObject emote = GameObject.Instantiate(this.emotesDict[pString],this.dialogueBox.transform);
        StartCoroutine(Wait(emote,pString));
    }

    private IEnumerator Wait(GameObject pEmote, string pString)
    {
        yield return new WaitForSeconds(timeEmote);
        Destroy(pEmote);
        this.canMove = true;
        if (activePnj != null)
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
            activePnj.transform.localScale = 1.05f * activePnj.transform.localScale;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (activePnj != null)
        {
            activePnj.transform.localScale = activePnj.transform.localScale / 1.05f;
            activePnj = null;
        }
    }

    public void setCanMove(bool pBool)
    {
        this.canMove = pBool;
    }
}
