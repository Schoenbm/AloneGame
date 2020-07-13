using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject redHeartEmote;
    public GameObject dialogueBox;
    public float timeBeforeRed;
    public float timeEmotePlayer;
    public float timeEmoteGirl;
    public float timeBetweenEmote;
    public float timeBeforeBothEmotes;
    public Rigidbody girlRb;
    public float speed;

    public void End(Player pPlayer)
    {
        StartCoroutine(Emotes(pPlayer));
    }


    private IEnumerator Emotes(Player pPlayer)
    {
        GameObject emote;
        yield return new WaitForSeconds(timeBeforeRed);

        emote = GameObject.Instantiate(redHeartEmote, pPlayer.dialogueBox.transform);
        yield return new WaitForSeconds(timeEmotePlayer);
        Destroy(emote);

        yield return new WaitForSeconds(timeBetweenEmote);

        emote = GameObject.Instantiate(redHeartEmote, this.dialogueBox.transform);
        yield return new WaitForSeconds(timeEmoteGirl);
        Destroy(emote);

        yield return new WaitForSeconds(timeBeforeBothEmotes);

        GameObject.Instantiate(redHeartEmote, this.dialogueBox.transform);
        GameObject.Instantiate(redHeartEmote, pPlayer.dialogueBox.transform);
        pPlayer.rb.velocity = new Vector2(speed,0);
        girlRb.velocity = new Vector2(speed,0);

    }
}
