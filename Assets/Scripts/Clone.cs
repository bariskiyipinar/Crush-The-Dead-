using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    [SerializeField] private GameObject m_Prefab;  // Klon prefab'�
    [SerializeField] private Transform m_ClonePosition;  // Klon konumu
    public static List<GameObject> clones = new List<GameObject>();  // Klonlar�n listesi

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Temas etti");
            // Klonlar aras�ndaki mesafeyi art�r�yoruz
            Vector3 cloneOffset = new Vector3(0, 0, 2);  // �ne do�ru bir mesafe ekleyelim
            GameObject newClone = Instantiate(m_Prefab, m_ClonePosition.position + cloneOffset, Quaternion.identity);
            clones.Add(newClone);

            // Klon objelerinin collider'lar�n� "Is Trigger" olarak ayarl�yoruz
            newClone.GetComponent<Collider>().isTrigger = true;
        }
    }
}
