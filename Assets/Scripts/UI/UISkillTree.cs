using UnityEngine;

public class UISkillTree : MonoBehaviour
{
    public int skillPoints;


    public bool HasEnoughSkillPoints(int cost) {
        return skillPoints >= cost;
    }

    public void RemoveSkillPoints(int cost) {
        skillPoints -= cost;
    }
}
