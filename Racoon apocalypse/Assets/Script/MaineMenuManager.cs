using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("level1"); 
    }
    public void QuitGame()
    {
        Debug.Log("Le jeu se ferme..."); 

        Application.Quit(); 
    }
}