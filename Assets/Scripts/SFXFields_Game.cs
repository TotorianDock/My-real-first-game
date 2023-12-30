using UnityEngine;
public class SFXFields_Game : SFXSystem
{
	public void HitSound()
	{
        ClipsName _hitSound = (ClipsName)Random.Range((float)ClipsName.Hit1, (float)ClipsName.Hit9 + 1);
		PlaySound(sounds[(int)_hitSound], 0.25f);
	}
	public void EnemyDeathSound()
	{
		PlaySound(sounds[(int)ClipsName.EnemyDeath], 0.3f);
	}
    public void HeroDeathSound()
    {
        PlaySound(sounds[(int)ClipsName.HeroDeath], 0.3f);
    }
	public void LevelUpSound()
	{
        ClipsName _lvlUpSound = (ClipsName)Random.Range((float)ClipsName.LevelUp1, (float)ClipsName.LevelUp2 + 1);
        PlaySound(sounds[(int)_lvlUpSound], 0.1f);
    }
	public void FinalBatleMusic()
	{
        ClipsName _finalBatleMusic = (ClipsName)Random.Range((float)ClipsName.FinalBatle1, (float)ClipsName.FinalBatle2 + 1);
        PlayMusic(sounds[(int)_finalBatleMusic], 0.12f);
    }
	public void BatleMusic()
	{
        ClipsName _batleMusic = (ClipsName)Random.Range((float)ClipsName.BatleMusic1, (float)ClipsName.BatleMusic4 + 1);
		float _batleVolume;
		
		if ((int)_batleMusic > (int)ClipsName.BatleMusic2)
			_batleVolume = 0.05f;
        else
			_batleVolume = 0.12f;
        PlayMusic(sounds[(int)_batleMusic], _batleVolume);
    }
	public void HealSound()
	{
        PlaySound(sounds[(int)ClipsName.Heal], 0.4f);
    }
    public void BlockSound()
    {
        PlaySound(sounds[(int)ClipsName.Block], 1f);
    }
    public void DebuffSound()
    {
        PlaySound(sounds[(int)ClipsName.Debuff], 0.3f);
    }
	public void ShieldUpSound()
	{
        ClipsName _shieldUpSound = (ClipsName)Random.Range((float)ClipsName.ShieldUp1, (float)ClipsName.ShieldUp2 + 1);
        PlaySound(sounds[(int)_shieldUpSound], 0.5f);
    }
	public void FailedAttackSound()
	{
        ClipsName _failedAttackSound = (ClipsName)Random.Range((float)ClipsName.FailedAttack1, (float)ClipsName.FailedAttack2 + 1);
        PlaySound(sounds[(int)_failedAttackSound], 0.5f);
    }

	private enum ClipsName
	{
		HeroDeath,
		EnemyDeath,
		
		LevelUp1,
		LevelUp2,
		
		BatleMusic1,
        BatleMusic2,
        BatleMusic3,
		BatleMusic4,
		
		FinalBatle1,
		FinalBatle2,
		
		Hit1,
		Hit2,
		Hit3,
		Hit4,
		Hit5,
		Hit6,
		Hit7,
		Hit8,
		Hit9,

		Heal,
		Block,
		Debuff,

        ShieldUp1,
		ShieldUp2,

		FailedAttack1,
		FailedAttack2
	}
}
