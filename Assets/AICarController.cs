using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
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

    [Header("Sensors")]
    public float SensorLength = 5.0f;
    public float FrontSensorAngle = 30.0f;
    public float FrontSensorDistance= 0.5f;
    public float FrontSensorForwardDistance = 1.33f;
    public Vector3 FrontSensorPosition;

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
        Sensors();
        _inputs = UnityEngine.Vector3.zero;
        _inputs.x = CalculateSteering();
        _inputs.z = CalculateThrottle();
    }

    private void Sensors()
    {
        

        // Center sensor
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * FrontSensorPosition.z;
        sensorStartPos += transform.up * FrontSensorPosition.y;

        float centerSensor = getCenterSensor(sensorStartPos);
        float leftSensor = getLeftSensor(sensorStartPos);
        float leftSensorOutw = getLeftSensorOutward(sensorStartPos);
        float rightSensor = getRightSensor(sensorStartPos);
        float rightSensorOutw = getRightSensorOutward(sensorStartPos);
    }

    private float getCenterSensor(Vector3 sensorStartPos)
    {
        RaycastHit hit;

        Vector3 startPosition = sensorStartPos;
        if (Physics.Raycast(startPosition, transform.forward, out hit, SensorLength))
        {
            Debug.DrawLine(startPosition, hit.point);
        }

        return 0;
    }

    private float getLeftSensor(Vector3 sensorStartPos)
    {
        RaycastHit hit;
        Vector3 startPosition = Vector3.zero;
        startPosition = sensorStartPos + (transform.right * FrontSensorDistance);
        if (Physics.Raycast(startPosition, transform.forward, out hit, SensorLength)) { 
            Debug.DrawLine(startPosition, hit.point);
        }

        return 0.0f;
    }

    private float getLeftSensorOutward(Vector3 sensorStartPos)
    {
        RaycastHit hit;
        Vector3 startPosition = Vector3.zero;
        startPosition = sensorStartPos - (transform.right * FrontSensorDistance );
        if (Physics.Raycast(startPosition, Quaternion.AngleAxis(-1*FrontSensorAngle, transform.up) * transform.forward, out hit, SensorLength))
        {
            Debug.DrawLine(startPosition, hit.point);
        }

        return 0.0f;
    }

    private float getRightSensor(Vector3 sensorStartPos)
    {
        RaycastHit hit;
        Vector3 startPosition = Vector3.zero;
        startPosition = sensorStartPos - (transform.right * FrontSensorDistance);
        if (Physics.Raycast(startPosition, transform.forward, out hit, SensorLength))
        {
            Debug.DrawLine(startPosition, hit.point);
        }

        return 0.0f;
    }

    private float getRightSensorOutward(Vector3 sensorStartPos)
    {
        RaycastHit hit;
        Vector3 startPosition = Vector3.zero;
        startPosition = sensorStartPos + (transform.right * FrontSensorDistance);
        if (Physics.Raycast(startPosition, Quaternion.AngleAxis(FrontSensorAngle, transform.up) * transform.forward, out hit, SensorLength))
        {
            Debug.DrawLine(startPosition, hit.point);
        }
        return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        AIDriverInformation aIDriverInformation = other.GetComponent<AIDriverInformation>();

        bool timeToChangeNode = aIDriverInformation == _nodes[_currentNode];
        if (timeToChangeNode) {
            bool lastNode = _currentNode == _nodes.Count - 1;
            if (lastNode)
            {
                _currentNode = 0;
            }
            else
            {
                _currentNode++;
            }
        }
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
