using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private Camera _cam;
    [SerializeField] private float rotSpeed;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Vector3 dir = body.transform.position - hit.point;
            Vector3 direction = new Vector3(dir.x, 0, dir.z) ;
            Quaternion rotation = Quaternion.LookRotation(direction);
            body.transform.localRotation = Quaternion.Lerp(body.transform.rotation, rotation, rotSpeed * Time.deltaTime); ;
        }
    }

}
