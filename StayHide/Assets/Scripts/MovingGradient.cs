using UnityEngine;
public class MovingGradient : MonoBehaviour
{
    public Material material; // Material que será alterado
    public Gradient gradient; // Gradiente de cores
    public float duration = 5f; // Tempo para completar o ciclo do gradiente

    private float time;

    void Update()
    {
        if (material != null)
        {
            // Calcula o progresso do tempo (loop infinito)
            time += Time.deltaTime / duration;
            float t = Mathf.PingPong(time, 1f);

            // Aplica a cor do gradiente ao material
            material.color = gradient.Evaluate(t);
        }
    }
}
