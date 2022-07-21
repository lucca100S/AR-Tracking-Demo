using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARControl : MonoBehaviour
{
    public GameObject reference; // tracking device (VR control or haptic device) that will be used as a reference to determine AR Tracking's precision
    private ConnectToServerSocket conectionServerSocket;
    private Vector3 positionReference = Vector3.zero;   // reference's position during calibration
    private Vector3 positionAR = Vector3.zero;          // AR Tracking's position during calibration
    private Quaternion rotationReference = Quaternion.identity;   // reference's rotation during calibration
    private Quaternion rotationAR = Quaternion.identity;          // AR Tracking's rotation during calibration
    private Vector3 ARCurrentPosition;                 // AR Tracking's position during the test
    private Quaternion ARCurrentRotation;              // AR Tracking's rotation during the test

    public void setConnection(ConnectToServerSocket conexaoServerSocketAR)
    {
        conectionServerSocket = conexaoServerSocketAR;
    }


    private void Update()
    {
        if (conectionServerSocket)
        {
            if (conectionServerSocket.JsonInfo.success)
            {
                if (Input.GetKeyDown(KeyCode.C))   // Starts calibration of AR Tracking based on the reference device
                {
                    positionReference = reference.transform.position;
                    positionAR = new Vector3(-conectionServerSocket.JsonInfo.translation_x / 50, -conectionServerSocket.JsonInfo.translation_y / 50, -conectionServerSocket.JsonInfo.translation_z / 50);
                    rotationReference = reference.transform.rotation;
                    Vector3 ARup = new Vector3(conectionServerSocket.JsonInfo.rotation_up_x, conectionServerSocket.JsonInfo.rotation_up_y, conectionServerSocket.JsonInfo.rotation_up_z);
                    Vector3 ARforward = new Vector3(conectionServerSocket.JsonInfo.rotation_forward_x, conectionServerSocket.JsonInfo.rotation_forward_y, conectionServerSocket.JsonInfo.rotation_forward_z);
                    rotationAR = Quaternion.LookRotation(ARforward, ARup);
                    print("Calibrated!");
                }

                conectionServerSocket.step = conectionServerSocket.speed * Time.deltaTime;
                //print("Msg recebida:" + " Tx: " + conectionServerSocket.JsonInfo.translation_x + " ;Ty: " + conectionServerSocket.JsonInfo.translation_y + " ;Tz: " + conectionServerSocket.JsonInfo.translation_z);

                ARCurrentPosition = new Vector3(-conectionServerSocket.JsonInfo.translation_x / 50, -conectionServerSocket.JsonInfo.translation_y / 50, -conectionServerSocket.JsonInfo.translation_z / 50);
                transform.localPosition = (ARCurrentPosition - positionAR) + positionReference;  // Determines the new position of AR Tracking based on the refence's position during calibration

                Vector3 up = new Vector3(conectionServerSocket.JsonInfo.rotation_up_x, conectionServerSocket.JsonInfo.rotation_up_y, conectionServerSocket.JsonInfo.rotation_up_z);
                Vector3 forward = new Vector3(conectionServerSocket.JsonInfo.rotation_forward_x, conectionServerSocket.JsonInfo.rotation_forward_y, conectionServerSocket.JsonInfo.rotation_forward_z);
                
                ARCurrentRotation = Quaternion.LookRotation(forward, up);
                transform.localRotation = (ARCurrentRotation * Quaternion.Inverse(rotationAR)) * rotationReference;   // Determines the new rotation of AR Tracking based on the refence's rotation during calibration
            }
        }
    }
}
