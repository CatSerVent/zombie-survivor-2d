using UnityEngine;

public class JoystickInput : MonoBehaviour
{
    public SimpleJoystick joystick;
    PlayerController pc;

    void Awake() { pc = GetComponent<PlayerController>(); }
    void Update() { pc.SetInput(joystick ? joystick.Value : Vector2.zero); }
}

