using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] Transform upDown;
    Vector2 currentMouseLook;
    Vector2 appliedMouseDelta;
    public float sensitivity = 1;
    public float smoothing = 2;

    private void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        // Get smooth mouse look.
        Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * sensitivity * smoothing);
        appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smoothing);
        currentMouseLook += appliedMouseDelta;
        currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -20, 20);
        currentMouseLook.x = Mathf.Clamp(currentMouseLook.x, -20, 20);


        // Rotate camera and controller.
        transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
        upDown.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Rays");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 500f))
            {
                MenuButtons button = hit.transform.gameObject.GetComponent<MenuButtons>();

                if (button)
                {
                    button.OnClick();
                }
            }
        }
    }
}
