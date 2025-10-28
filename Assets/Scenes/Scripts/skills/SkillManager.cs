using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public DashSkill dash;
    public SpinAttackSkill spin;
    public BoomerangSkill boomerang;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) dash?.TryUse();
        if (Input.GetKeyDown(KeyCode.Q))         spin?.TryUse();
        if (Input.GetKeyDown(KeyCode.E))         boomerang?.TryUse();
    }
}