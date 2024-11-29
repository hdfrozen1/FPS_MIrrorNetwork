using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] CharacterController characterController;
    public float moveSpeed = 10f;

    [Header("Health")]
    [SyncVar(hook = nameof(TakeDame))]
    public float Health = 100f;

    [Header("Look Settings")]
    [SerializeField] float sensitivityX = 100f;
    [SerializeField] float sensitivityY = 100f;
    [SerializeField] float maxCameraX = 80f;
    [SerializeField] float minCameraX = -80f;
    float xRotation;

    public override void OnStartLocalPlayer()
    {
        if (!isLocalPlayer)
        {
            enabled = false; // Disable this script for non-local players
            return;
        }
        SetHealth();
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 1.197f, 0.312f);
        Camera.main.transform.rotation = Quaternion.identity;
        Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
    }

    private void Update()
    {
        if (!isLocalPlayer)
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

        if(Input.GetMouseButtonDown(0)){
            CmdPlayMuzzleFlash();
            RaycastAttack();
        }
    }
    [ClientCallback]
        private void RaycastAttack()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f))
            {
                PlayerMovement other = hit.transform.GetComponent<PlayerMovement>();
                if(other != null){
                    CmdAttack(other.gameObject);
                }
            }

        }
        [Command]
        private void CmdAttack(GameObject target){
            target.GetComponent<PlayerMovement>().Health -= 10;
        }

        void TakeDame(float _new,float old){
            Debug.Log("my name is:" + gameObject.name);
        }
        [Command]
        void SetHealth(){
            Health =100f;
        }
        [Command]
        void CmdPlayMuzzleFlash(){
            PlayMuzzleFlash();
        }
        [ClientRpc]
        void PlayMuzzleFlash(){
            //play muzzleflash
        }
}


