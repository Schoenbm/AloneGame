using UnityEngine.SceneManagement;
using UnityEngine;

public class END : MonoBehaviour
{
 public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
