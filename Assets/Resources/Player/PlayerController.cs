
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("                                    Player UI")]

    [SerializeField] private List<Button> btnBuySkins;
    [SerializeField] private TextMeshProUGUI textPlayerGold;
    [SerializeField] private TextMeshProUGUI textPlayerLevel;
    [SerializeField] private TextMeshProUGUI textPlayerStackCapacity;
    [SerializeField] private TextMeshProUGUI textPunchPower;
    [SerializeField] private Button btnBuyLevels;
    [SerializeField] private Button btnBuyPunchPower;
    [SerializeField] private FixedJoystick joystick;

    [Header("                                    Player Stuff")]
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private GameObject levelUPFloor;
    [SerializeField] public Player player;
    [NonSerialized] private int punchForce;
    [SerializeField] private Rigidbody _rigdbody;
    [SerializeField] private Animator _animator;
    [NonSerialized] public bool isPunching = false;
    [SerializeField] private List<Material> playerMaterials;

    private EnemyController enemyController;
    public void Start()
    {
        enemyController = FindFirstObjectByType<EnemyController>();
        player = new Player();
    }

    void FixedUpdate()
    {
        _animator.SetBool("punch", false);

        if (Input.GetKeyDown(KeyCode.W))
        {
            this.Punch();
        }

        #region Move Player (Mobile)
        _rigdbody.velocity = new Vector3(joystick.Horizontal * runSpeed, _rigdbody.velocity.y, joystick.Vertical * runSpeed);

        if (joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            this.transform.rotation = Quaternion.LookRotation(_rigdbody.velocity);
            _animator.SetBool("run", true);
        }
        else
        {
            _animator.SetBool("run", false);
        }
        #endregion
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("LevelUPFloor"))
        {
            StartCoroutine(DroppCorpsesForGold());
        }
    }

    public void Punch()
    {
        //Trigger Punch Animation and enemy reaction
        _animator.SetBool("punch", true);

        if (enemyController.isAnimationDone == true && isPunching == false)
        {
            isPunching = true;
            StartCoroutine(this.PunchDelay());
        }
    }

    public IEnumerator PunchDelay()
    {
        //Function to delay ragdoll animation
        yield return new WaitForSeconds(0.4f);

        enemyController.TriggerRagdoll(punchForce, transform.position);
        isPunching = false;
    }

    public void IncreaseGold(int numBodies)
    {
        player.gold += (50 * numBodies);
        textPlayerGold.text = player.gold.ToString();
    }

    public void BuyLevel()
    {
        //buying levels increase the stack capacity
        //max 15 levels
        if (player.gold >= 100 && player.level < 15)
        {
            player.gold -= 100;
            player.level += 1;
            player.stackBodiesLimit += 1;

            textPlayerGold.text = player.gold.ToString();
            textPlayerLevel.text = player.level.ToString();
            textPlayerStackCapacity.text = player.stackBodiesLimit.ToString();

            if (player.level == 15)
                btnBuyLevels.image.color = new Color32(150, 0, 30, 255);
        }
    }

    public void BuyPunchPower()
    {
        if (player.gold >= 100 && punchForce < 400)
        {
            punchForce += 50;
            player.gold -= 100;
            textPlayerGold.text = player.gold.ToString();
            textPunchPower.text = punchForce.ToString();

            if (punchForce == 400)
                btnBuyPunchPower.image.color = new Color32(150, 0, 30, 255);
        }
    }

    public void BuySkin(int index)
    {
        ChangePlayerSkin(index);
    }

    private void ChangePlayerSkin(int index)
    {
        if (btnBuySkins[index].image.color != new Color32(150, 0, 30, 255))
        {
            if (player.gold > 50)
            {
                player.gold -= 50;
                btnBuySkins[index].image.color = new Color32(150, 0, 30, 255);
                this.gameObject.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = playerMaterials[index];
                btnBuySkins[index].transform.GetComponentInChildren<TextMeshProUGUI>().text = "Sold";
            }
        }
        else
        {
            this.gameObject.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = playerMaterials[index];
        }
    }

    public IEnumerator DroppCorpsesForGold()
    {
        yield return new WaitForSeconds(1.5f);
        enemyController.RespawDeadEnemies();
    }
}
