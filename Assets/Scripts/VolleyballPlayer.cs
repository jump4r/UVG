using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role { OUTSIDE, SETTER, OPPOSITE };
public enum Team { RED, BLUE };
public class VolleyballPlayer : MonoBehaviour
{
    public Team team;
    public Role role;
}
