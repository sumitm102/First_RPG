using UnityEngine;

public class SkillDash : SkillBase
{
    public void OnStartEffect() {
        if(IsUpgradeUnlocked(E_SkillUpgradeType.Dash_CloneOnStart) || IsUpgradeUnlocked(E_SkillUpgradeType.Dash_CloneOnStartAndArrival)) 
            CreateClone();
        

        if (IsUpgradeUnlocked(E_SkillUpgradeType.Dash_ShardOnStart) || IsUpgradeUnlocked(E_SkillUpgradeType.Dash_ShardOnStartAndArrival)) 
            CreateShard();
        
    }

    public void OnEndEffect() {
        if (IsUpgradeUnlocked(E_SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();

        if (IsUpgradeUnlocked(E_SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }
    
    private void CreateShard() {
        PlayerSkillManager.ShardSkill.CreateRawShard();
    }

    private void CreateClone() {
        Debug.Log("Created clone");
    }
}