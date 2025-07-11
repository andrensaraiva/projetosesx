using UnityEngine;

public class DifController : MonoBehaviour
{
    public static DifController instance;

    private double dificult = 5;

    private float velocity = 50f;

    public void Awake()
    {
        instance = this;
    }

    public void addDiff()
    {
        if (dificult > 0.5)
        {
            dificult -= 0.2f;
            velocity += 5f;
        }
        else
        {
            return;
        }
        
    }
    public double getDiff()
    {
        return dificult;
    }

    public float getVelocity()
    {
        return velocity;
    }
}
