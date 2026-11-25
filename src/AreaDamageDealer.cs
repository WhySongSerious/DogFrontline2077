using System.Collections;
using UnityEngine;

public class AreaDamageDealer : MonoBehaviour
{
    public float interval = 5f;
    public int damage = 100;

    public GameObject damageAllEffect; // "DamageAll" 파티클 오브젝트
    public float effectDuration = 1.5f;

    private void Start()
    {
        StartCoroutine(DamageLoop());
    }

    IEnumerator DamageLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // 파티클 이펙트 켜기
            if (damageAllEffect != null)
            {
                damageAllEffect.SetActive(true);
                StartCoroutine(DisableEffectAfterDelay(effectDuration));
            }

            // 데미지 적용 (화면에 보이는 로봇만)
            ApplyDamageToVisibleRobots();
        }
    }

    bool IsVisibleToCamera(GameObject obj)
{
    Vector3 viewportPos = Camera.main.WorldToViewportPoint(obj.transform.position);
    return viewportPos.z > 0f && viewportPos.x >= 0f && viewportPos.x <= 1f
           && viewportPos.y >= 0f && viewportPos.y <= 1f;
}


    void ApplyDamageToVisibleRobots()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // 로봇인지 확인
if (obj.layer == LayerMask.NameToLayer("Robot") && obj.activeInHierarchy)
{
    if (IsVisibleToCamera(obj))
    {
        RobotController robot = obj.GetComponent<RobotController>();
        if (robot != null)
        {
            robot.TakeDamage(damage);
        }
    }
}

        }
    }

    IEnumerator DisableEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (damageAllEffect != null)
            damageAllEffect.SetActive(false);
    }
}
