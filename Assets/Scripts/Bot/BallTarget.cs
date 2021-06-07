using UnityEngine;

public enum TargetType { Pass, Set, Hit };
public class BallTarget : MonoBehaviour
{
    public TargetType targetType;
    public Role role;
}
