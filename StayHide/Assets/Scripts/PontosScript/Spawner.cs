using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pts;

    private void Start()
    {
        StartCoroutine(spawnar()); 
    }

    IEnumerator spawnar()
    {
        Vector2 local = new Vector2(Random.Range( -3f,3f),Random.Range(-3,3));
        Instantiate(pts, local, Quaternion.identity);
        yield return new WaitForSeconds(5);
        StartCoroutine(spawnar());
    }
}
