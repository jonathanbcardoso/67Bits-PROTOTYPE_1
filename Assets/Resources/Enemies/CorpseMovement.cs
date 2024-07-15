using System;
using System.Collections;
using UnityEngine;

public class CorpseMovement : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [NonSerialized] public bool isActive = true;
    [NonSerialized] public bool isBodyOnPile = false;

    public void UpdateCorpsePosition(Transform followedCorpse, bool isFollowStart)
    {
        StartCoroutine(StartFollowingLastCorpsePosition(followedCorpse, isFollowStart));
    }

    IEnumerator StartFollowingLastCorpsePosition(Transform followedCorpse, bool isFollowStart)
    {
        while (isFollowStart)
        {
            yield return new WaitForEndOfFrame();
            if (isActive == true)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, followedCorpse.position.x, followSpeed * Time.deltaTime),
                    transform.position.y,
                    Mathf.Lerp(transform.position.z, followedCorpse.position.z, followSpeed * Time.deltaTime));

                isBodyOnPile = true;
            }
            else
            {
                Destroy(transform.parent.gameObject);
                break;
            }
        }
    }
}
