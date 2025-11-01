using UnityEngine;

public class SkillDash : SkillBase
{
    public void OnStartEffect() {
        if(Unlocked(E_SkillUpgradeType.Dash_CloneOnStart) || Unlocked(E_SkillUpgradeType.Dash_CloneOnStartAndArrival)) 
            CreateClone();
        

        if (Unlocked(E_SkillUpgradeType.Dash_ShardOnStart) || Unlocked(E_SkillUpgradeType.Dash_ShardOnStartAndArrival)) 
            CreateShard();
        
    }

    public void OnEndEffect() {
        if (Unlocked(E_SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();

        if (Unlocked(E_SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }
    
    private void CreateShard() {
        Debug.Log("Created time shard");
    }

    private void CreateClone() {
        Debug.Log("Created clone");
    }
}