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
}
