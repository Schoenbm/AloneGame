using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pnj : MonoBehaviour
{
    public string goodAnswer;
    public GameObject goodEmote;
    public GameObject badEmote;

    public GameObject dialogueBox;

    private GameObject emote;

    public float timeEmote;
    public bool isGirl;

    public void answer(Player pPlayer, GameObject pEmote)
    {
        Debug.Log("Emote Received");
        string pString = pEmote.tag;
        if (pString.Equals(goodAnswer))
        {
            Debug.Log("GoodEmote");
            if (isGirl)
            {
                pPlayer.setCanMove(false);
                pPlayer.cameraTransform.setBlocked(true);
                this.GetComponent<EndGame>().Ending(pPlayer);
            }
            else if (goodEmote != null)
                emote = GameObject.Instantiate(goodEmote, this.dialogueBox.transform);
        }
        else
        {
            Debug.Log("BadEmote");
            if (isGirl)
                emote = GameObject.Instantiate(pEmote, this.dialogueBox.transform);
            else if(badEmote != null)
                emote = GameObject.Instantiate(badEmote, this.dialogueBox.transform);
        }

        if (emote != null)
            pPlayer.learn(emote.tag);
            StartCoroutine(Wait());

    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeEmote);
        Destroy(emote);
    }
}
