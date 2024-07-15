using System.Collections.Generic;
using UnityEngine;

public class StackingBodies : MonoBehaviour
{
    private float _xMove;
    private float _zMove;
    private Vector3 _firstCorpsePos;
    private Vector3 _currentCorpsePos;
    public bool isActive = true;
    //
    [SerializeField] private float _speed;
    //
    List<GameObject> _corpseList = new List<GameObject>();
    private int _corpseListIndexCounter = 0;

    private EnemyController enemyController;

    private void OnTriggerEnter(Collider other)
    {
        //Check if player is close to the enemy corpse using the collider, then add the enemy body to the pile of corpses    
        if (other.CompareTag("Enemy") && isActive == true)
        {
            enemyController = FindFirstObjectByType<EnemyController>();

            if (_corpseList.Count < transform.parent.GetComponent<PlayerController>().player.stackBodiesLimit
                && other.gameObject.GetComponent<Rigidbody>().isKinematic == true
                && _corpseList.Contains(other.gameObject) == false 
                && enemyController.CheckRagdollPhysicsDone(other.gameObject.transform.parent.gameObject) == true)
            { 
                _corpseList.Add(other.gameObject);

                if (_corpseList.Count == 1)
                {
                    _firstCorpsePos = GetComponent<SkinnedMeshRenderer>().bounds.max;
                    _currentCorpsePos = new Vector3(other.transform.position.x, _firstCorpsePos.y, other.transform.position.z);
                    other.gameObject.transform.position = _currentCorpsePos;
                    _currentCorpsePos = new Vector3(other.transform.position.x, this.transform.position.y + 2f, other.transform.position.z);
                    other.gameObject.GetComponent<CorpseMovement>().UpdateCorpsePosition(transform, true);

                }
                else if (_corpseList.Count > 1)
                {
                    other.gameObject.transform.position = _currentCorpsePos;
                    _currentCorpsePos = new Vector3(other.transform.position.x, other.gameObject.transform.position.y + 0.5f, other.transform.position.z);
                    other.gameObject.GetComponent<CorpseMovement>().UpdateCorpsePosition(_corpseList[_corpseListIndexCounter].transform, true);
                    _corpseListIndexCounter++;
                }
            }
        }
    }

    public void CleanPileOfCorpses()
    {
        _corpseList.Clear();
        _corpseListIndexCounter = 0;
    }
}
