using UnityEngine;

public enum TargetType { Pass, Set, Hit, Serve };
public class BallTarget : MonoBehaviour
{
    public TargetType targetType;
    public Role role;
}
