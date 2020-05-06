using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour, ICarController
{
    public UnityEngine.Vector3 _inputs;

    public Transform path;

    private WheelVehicleVr _wheelController;

    private List<AIDriverInformation> _nodes;
    private int _currentNode = 0;

    public float MaxSpeed = 100.0f;
    public float MaxSteeringAngle = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        _wheelController = GetComponent<WheelVehicleVr>();
        _inputs = UnityEngine.Vector3.zero;
        GetPath();
    }

    private void GetPath()
    {
        AIDriverInformation[] pathTransforms = path.GetComponentsInChildren<AIDriverInformation>();
        _nodes = new List<AIDriverInformation>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                _nodes.Add(pathTransforms[i]);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _inputs = UnityEngine.Vector3.zero;
        Drive();
        _inputs.x = CalculateSteering();
        _inputs.z = CalculateThrottle();
    }

    private void Drive()
    {
        bool timeToChangeNode = Vector3.Distance(transform.position, _nodes[_currentNode].transform.position) < 1.0f;
        if (timeToChangeNode) {
            bool lastNode = _currentNode == _nodes.Count - 1;
            if (lastNode)
            {
                _currentNode = 0;
            }
            else {
                _currentNode++;
            }
        }
    }

    private float CalculateSteering()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(_nodes[_currentNode].transform.position);
        float steer = relativeVector.x / relativeVector.magnitude;
        steer = steer * MaxSteeringAngle;
        return steer;
    }
    private float CalculateThrottle()
    {
        bool isAboveMaxSpeed = _wheelController.Speed >= MaxSpeed;
        bool isAboveAllowedSpeed = _wheelController.Speed >= _nodes[_currentNode].MaxSpeed;

        if (isAboveMaxSpeed) {
            return 0.0f;
        }
        else if(isAboveAllowedSpeed) {
            return -1.0f;
        }
        else {
            return 1.0f;
        }
    }

    public Vector3 GetInputs()
    {
        return _inputs;
    }
}
