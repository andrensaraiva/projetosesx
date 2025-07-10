using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Encontra a c�mera principal uma vez para otimizar
        cameraTransform = Camera.main.transform;
    }

    // Usamos LateUpdate para garantir que o billboard s� gire depois que a c�mera se moveu
    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Faz o objeto olhar na dire��o da c�mera
        transform.LookAt(cameraTransform);

        // Corrige a rota��o para que ele n�o incline para cima ou para baixo, apenas no eixo Y
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}