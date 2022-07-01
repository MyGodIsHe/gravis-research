/*
 * Based on http://wiki.unity3d.com/index.php?title=MouseOrbitImproved
 */
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class DragMouseOrbit : MonoBehaviour
{

    public Vector3 target;
    public float distance = 5.0f;
    public float xSpeed = 500.0f;
    public float ySpeed = 1500.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .05f;
    public float distanceMax = 1000f;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * distance * 0.02f;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        distance = Zooming(distance);

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target;

        transform.rotation = rotation;
        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public float Zooming(float dist)
    {
        var modify = Input.GetAxis("Mouse ScrollWheel")*5;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            modify = 1*2;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            modify = -1*2;
        }

        return Mathf.Clamp(dist - modify, distanceMin, distanceMax);;
    }
    
}
