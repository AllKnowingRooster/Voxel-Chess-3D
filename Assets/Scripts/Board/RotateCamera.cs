using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    private float rotateSpeed = 20.0f;
    [SerializeField] private InputActionAsset inputActionAsset;
    private InputActionMap inputActionMap;
    private InputAction inputAction;
    private string inputActionMapName;
    private string InputActionName;
    private void Awake()
    {
        inputActionMapName = "Player";
        InputActionName = "Camera Control";
    }

    private void Start()
    {
        inputActionMap = inputActionAsset.FindActionMap(inputActionMapName);
        inputAction = inputActionMap.FindAction(InputActionName);
        inputActionAsset.Enable();
        inputActionMap.Enable();
        inputAction.Enable();
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputValue = inputAction.ReadValue<float>();

        if (inputValue!=0.0f)
        {
            transform.Rotate(Vector3.up * inputValue * rotateSpeed * Time.deltaTime);
        }      
    }
}
