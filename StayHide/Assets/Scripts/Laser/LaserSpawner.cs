using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject laser;
    private void Start()
    {
        StartCoroutine(spawnarLaser());
    }

    IEnumerator spawnarLaser()
    {
        yield return new WaitForSeconds((float)DifController.instance.getDiff());
        
        switch (escolherLado())
        {
            case 1:
                Vector2 vetor = new Vector2(10, 0);
                Instantiate(laser,vetor,Quaternion.Euler(0,0,0));
                break;
            case 2:
                Vector2 vetor2 = new Vector2(-10, 0);
                Instantiate(laser, vetor2, Quaternion.Euler(0, 0, 0));
                break;
            case 3:
                Vector2 vetor3 = new Vector2(0, 10);
                Instantiate(laser, vetor3, Quaternion.Euler(0, 0, 90));
                break;
            case 4:
                Vector2 vetor4 = new Vector2(0, -10);
                Instantiate(laser, vetor4, Quaternion.Euler(0, 0, 90));
                break;
        }

        StartCoroutine(spawnarLaser());

    }

    public int escolherLado()
    {
        return Random.Range(1,4);
    }
}
