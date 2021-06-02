using UnityEngine;

public enum TargetType { Pass, Set, Hit };
public class BallTarget : MonoBehaviour
{
    public TargetType type;
    public Role role;
}
