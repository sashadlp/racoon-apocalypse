using UnityEngine;

public class projectile : MonoBehaviour {
    public int degats = 1;          
    public float lifeTime = 5.0f;   

    private void Start() {
        Destroy(gameObject, lifeTime);
    } 
    
    void OnTriggerEnter2D(Collider2D truc) {
        Ennemi ennemi = truc.GetComponent<Ennemi>();
        
        if (ennemi != null)
        {
            ennemi.TakeDamage(degats); 
            Destroy(gameObject);      
        }
        else if (truc.CompareTag("cage")) {
            DestructibleCage cage = truc.GetComponent<DestructibleCage>();
            
            if (cage != null) {
               
                cage.LibererAnimal();
            } else {
                Destroy(truc.gameObject);
            }
            
            Destroy(gameObject);
        }
        else if (!truc.isTrigger && !truc.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}