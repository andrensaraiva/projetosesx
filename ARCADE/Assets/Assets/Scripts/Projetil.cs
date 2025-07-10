using UnityEngine;

public class Projeti : MonoBehaviour
{
    public float speed = 20f;
    public float tempoDeVida = 2f; // Em segundos

    void Start()
    {
        // Destrói o projétil depois de um tempo para não poluir a cena
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // Move o projétil para cima
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // Se o projétil é um Trigger, usamos OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se colidir com uma bola (vamos criar a tag "Bola" depois)
        if (other.CompareTag("Bola"))
        {
            // Destrói o projétil ao colidir
            Destroy(gameObject);
        }
    }
}