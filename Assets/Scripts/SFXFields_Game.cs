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
		PlaySound(sounds[(int)ClipsName.DeathEnemy], 0.3f);
	}
    public void HeroDeathSound()
    {
        PlaySound(sounds[(int)ClipsName.DeathHero], 0.3f);
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
		DeathHero = 0,
		DeathEnemy = 1,
		
		LevelUp1 = 2,
		LevelUp2 = 3,
		
		BatleMusic1 = 4,
        BatleMusic2 = 5,
        BatleMusic3 = 6,
		BatleMusic4 = 7,
		
		FinalBatle1 = 8,
		FinalBatle2 = 9,
		
		Hit1 = 10,
		Hit2 = 11,
		Hit3 = 12,
		Hit4 = 13,
		Hit5 = 14,
		Hit6 = 15,
		Hit7 = 16,
		Hit8 = 17,
		Hit9 = 18,

		Heal = 19,
		Block = 20,
		Debuff = 21,

        ShieldUp1 = 22,
		ShieldUp2 = 23,

		FailedAttack1 = 24,
		FailedAttack2 = 25
	}
}
