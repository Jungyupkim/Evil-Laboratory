using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public GameObject minimapCamera, mapCamera;

    private bool mapActive;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Tab)) //if the player press tab, it will activate/deactivate the map
        {
            if (!mapActive)
            {
                ActivateMap();
            }
            else
            {
                DeactivateMap();
            }
        }
    }

    public void ChangeTarget (Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            mapActive = true;

            mapCamera.gameObject.SetActive(true);
            minimapCamera.gameObject.SetActive(false);
            UIController.instance.MapFrameON();
        }
    }

    public void DeactivateMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            mapActive = false;

            mapCamera.gameObject.SetActive(false);
            minimapCamera.gameObject.SetActive(true);
            UIController.instance.MapFrameOFF();
        }
    }
}
