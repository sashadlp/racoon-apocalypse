using UnityEngine;

public class Gameloop : MonoBehaviour
{
    public bool isGameRunning = false;
    public GameObject victoryScreen;
   

    public Transform PlayerTransform { get; private set; }

    public static Gameloop Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerTransform = playerObj.transform;
        }
    }

    public void Start() 
    {
        isGameRunning = true;
        Time.timeScale = 1f; 
        
        if (victoryScreen != null) victoryScreen.SetActive(false);

        Debug.Log("Partie lancée ! Objectif : Atteindre la zone de victoire.");
    }

    public void Update() 
    {
        if (!isGameRunning) return;
    }
    
    public void VictoryCondition() 
    {
        if (isGameRunning)
        {
            TriggerVictory();
        }
    }

    public void TriggerVictory() 
    {
        isGameRunning = false;
        Time.timeScale = 0f; // On fige le jeu
        
        if (victoryScreen != null)
        {
            VictoryScreen screenScript = victoryScreen.GetComponent<VictoryScreen>();
            
            int scoreFinal = 0;
            if (ScoreManager.instance != null)
            {
                scoreFinal = ScoreManager.instance.GetScore();
            }

            screenScript.Show(scoreFinal);
        }
        
        Debug.Log("Victoire ! damien a pu se sauver ");
    }
}