Ovale.defaut["PRIEST"] =
[[
# Define constants for easier addressing of spells
Define(SWP 589) # Shadow Word: Pain
Define(VT 34916) # Vampiric Touch
Define(VE 15286) # Vampiric Embrace
Define(SF 15473) # Shadowform
Define(MF 15407) # Mind Flay
Define(MB 8092) # Mind Blast
Define(DP 2944) # Devouring Plague
Define(SW 15257) # Shadow Weaving
Define(IF 48168) # Inner Fire
Define(Focus 14751) # Inner Focus
Define(Dispersion 47585)
Define(Shadowfiend 34433)

AddCheckBox(multidot L(multidot))

# Spells with cast time that add buff or debuff
SpellAddTargetDebuff(SWP SWP=18)
SpellInfo(SWP duration=18)
SpellAddBuff(SWP SW=15)
SpellAddTargetDebuff(VT VT=15)
SpellInfo(VT duration=15)
SpellAddBuff(VT SW=15)
SpellInfo(MF canStopChannelling=3)
SpellAddBuff(MF SW=15)
SpellInfo(MB cd=5.5)
SpellAddBuff(MB SW=15)
SpellAddBuff(IF IF=1800)
SpellAddTargetDebuff(DP DP=24)
SpellInfo(DP duration=24)
SpellInfo(Focus cd=180)
SpellInfo(Dispersion cd=120)
SpellInfo(Shadowfiend cd=300)
ScoreSpells(MB SWP VT DP MF)

# Add main monitor
AddIcon help=main
{

#Check shadowform is up
unless BuffPresent(SF)
    Spell(SF)

# Refresh inner fire
if BuffExpires(IF 60)
    Spell(IF)

#if inner focus is active, cast mind blast
if BuffPresent(Focus)
    Spell(MB)

# Check if Shadow Weave is stacked 5 times
# before suggesting Shadow Word: Pain
if BuffPresent(SW stacks=5) and TargetDebuffExpires(SWP 0 mine=1) and TargetDeadIn(more 6)
{
   Spell(SWP)
}

#Refresh VT
unless CheckBoxOn(multidot) and OtherDebuffPresent(VT)
{
	if TargetDebuffExpires(VT 1.4 mine=1 haste=spell) and TargetDeadIn(more 8)
		Spell(VT)
}

#cast MB if up
Spell(MB)

#Refresh devouring plague
unless CheckBoxOn(multidot) and OtherDebuffPresent(DP)
{
	if TargetDebuffExpires(DP 0 mine=1) and TargetDeadIn(more 8)
		Spell(DP)
}

if CheckBoxOn(multidot) and OtherDebuffExpires(SWP)
	Texture(INV_Misc_Coin_01)

#cast Mind flay if nothing else can be done
Spell(MF priority=2)

} # End of main monitor

]]
