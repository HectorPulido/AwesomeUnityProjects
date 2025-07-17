using UnityEngine;

public class InputManager
{
    public static float Horizontal {
        get {
            return Input.GetAxis("Horizontal");
        }
    }

    public static bool ShootDown {
        get{
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}
