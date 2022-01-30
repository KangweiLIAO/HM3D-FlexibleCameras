using UnityEngine;

public class CameraController : MonoBehaviour {
    public Camera[] cameras;
    public float rotateSpeed = 100f;

    private int currCamIndex = 0;
    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start() {
        // initialization:
        Cursor.lockState = CursorLockMode.Locked;
        cameras = Camera.allCameras;
        Debug.Log("Camera Count: " + Camera.allCamerasCount);

        for (int i = 1; i < cameras.Length; i++) {
            cameras[i].gameObject.SetActive(false);
        }
        if (cameras.Length > 0) {
            cameras[0].gameObject.SetActive(true);
            Debug.Log("Camera with name: " + cameras[0].name + ", is now enabled");
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            currCamIndex++;
            Debug.Log("C button has been pressed. Switching to the next camera");
            if (currCamIndex < cameras.Length) {
                cameras[currCamIndex - 1].gameObject.SetActive(false);
                cameras[currCamIndex].gameObject.SetActive(true);
                Debug.Log("Camera with name: " + cameras[currCamIndex].name + ", is now enabled");
            } else {
                cameras[currCamIndex - 1].gameObject.SetActive(false);
                currCamIndex = 0;
                cameras[currCamIndex].gameObject.SetActive(true);
                Debug.Log("Camera with name: " + cameras[currCamIndex].name + ", is now enabled");
            }
        }

        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        xRotation -= mouseY;
        yRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameras[currCamIndex].transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);
        cameras[currCamIndex].transform.Rotate(Vector3.up * mouseX);
        cameras[currCamIndex].transform.Rotate(Vector3.left * mouseY);
    }
}
