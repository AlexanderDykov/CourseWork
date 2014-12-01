using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour {

    public Transform backImage;
    public Transform frontImage;

    float radius = 1.0f;

    public float Dist()
    {
        float dist = Vector2.Distance(backImage.position, frontImage.position);
        return dist;
    }

    public int JoyTochRight()
    {
        if (frontImage.position.x < backImage.position.x)
            return -1;
        else if (frontImage.position.x > backImage.position.x) return 1;
        return 0;
    }
    public Quaternion Rotation(Vector2 worldP)
    {
        Vector3 moveDirection;
        frontImage.position = Vector2.ClampMagnitude(new Vector2(worldP.x - backImage.position.x, worldP.y - backImage.position.y), radius) + new Vector2(backImage.position.x, backImage.position.y);
        
        moveDirection = frontImage.position - backImage.position;
        moveDirection.z = 0;
        moveDirection.Normalize();
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, targetAngle);
    }

    public Vector3 Move(Vector2 worldP)
    {
        Vector3 moveDirection;
        frontImage.position = Vector2.ClampMagnitude(new Vector2(worldP.x - backImage.position.x, worldP.y - backImage.position.y), radius) + new Vector2(backImage.position.x, backImage.position.y);
        moveDirection = frontImage.position - backImage.position;
        moveDirection.z = 0;
        return moveDirection;        
    }

    public void JoystickReset()
    {
        frontImage.localPosition = new Vector3(0, 0, frontImage.position.z);
    }
}
