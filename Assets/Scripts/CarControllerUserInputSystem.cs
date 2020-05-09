using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerUserInputSystem : MonoBehaviour
{

    private CarControllerSystem m_Car; // the car controller we want to use
    public UnityEngine.Vector3 _inputs;
    private void Start()
    {
        _inputs = UnityEngine.Vector3.zero;
        // get the car controller
        m_Car = GetComponent<CarControllerSystem>();
    }

    private void FixedUpdate()
    {
        UnityEngine.Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        float v = Input.GetAxis("Vertical");
        if (primaryAxis.y != 0.0f)
            v = primaryAxis.y;

        float h = Input.GetAxis("Horizontal");
        if (primaryAxis.x != 0.0f)
            h = primaryAxis.x;

        m_Car.Move(h, v, v, 0f);

        _inputs.z = v;
        _inputs.x = h;
    }

    public Vector3 GetInputs()
    {
        return _inputs;
    }
}
