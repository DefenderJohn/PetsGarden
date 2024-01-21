using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public Vector2 moveInput;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x) * moveSpeed * Time.deltaTime;
            transform.position += moveDirection;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    public void OnViewportRotating(InputAction.CallbackContext context)
    {
        Vector2 rotationDelta = context.ReadValue<Vector2>();
        if (rotationDelta != Vector2.zero)
        {
            Quaternion rotationX = Quaternion.AngleAxis(-rotationDelta.y * rotationSpeed * Time.deltaTime, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(rotationDelta.x * rotationSpeed * Time.deltaTime, Vector3.up);

            this.transform.rotation *= rotationX;
            this.transform.rotation *= rotationY;

            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, 0);
        }
    }
}
