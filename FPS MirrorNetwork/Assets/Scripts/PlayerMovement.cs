using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] CharacterController characterController;
    public float moveSpeed = 10f;

    
    [Header("dame")]
    public float Dame;

    [Header("Look Settings")]
    [SerializeField] float sensitivityX = 100f;
    [SerializeField] float sensitivityY = 100f;
    [SerializeField] float maxCameraX = 80f;
    [SerializeField] float minCameraX = -80f;
    float xRotation;


    public override void OnStartAuthority()
    {
        if (!isOwned)
        {
            enabled = false; // Disable this script for non-local players
            return;
        }

        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 1.197f, 0.312f);
        Camera.main.transform.rotation = Quaternion.identity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isOwned)
        {
            return;
        }



        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * x + transform.forward * z;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        transform.Rotate(Vector3.up, mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minCameraX, maxCameraX);
        //weaponArm.localEulerAngles = new Vector3(xRotation, 0, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (Input.GetMouseButtonDown(0))
        {
            // play muzzle flash
            RaycastAttack();
        }
    }
    [ClientCallback]
    private void RaycastAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f))
        {
            HealthController other = hit.transform.GetComponent<HealthController>();
            if (other != null)
            {
                Debug.Log("other is not null");
                CmdAttack(other,Dame);
            }
        }

    }

    [Command(requiresAuthority = false)]
    private void CmdAttack(HealthController target,float dame){
        Debug.Log("Hello");
        target.TakeDame(dame);
    }



}


