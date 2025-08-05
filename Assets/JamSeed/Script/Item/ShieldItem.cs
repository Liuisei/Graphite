using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Shield existingShield = other.GetComponentInChildren<Shield>();

        if (existingShield != null)
        {
            existingShield.Refresh();
        }
        else
        {
            // プレハブの回転からPlayerの回転を引く（オイラー角）
            Vector3 shieldEuler = shieldPrefab.transform.eulerAngles;
            Vector3 playerEuler = other.transform.eulerAngles;
            Vector3 finalEuler = shieldEuler - playerEuler;
            Quaternion finalRotation = Quaternion.Euler(finalEuler);

            GameObject newShield = Instantiate(
                shieldPrefab,
                other.transform.position,
                finalRotation,
                other.transform
            );

            newShield.transform.localPosition = Vector3.zero;
        }

        Destroy(gameObject);
    }
}