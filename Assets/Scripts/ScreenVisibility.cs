using UnityEngine;

public class ScreenVisibility : MonoBehaviour
{
    public GameObject screen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("MainCamera"))
        {
            screen.SetActive(false);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            screen.SetActive(true);
        }
    }

}
