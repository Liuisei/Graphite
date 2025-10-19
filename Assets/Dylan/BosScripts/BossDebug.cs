using UnityEngine;

public class BossDebug : MonoBehaviour
{
    public BossManager bossManager;

    void Update()
    {
        // Going to Phase 2 : shields destroyed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (var part in bossManager.shields)
            {
                part.TakeDamage(part.MaxHP, null);
            }
        }

        // Going to Phase 3 : lampion + mouth destroyed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var part in bossManager.lampionAndMouth)
            {
                part.TakeDamage(part.MaxHP, null);
            }
        }

        // Going to final Phase : Body destroyed
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (bossManager.body != null)
            {
                bossManager.body.TakeDamage(bossManager.body.MaxHP, null);
            }
        }
    }
}
