using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class START : MonoBehaviour
{
    public void Startgame()
    {
        Debug.Log("Let's go");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    // Start is called before the first frame update
    //void Start()
         

    // Update is called once per frame
   // void Update()
   
}
