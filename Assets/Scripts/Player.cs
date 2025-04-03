using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float horizontalSpeed = 5f;
    public GameObject Vehicle;
    private float leftClamp = -2.3f;
    private float rightClamp = 2.3f;

    private void FixedUpdate()
    {
        transform.Translate(0, 0, speed * Time.fixedDeltaTime);
        MovePlayer();
    }

    void MovePlayer()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float middleScreen = Screen.width / 2;

            if (touch.position.x < middleScreen)
            {
                transform.position = new Vector3(leftClamp, transform.position.y, transform.position.z);
            }
            else if (touch.position.x > middleScreen)
            {
                transform.position = new Vector3(rightClamp, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Klonlarý silme iþlemi
        if (other.gameObject.CompareTag("DestroyClone"))
        {
            if (Clone.clones.Count > 1)
            {
                int previousIndex = Clone.clones.Count - 1;

                // Son klonu yok etme ve listeden çýkarma
                Destroy(Clone.clones[previousIndex]);
                Clone.clones.RemoveAt(previousIndex);
            }
        }

        // Obstacle (engel) ile çarpýþma
        if (other.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(VehicleTime(0.02f));
        }
    }

    IEnumerator VehicleTime(float time)
    {
        
        yield return new WaitForSeconds(time);
        
        Instantiate(Vehicle, transform.position, Quaternion.Euler(0, -159.21f, 0));
        Destroy(this.gameObject);

        Vehicle.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(3f);
        Vehicle.GetComponentInChildren<ParticleSystem>().Stop();
    }

    
}
