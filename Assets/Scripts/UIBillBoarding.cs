using UnityEngine;

public class UIBillBoarding : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.forward  = mainCamera.transform.forward;
        transform.localScale = new Vector3(1, 1, 1) * (Vector3.Distance(mainCamera.transform.position, transform.position) /2f);
    }
}
