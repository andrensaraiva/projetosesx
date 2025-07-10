// EnemyStatus.cs
using UnityEngine;
using System.Collections;
public class EnemyStatus : MonoBehaviour
{
    private IControllableAI aiController;
    void Awake() { aiController = GetComponent<IControllableAI>(); }
    public void Freeze(float duration) { StartCoroutine(FreezeRoutine(duration)); }
    private IEnumerator FreezeRoutine(float duration) { aiController?.SetFrozen(true); yield return new WaitForSeconds(duration); aiController?.SetFrozen(false); }
    public void ApplySlow(float slowFactor, float duration) { StartCoroutine(SlowRoutine(slowFactor, duration)); }
    private IEnumerator SlowRoutine(float slowFactor, float duration)
    {
        if (aiController != null)
        {
            aiController.SetSpeed(aiController.GetOriginalSpeed() * slowFactor);
            yield return new WaitForSeconds(duration);
            aiController.ResetSpeed();
        }
    }
    public void Knockback(Vector2 direction, float force)
    {
        if (TryGetComponent<Rigidbody2D>(out var rb) && rb.bodyType == RigidbodyType2D.Dynamic) { rb.AddForce(direction * force, ForceMode2D.Impulse); }
    }
}