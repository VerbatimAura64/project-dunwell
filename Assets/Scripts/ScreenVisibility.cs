using UnityEngine;

public class ScreenVisibility : MonoBehaviour
{
    public GameObject screen;

    private void OnTriggerEnter(GameObject other)
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
