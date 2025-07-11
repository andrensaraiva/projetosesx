using UnityEngine;

public class Pontos : MonoBehaviour
{
    private bool colidiu = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            colidiu = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            colidiu = false;
        }
    }

    private void Update()
    {
        if (colidiu && JumpManager.instance.onGround)
        {
            Dados.instance.addPonto();
            DifController.instance.addDiff();
            Destroy(this.gameObject);
        }
    }
}
