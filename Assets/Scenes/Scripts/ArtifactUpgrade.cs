using UnityEngine;

/// <summary>
/// Quản lý việc nâng cấp Artifact (trụ) bao gồm level, máu tối đa và hiệu ứng animation.
/// </summary>
public class ArtifactUpgrade : MonoBehaviour
{
    [Header("Animator & Artifact")]
    public Animator animator;
    private Artifact artifact;
    private ArtifactLaserAttack laserAttack; 

    [Header("Player Inventory")]
    public PlayerUpgradeInventory playerInv;

    [Header("Upgrade Settings")]
    [SerializeField] private int[] upgradeCosts = { 0, 20, 30 };
    [SerializeField] private int[] maxHealthByLevel = { 0, 100, 200, 300 };

    [Header("Upgrade UI")]
    public UpgradeUI upgradeUI;

    private int level = 1;
    private bool isUpgrading = false;

    void Start()
    {
        level = Mathf.Clamp(level, 1, 3);
        animator.ResetTrigger("LevelUpTrigger");
        animator.SetInteger("level", level);

        artifact = GetComponent<Artifact>();
        laserAttack = GetComponent<ArtifactLaserAttack>();

        if (artifact != null)
            artifact.SetMaxHealth(maxHealthByLevel[level]);

        if (laserAttack != null)
            laserAttack.enabled = (level >= 3);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryUpgrade();
        }
    }

    void TryUpgrade()
    {
        if (isUpgrading)
        {
            upgradeUI?.ShowUpgrading();
            return;
        }

        if (level >= 3)
        {
            upgradeUI?.ShowTowerMaxLevel();
            return;
        }

        int cost = upgradeCosts[Mathf.Min(level, upgradeCosts.Length - 1)];

        if (!playerInv.UseStones(cost))
        {
            upgradeUI?.ShowNotEnoughStones();
            return;
        }

        isUpgrading = true;
        upgradeUI?.ShowUpgrading();

        animator.ResetTrigger("LevelUpTrigger");
        animator.SetTrigger("LevelUpTrigger");
    }

    // Gọi khi animation LevelUp kết thúc (từ Animation Event)
    public void OnLevelUpEffectEnd()
    {
        if (!isUpgrading)
        {
            return;
        }

        level = Mathf.Min(level + 1, 3);
        animator.SetInteger("level", level);

        // Cập nhật máu max
        if (artifact != null)
        {
            artifact.SetMaxHealth(maxHealthByLevel[level]);
        }

        isUpgrading = false;

        upgradeUI?.ShowUpgradeSuccessful();

        // Nếu đạt level 3, kích hoạt bắn laser
        if (laserAttack != null)
            laserAttack.enabled = (level >= 3);

    }
}
