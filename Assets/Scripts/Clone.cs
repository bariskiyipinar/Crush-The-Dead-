using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    [SerializeField] private GameObject m_Prefab;  // Klon prefab'ý
    [SerializeField] private Transform m_ClonePosition;  // Klon konumu
    public static List<GameObject> clones = new List<GameObject>();  // Klonlarýn listesi

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Temas etti");
            // Klonlar arasýndaki mesafeyi artýrýyoruz
            Vector3 cloneOffset = new Vector3(0, 0, 2);  // Öne doðru bir mesafe ekleyelim
            GameObject newClone = Instantiate(m_Prefab, m_ClonePosition.position + cloneOffset, Quaternion.identity);
            clones.Add(newClone);

            // Klon objelerinin collider'larýný "Is Trigger" olarak ayarlýyoruz
            newClone.GetComponent<Collider>().isTrigger = true;
        }
    }
}
