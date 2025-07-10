// IControllableAI.cs
public interface IControllableAI
{
    void SetFrozen(bool isFrozen);
    void SetSpeed(float newSpeed);
    void ResetSpeed();
    float GetOriginalSpeed();
}