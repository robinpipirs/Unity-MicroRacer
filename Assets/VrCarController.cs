 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrCarController : MonoBehaviour, ICarController
{
    public UnityEngine.Vector3 _inputs;



    // Start is called before the first frame update
    void Start()
    {
        _inputs = UnityEngine.Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _inputs = UnityEngine.Vector3.zero;
        UnityEngine.Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        _inputs.z = Input.GetAxis("Vertical");
        if (primaryAxis.y != 0.0f)
            _inputs.z = primaryAxis.y;

        _inputs.x = Input.GetAxis("Horizontal");
        if (primaryAxis.x != 0.0f)
            _inputs.x = primaryAxis.x;
    }

    public Vector3 GetInputs()
    {
        return _inputs;
    }
}
