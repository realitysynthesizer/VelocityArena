using UnityEngine;

public class controller : MonoBehaviour
{
    internal enum driveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [SerializeField]private driveType drive;

    private inputManager IM;
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] wheelMesh = new GameObject[4];
    private Rigidbody rigidbody;
    public float KPH;
    public float radius=6;
    public float torque = 400f;
    public float steeringMax = 20;
    public float turningSpeedRatio=1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getObjects();
    }


    void FixedUpdate()
    {
        animateWheels();
        moveVehicle();
        steerVehicle();
    }

    private void moveVehicle()
    {
        float totalPower;
        if(drive==driveType.allWheelDrive)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = IM.vertical*(torque/4);
            }
        }
        else if(drive==driveType.rearWheelDrive)
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = IM.vertical*(torque/2);
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length-2; i++)
            {
                wheels[i].motorTorque = IM.vertical*(torque/2);
            }
        }
        KPH = rigidbody.linearVelocity.magnitude * 3.6f;
        
        
    }

    private void steerVehicle()
    {       if (IM.horizontal > 0) {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * IM.horizontal*turningSpeedRatio/(turningSpeedRatio+KPH);
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * IM.horizontal*turningSpeedRatio/(turningSpeedRatio+KPH);
        } else if (IM.horizontal < 0) {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * IM.horizontal*turningSpeedRatio/(turningSpeedRatio+KPH);
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * IM.horizontal*turningSpeedRatio/(turningSpeedRatio+KPH);
        } else {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }


    void animateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for(int i=0; i<4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }
    public void getObjects()
    {
        IM = GetComponent<inputManager>();
        rigidbody = GetComponent<Rigidbody>();
    }
}
