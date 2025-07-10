using UnityEngine;

public class VisualEffects : MonoBehaviour {
    public ParticleSystem shootEffect;

    public void PlayShootEffect(Vector3 position) {
        shootEffect.transform.position = position;
        shootEffect.Play();
    }
}