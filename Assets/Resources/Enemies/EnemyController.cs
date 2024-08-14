using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemyController : MonoBehaviour
{
    [Header("                                    Enemy Stuff")]
    public float distanceToHit;
    [SerializeField] private GameObject[] _enemiesPrefabArray;
    [NonSerialized] public GameObject tmpEnemy;
    [SerializeField] private List<GameObject> _enemyList;
    [SerializeField] private List<Rigidbody[]> _ragDollRigdBodysList;
    [NonSerialized] public List<bool> ragDollPhysicsDone;
    [NonSerialized] public float distance;
    [NonSerialized] public float distanceX;
    [NonSerialized] public float distanceZ;
    [NonSerialized] public List<int> objIndex;
    [NonSerialized] public float tmpDistance;
    [NonSerialized] public bool isAnimationDone = true;
    [NonSerialized] private Rigidbody _hitEnemyRigidbody;
    [NonSerialized] private PlayerController _playerController;

    void Start()
    {
        _playerController = FindFirstObjectByType<PlayerController>();
        ragDollPhysicsDone = new List<bool>();
        _ragDollRigdBodysList = new List<Rigidbody[]>();
        _enemyList = new List<GameObject>();
        InstantiateEnemies(true);
        DisableAllRagdolls();
    }

    private void DisableAllRagdolls()
    {
        //Disable ragdoll physics on all enemys
        for (int i = 0; i < _enemyList.Count; i++)
        {
            foreach (var rigdbody in _ragDollRigdBodysList[i])
            {
                rigdbody.isKinematic = true;
            }
        }
    }

    public void RespawDeadEnemies()
    {
        _playerController.gameObject.GetComponentInChildren<StackingBodies>().CleanPileOfCorpses();

        for (int i = 0; i <= ragDollPhysicsDone.Count; i++)
        {
            if (i > 0)
            {
                i--;
            }

            if (ragDollPhysicsDone[i] == true
                && _enemyList[i].GetComponentInChildren<CorpseMovement>().isBodyOnPile == true)
            {
                _enemyList[i].GetComponentInChildren<CorpseMovement>().isActive = false;
                _ragDollRigdBodysList.RemoveAt(i);
                _enemyList.RemoveAt(i);
                ragDollPhysicsDone.RemoveAt(i);
                _playerController.IncreaseGold(1);
                InstantiateEnemies(false);
            }
            else
            {
                i++;
            }
        }
    }

    private void EnableRagdoll(int index)
    {
        //Enable ragdoll physics on punched enemies
        _enemyList[index].GetComponent<Animator>().enabled = false;

        foreach (var rigdbody in _ragDollRigdBodysList[index])
        {
            rigdbody.isKinematic = false;
        }

        StartCoroutine(DisableRagdoll(index));
    }

    private IEnumerator DisableRagdoll(int index)
    {
        isAnimationDone = true;

        //Wait for ragdoll physics to complete (on the ground), then deactivate.
        yield return new WaitForSeconds(1.8f);
        _ragDollRigdBodysList[index].FirstOrDefault().isKinematic = true;
        ragDollPhysicsDone[index] = true;
    }

    public bool CheckRagdollPhysicsDone(GameObject enemy)
    {
        //Return if physics is done (False or True)
        if (_enemyList.Count > 0 && _enemyList.Contains(enemy))
        {
            return ragDollPhysicsDone[_enemyList.IndexOf(enemy)];
        }

        return false;
    }

    public void TriggerRagdoll(int forceMagnitude, Vector3 playerPos)
    {
        //Get the first enemy collider and force an impulse in the opposite direction of the player facing direction

        distance = 20f;
        objIndex = new List<int>();
        for (int i = 0; i < _enemyList.Count; i++)
        {
            if (_enemyList[i].GetComponent<Animator>().enabled == true)
            {
                //Check and Get the closest enemies from the player
                tmpDistance = Vector3.Distance(playerPos, _enemyList[i].transform.position);

                if (tmpDistance < distanceToHit)
                {
                    objIndex.Add(i);
                }
            }
        }

        for (int i = 0; i < objIndex.Count; i++)
        {
            isAnimationDone = false;
            _hitEnemyRigidbody = _ragDollRigdBodysList[objIndex[i]].FirstOrDefault();
            distanceX = Mathf.Abs(playerPos.x - _enemyList[objIndex[i]].transform.position.x);
            distanceZ = Mathf.Abs(playerPos.z - _enemyList[objIndex[i]].transform.position.z);

            EnableRagdoll(objIndex[i]);
            if (distanceX < distanceZ)
            {
                _hitEnemyRigidbody.AddForceAtPosition(new Vector3(0f, forceMagnitude, (_playerController.transform.forward.z * forceMagnitude)), _hitEnemyRigidbody.position, ForceMode.Impulse);
            }
            else
            {
                _hitEnemyRigidbody.AddForceAtPosition(new Vector3((_playerController.transform.forward.x * forceMagnitude), forceMagnitude, 0f), _hitEnemyRigidbody.position, ForceMode.Impulse);
            }
        }
    }

    public void InstantiateEnemies(bool isStart)
    {
        if (isStart)
        {
            //Instantiate all prefab enemies on the start
            for (int i = 0; i < _enemiesPrefabArray.Length; i++)
            {
                tmpEnemy = (GameObject)Instantiate(_enemiesPrefabArray[i], new Vector3(Random.Range(-22.00024f, -14.0f), 0.12f, Random.Range(26, 14f)), Quaternion.identity);
                tmpEnemy.transform.SetParent(this.transform);
                _enemyList.Add(tmpEnemy);
            }

            for (int i = 0; i < _enemyList.Count; i++)
            {
                _ragDollRigdBodysList.Add(_enemyList[i].GetComponentsInChildren<Rigidbody>());
                ragDollPhysicsDone.Add(false);
            }
        }
        else
        {
            tmpEnemy = (GameObject)Instantiate(_enemiesPrefabArray[0], new Vector3(Random.Range(-22.00024f, -15.0f), 0.12f, Random.Range(26, 14f)), Quaternion.identity);
            tmpEnemy.transform.SetParent(this.transform);
            _enemyList.Add(tmpEnemy);

            _ragDollRigdBodysList.Add(tmpEnemy.GetComponentsInChildren<Rigidbody>());
            ragDollPhysicsDone.Add(false);
        }

    }
}
