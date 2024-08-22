﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Library;
using Library.Network;
using Library.SystemModels;
using Server.DBModels;
using Server.Envir;
using Server.Models.Monsters;
using S = Library.Network.ServerPackets;


namespace Server.Models
{
    public class MonsterObject : MapObject
    {
        public override ObjectType Race => ObjectType.Monster;

        public sealed override MirDirection Direction { get; set; }

        public DateTime SearchTime, RoamTime, EXPOwnerTime, DeadTime, RageTime, TameTime;

        public TimeSpan SearchDelay = TimeSpan.FromSeconds(3),
                        RoamDelay = TimeSpan.FromSeconds(2),
                        EXPOwnerDelay = TimeSpan.FromSeconds(5);

        public MonsterInfo MonsterInfo;

        public SpawnInfo SpawnInfo;
        public int DropSet;


        public MapObject Target
        {
            get { return _Target; }
            set
            {
                if (_Target == value) return;

                _Target = value;

                if (_Target == null)
                    SearchTime = DateTime.MinValue;
            }
        }
        private MapObject _Target;

        public bool PlayerTagged;

        public Dictionary<MonsterInfo, int> SpawnList = new Dictionary<MonsterInfo, int>();
        public bool Skeleton;

        #region EXPOwner

        public PlayerObject EXPOwner
        {
            get { return _EXPOwner; }
            set
            {
                if (_EXPOwner == value) return;

                PlayerObject oldValue = _EXPOwner;
                _EXPOwner = value;

                OnEXPOwnerChanged(oldValue, value);
            }
        }
        private PlayerObject _EXPOwner;
        public virtual void OnEXPOwnerChanged(PlayerObject oValue, PlayerObject nValue)
        {
            oValue?.TaggedMonsters.Remove(this);

            nValue?.TaggedMonsters.Add(this);
        }

        #endregion

        public Dictionary<AccountInfo, List<UserItem>> Drops;

        public virtual decimal Experience => MonsterInfo.Experience;

        public decimal ExtraExperienceRate = 0;

        public bool Passive, NeedHarvest, AvoidFireWall;
        public int HarvestCount;

        public int DeathCloudDurationMin = 4000, DeathCloudDurationRandom = 0;

        private uint _TargetAttackTick;

        public int MoveDelay;
        public int AttackDelay;

        public List<MonsterObject> MinionList = new List<MonsterObject>();
        public MonsterObject Master;
        public int MaxMinions = 20;

        public PlayerObject PetOwner;
        public HashSet<UserMagic> Magics = new HashSet<UserMagic>();

        
        public int PetLevel;
        public decimal PetExperience;
        public int MaxPetLevel = Config.宝宝最高等级;


        public int ViewRange
        {
            get { return PoisonList.Any(x => x.Type == PoisonType.Abyss) ? 2 : MonsterInfo.ViewRange; }
        }

        public PoisonType PoisonType;
        public int PoisonRate = 10;
        public int PoisonTicks = 5;
        public int PoisonFrequency = 2;

        public bool IgnoreShield;

        public bool EasterEventMob, HalloweenEventMob, ChristmasEventMob;

        public int MapHealthRate, MapDamageRate, MapExperienceRate, MapDropRate, MapGoldRate;

        public Element AttackElement => Stats.GetAffinityElement();

        public override bool CanMove => base.CanMove && (Poison & PoisonType.Silenced) != PoisonType.Silenced && MoveDelay > 0 && (PetOwner == null || PetOwner.PetMode == PetMode.Both || PetOwner.PetMode == PetMode.Move || PetOwner.PetMode == PetMode.PvP);
        public override bool CanAttack => base.CanAttack && (Poison & PoisonType.Silenced) != PoisonType.Silenced && AttackDelay > 0 && (PetOwner == null || PetOwner.PetMode == PetMode.Both || PetOwner.PetMode == PetMode.Attack || PetOwner.PetMode == PetMode.PvP);


        public static MonsterObject GetMonster(MonsterInfo monsterInfo)
        {
            switch (monsterInfo.AI)
            {
                case -1:
                    return new Guard { MonsterInfo = monsterInfo };
                case 1:
                    return new MonsterObject { MonsterInfo = monsterInfo, Passive = true, NeedHarvest = true, HarvestCount = 2 };
                case 2:
                    return new MonsterObject { MonsterInfo = monsterInfo, Passive = true, NeedHarvest = true, HarvestCount = 3 };
                case 3:
                    return new MonsterObject { MonsterInfo = monsterInfo, NeedHarvest = true, HarvestCount = 3 };
                case 4:
                    return new TreeMonster { MonsterInfo = monsterInfo };
                case 5:
                    return new CarnivorousPlant { MonsterInfo = monsterInfo, NeedHarvest = true, HarvestCount = 2 };
                case 6:
                    return new SpittingSpider { MonsterInfo = monsterInfo, NeedHarvest = true, HarvestCount = 2, PoisonType = PoisonType.Green };
                case 7:
                    return new SkeletonAxeThrower { MonsterInfo = monsterInfo };
                case 8:
                    return new MonsterObject { MonsterInfo = monsterInfo, NeedHarvest = true, HarvestCount = 2, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5, PoisonRate = 12 };
                case 9:
                    return new GhostSorcerer { MonsterInfo = monsterInfo };
                case 10:
                    return new GhostMage { MonsterInfo = monsterInfo };
                case 11:
                    return new VoraciousGhost { MonsterInfo = monsterInfo };
                case 12:
                    return new HealerAnt { MonsterInfo = monsterInfo };
                case 13:
                    return new LordNiJae { MonsterInfo = monsterInfo };
                case 14:
                    return new SpittingSpider { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green };
                case 15:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 16:
                    return new UmaKing { MonsterInfo = monsterInfo };
                case 17:
                    return new ArachnidGrazer
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.Larva)] = 1 }
                    };
                case 18:
                    return new Larva { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green };
                case 19:
                    return new RedMoonTheFallen { MonsterInfo = monsterInfo };
                case 20:
                    return new SkeletonAxeThrower { MonsterInfo = monsterInfo, FearRate = 2, FearDuration = 4 };
                case 21:
                    return new ZumaGuardian { MonsterInfo = monsterInfo };
                case 22:
                    return new ZumaKing
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.ZumaArcherMonster)] = 50,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.ZumaFanaticMonster)] = 25,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.ZumaGuardianMonster)] = 25,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.ZumaKeeperMonster)] = 1
                        }
                    };
                case 23:
                    return new Monkey { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green };
                case 24:
                    return new Monkey { MonsterInfo = monsterInfo, PoisonType = PoisonType.Red };
                case 25:
                    return new EvilElephant { MonsterInfo = monsterInfo };
                case 26:
                    return new NumaMage { MonsterInfo = monsterInfo };
                case 27:
                    return new GhostMage { MonsterInfo = monsterInfo };
                case 28:
                    return new WindfurySorcerer { MonsterInfo = monsterInfo };
                case 29:
                    return new SkeletonAxeThrower { MonsterInfo = monsterInfo };
                case 30:
                    return new NetherworldGate { MonsterInfo = monsterInfo };
                case 31:
                    return new SonicLizard { MonsterInfo = monsterInfo };
                case 33:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 9, IgnoreShield = true };
                case 34:
                    return new SkeletonAxeThrower { MonsterInfo = monsterInfo, AttackRange = 9 };
                case 35:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 36:
                    return new NumaMage { MonsterInfo = monsterInfo };
                case 37:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 38:
                    return new BanyaLeftGuard { MonsterInfo = monsterInfo };
                case 39:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 40:
                    return new MonsterObject { MonsterInfo = monsterInfo };
                case 41:
                    return new EmperorSaWoo { MonsterInfo = monsterInfo };
                case 42:
                    return new SpittingSpider { MonsterInfo = monsterInfo };
                case 43:
                    return new ArchLichTaedu
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BoneArcher)] = 90,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BoneSoldier)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BoneBladesman)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BoneCaptain)] = 15,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.SkeletonEnforcer)] = 1
                        }
                    };
                case 44:
                    return new WedgeMothLarva
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.LesserWedgeMoth)] = 1 }
                    };
                case 45:
                    return new RazorTusk { MonsterInfo = monsterInfo };
                case 46:
                    return new SpittingSpider { MonsterInfo = monsterInfo, PoisonType = PoisonType.Red, PoisonTicks = 1, PoisonFrequency = 10, PoisonRate = 25 };
                case 47:
                    return new SpittingSpider { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green, PoisonTicks = 7, PoisonRate = 15 };
                case 48:
                    return new SonicLizard { MonsterInfo = monsterInfo, IgnoreShield = true };
                case 49:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 8, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5 };
                case 50:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 8 };
                case 52:
                    return new WhiteBone() { MonsterInfo = monsterInfo };
                case 53:
                    return new Shinsu { MonsterInfo = monsterInfo };
                case 54:
                    return new GiantLizard { MonsterInfo = monsterInfo, RangeCooldown = TimeSpan.FromSeconds(5) };
                case 56:
                    return new CorrosivePoisonSpitter { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green, PoisonTicks = 7, PoisonRate = 15, IgnoreShield = true };
                case 57:
                    return new CorrosivePoisonSpitter { MonsterInfo = monsterInfo };
                case 58:
                    return new Stomper { MonsterInfo = monsterInfo };
                case 59:
                    return new CrimsonNecromancer() { MonsterInfo = monsterInfo };
                case 60:
                    return new ChaosKnight() { MonsterInfo = monsterInfo };
                case 61:
                    return new PachontheChaosbringer { MonsterInfo = monsterInfo };
                case 62:
                    return new NumaHighMage { MonsterInfo = monsterInfo };
                case 63:
                    return new NumaStoneThrower { MonsterInfo = monsterInfo };
                case 64:
                    return new Monkey { MonsterInfo = monsterInfo };
                case 65:
                    return new IcyGoddess { MonsterInfo = monsterInfo, FindRange = 3 };
                case 66:
                    return new IcySpiritWarrior { MonsterInfo = monsterInfo, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5, PoisonRate = 25 };
                case 67:
                    return new IcySpiritGeneral
                    {
                        MonsterInfo = monsterInfo,
                        IgnoreShield = true,
                    };
                case 68:
                    return new Warewolf
                    {
                        MonsterInfo = monsterInfo,
                        IgnoreShield = true,
                    };
                case 69:
                    return new JinamStoneGate { MonsterInfo = monsterInfo };
                case 70:
                    return new FrostLordHwa { MonsterInfo = monsterInfo };
                case 71:
                    return new BanyoWarrior { MonsterInfo = monsterInfo };
                case 72:
                    return new BanyoCaptain { MonsterInfo = monsterInfo };
                case 74:
                    return new BanyoLordGuzak
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.BanyoCaptain)] = 2,
                        }
                    };
                case 75:
                    return new DepartedMonster
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MatureEarwig)] = 1 }
                    };
                case 76:
                    return new DepartedMonster
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GoldenArmouredBeetle)] = 1 }
                    };
                case 77:
                    return new EnragedLordNiJae
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList = { [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.Millipede)] = 1 },
                        MaxMinions = 200,
                    };
                case 78:
                    return new JinchonDevil { MonsterInfo = monsterInfo };
                case 79:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 10, RangeCooldown = TimeSpan.FromSeconds(5) };
                case 80:
                    return new SunFeralWarrior
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.FerociousFlameDemon)] = 5,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.FlameDemon)] = 1,
                        }
                    };
                case 81:
                    return new MoonFeralWarrior
                    {
                        MonsterInfo = monsterInfo
                    };
                case 82:
                    return new OxFeralGeneral
                    {
                        MonsterInfo = monsterInfo,
                        IgnoreShield = true,
                    };
                case 83:
                    return new FlameDemon
                    {
                        MonsterInfo = monsterInfo,
                        Min = -2,
                        Max = 2,
                    };
                case 84:
                    return new WingedHorror
                    {
                        MonsterInfo = monsterInfo,
                        RangeChance = 1,
                    };
                case 85:
                    return new EmperorSaWoo { MonsterInfo = monsterInfo, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5, PoisonRate = 8 };
                case 86:
                    return new FlameDemon
                    {
                        MonsterInfo = monsterInfo,
                        Passive = true,
                        Min = 0,
                        Max = 8,
                    };
                case 87:
                    return new OmaWarlord
                    {
                        MonsterInfo = monsterInfo,
                        PoisonType = PoisonType.Abyss,
                        PoisonTicks = 1,
                        PoisonFrequency = 7,
                        PoisonRate = 15
                    };
                case 88:
                    return new GoruSpearman
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 89:
                    return new GoruArcher
                    {
                        MonsterInfo = monsterInfo,

                        PoisonType = PoisonType.Silenced,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 10
                    };
                case 90:
                    return new OmaWarlord
                    {
                        MonsterInfo = monsterInfo,
                        PoisonType = PoisonType.Paralysis,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 25
                    };
                case 91:
                    return new EnragedArchLichTaedu
                    {
                        MonsterInfo = monsterInfo,

                        MinSpawn = 5,
                        RandomSpawn = 5,

                        PoisonType = PoisonType.Red,
                        PoisonTicks = 1,
                        PoisonFrequency = 25,
                        PoisonRate = 5,

                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GoruArcher)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GoruGeneral)] = 5,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.GoruSpearman)] = 5,
                        }
                    };
                case 92:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 9 };
                case 93:
                    return new EscortCommander { MonsterInfo = monsterInfo };
                case 94:
                    return new FieryDancer { MonsterInfo = monsterInfo };
                case 95:
                    return new FieryDancer
                    {
                        MonsterInfo = monsterInfo,
                        PoisonType = PoisonType.Paralysis,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 15,
                    };
                case 96:
                    return new QueenOfDawn { MonsterInfo = monsterInfo };
                case 97:
                    return new SonicLizard { MonsterInfo = monsterInfo, IgnoreShield = true, Range = 5 };
                case 98:
                    return new YumgonWitch
                    {
                        MonsterInfo = monsterInfo,
                        AoEElement = Element.Lightning
                    };
                case 99:
                    return new JinhwanSpirit
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [monsterInfo] = 1,
                        }
                    };
                case 100:
                    return new YumgonWitch
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 101:
                    return new DragonQueen
                    {
                        MonsterInfo = monsterInfo,
                        DragonLordInfo = SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.DragonLord),

                        SpawnList =
                         {
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.OYoungBeast)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.YumgonWitch)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MaWarden)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MaWarlord)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.JinhwanSpirit)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.JinhwanGuardian)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.OyoungGeneral)] = 2,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.YumgonGeneral)] = 2,
                         }
                    };
                case 102:
                    return new DragonLord
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                         {
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.OYoungBeast)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.YumgonWitch)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MaWarden)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.MaWarlord)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.JinhwanSpirit)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.JinhwanGuardian)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.OyoungGeneral)] = 10000,
                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.YumgonGeneral)] = 10000,

                             [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.DragonLord)] = 1,
                         }
                    };
                case 103:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 5 };
                case 104:
                    return new FerociousIceTiger { MonsterInfo = monsterInfo };
                case 105:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 5, IgnoreShield = true, CanPvPRange = true };
                case 106:
                    return new GiantLizard { MonsterInfo = monsterInfo, AttackRange = 7, CanPvPRange = true };
                case 107:
                    return new SamaFireGuardian { MonsterInfo = monsterInfo };
                case 108:
                    return new SamaIceGuardian { MonsterInfo = monsterInfo };
                case 109:
                    return new SamaLightningGuardian { MonsterInfo = monsterInfo };
                case 110:
                    return new SamaWindGuardian { MonsterInfo = monsterInfo };

                case 111:
                    return new SamaPhoenix { MonsterInfo = monsterInfo };
                case 112:
                    return new SamaBlack { MonsterInfo = monsterInfo };
                case 113:
                    return new SamaBlue { MonsterInfo = monsterInfo };
                case 114:
                    return new SamaWhite { MonsterInfo = monsterInfo };

                case 115:
                    return new SamaProphet
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.SamaSorcerer)] = 1,
                        }
                    };
                case 116:
                    return new SamaScorcer()
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 117:
                    return new BanyoWarrior { MonsterInfo = monsterInfo, DoubleDamage = true };
                case 118:
                    return new OmaMage { MonsterInfo = monsterInfo };
                case 119:
                    return new MonsterObject
                    {
                        MonsterInfo = monsterInfo,

                        PoisonType = PoisonType.Silenced,
                        PoisonTicks = 1,
                        PoisonFrequency = 5,
                        PoisonRate = 10
                    };
                case 120:
                    return new DoomClaw()
                    {
                        MonsterInfo = monsterInfo,
                    };
                case 121:
                    return new PinkBat { MonsterInfo = monsterInfo };
                case 122:
                    return new QuartzTurtleSub
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzMiniTurtle)] = 2,
                        }
                    };
                case 123:
                    return new Larva
                    {
                        MonsterInfo = monsterInfo,
                        Range = 3,
                    };
                case 124:
                    return new QuartzTree
                    {
                        MonsterInfo = monsterInfo,
                        SubBossInfo = SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzTurtleSub),
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzBlueBat)] = 20,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzPinkBat)] = 20,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzBlueCrystal)] = 20,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.QuartzRedHood)] = 2,
                        }
                    };
                case 125:
                    return new CarnivorousPlant { MonsterInfo = monsterInfo, HideRange = 1, FindRange = 1 };
                case 126:
                    return new MonasteryBoss
                    {
                        MonsterInfo = monsterInfo,
                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.Sacrafice)] = 1,
                        }
                    };

                case 127:
                    return new JinchonDevil { MonsterInfo = monsterInfo, CastDelay = TimeSpan.FromSeconds(8), DeathCloudDurationMin = 2000, DeathCloudDurationRandom = 5000 };
                case 128:
                    return new HellBringer
                    {
                        MonsterInfo = monsterInfo,
                        BatInfo = SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.HellishBat),
                    };
                case 129:
                    return new DuelHitMonster { MonsterInfo = monsterInfo };
                case 130:
                    return new CrawlerSlave { MonsterInfo = monsterInfo };
                case 131:
                    return new CursedSlave { MonsterInfo = monsterInfo };
                case 132:
                    return new EvilCursedSlave { MonsterInfo = monsterInfo };
                case 133:
                    return new PoisonousGolem { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green, PoisonTicks = 20, PoisonRate = 5 };
                case 134:
                    return new GardenSoldier { MonsterInfo = monsterInfo };
                case 135:
                    return new GardenDefender { MonsterInfo = monsterInfo };
                case 136:
                    return new RedBlossom { MonsterInfo = monsterInfo };
                case 137:
                    return new BlueBlossom { MonsterInfo = monsterInfo };
                case 138:
                    return new FireBird { MonsterInfo = monsterInfo };
                case 139:  
                    return new WolongBianfu01 { MonsterInfo = monsterInfo, PoisonType = PoisonType.Green, PoisonTicks = 20, PoisonRate = 5 };
                case 140:  
                    return new WolongBianfu02 { MonsterInfo = monsterInfo, PoisonType = PoisonType.Red, PoisonTicks = 20, PoisonRate = 5 };
                case 141:  
                    return new WolongBianfu01 { MonsterInfo = monsterInfo, PoisonType = PoisonType.Paralysis, PoisonTicks = 20, PoisonRate = 5 };
                case 142:  
                    return new WolongLiuxingchui { MonsterInfo = monsterInfo };
                case 143:  
                    return new YuanguShiwang { MonsterInfo = monsterInfo };
                case 144:  
                    return new MabiGuaiwu { MonsterInfo = monsterInfo, PoisonType = PoisonType.Paralysis, PoisonTicks = 1, PoisonFrequency = 5, PoisonRate = 0 };
                case 145:  
                    return new MilinWuguifashi { MonsterInfo = monsterInfo, PoisonType = PoisonType.Red, PoisonTicks = 20, PoisonRate = 5 };
                case 146: 
                    return new HongyueHuoyan { MonsterInfo = monsterInfo, AttackRange = 1 };
                case 279:   
                    return new Custom { MonsterInfo = monsterInfo };
                case 280:   
                    return new YaotaStoneGate { MonsterInfo = monsterInfo };
                case 281:  
                    return new MotaStoneGate { MonsterInfo = monsterInfo };
                case 282:  
                    return new Huodong01StoneGate { MonsterInfo = monsterInfo };
                case 283:  
                    return new Huodong02StoneGate { MonsterInfo = monsterInfo };
                case 284:  
                    return new Huodong03StoneGate { MonsterInfo = monsterInfo };
                case 285:  
                    return new Huodong04StoneGate { MonsterInfo = monsterInfo };
                case 286:  
                    return new Huodong05StoneGate { MonsterInfo = monsterInfo };
                case 287:  
                    return new Huodong06StoneGate { MonsterInfo = monsterInfo };
                case 288:  
                    return new Huodong07StoneGate { MonsterInfo = monsterInfo };
                case 289:  
                    return new Huodong08StoneGate { MonsterInfo = monsterInfo };
                case 290:  
                    return new Huodong09StoneGate { MonsterInfo = monsterInfo };
                case 291:  
                    return new Huodong10StoneGate { MonsterInfo = monsterInfo };
                case 292:  
                    return new Huodong11StoneGate { MonsterInfo = monsterInfo };
                case 293:  
                    return new Huodong12StoneGate { MonsterInfo = monsterInfo };
                case 294:  
                    return new GuildBosshd01 { MonsterInfo = monsterInfo, DoubleDamage = true };
                case 295:  
                    return new GuildFbBoss { MonsterInfo = monsterInfo };
                case 296:  
                    return new Haidi01 { MonsterInfo = monsterInfo };
                case 297:  
                    return new Haidi02 { MonsterInfo = monsterInfo };
                case 298:
                    return new ZhangyuMonster
                    {
                        MonsterInfo = monsterInfo,

                        SpawnList =
                        {
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.HaidiGuicha)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.HaidiFengjing)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.HaidiMoling)] = 10,
                            [SEnvir.MonsterInfoList.Binding.First(x => x.Flag == MonsterFlag.HaidiPixia)] = 10,
                        }
                    };
                default:
                    return new MonsterObject { MonsterInfo = monsterInfo };
            }
        }
        public MonsterObject()
        {
            Stats = new Stats();
            Direction = (MirDirection)SEnvir.Random.Next(8);
        }

        protected override void OnSpawned()
        {
            base.OnSpawned();

            if (SpawnInfo != null && SpawnInfo.Info.EasterEventChance > 0 && SEnvir.Now > Config.EasterEventTime && SEnvir.Now < Config.EasterEventEnd)
                EasterEventMob = SEnvir.Random.Next(SpawnInfo.Info.EasterEventChance) == 0;

            int offset = 1000000;

            MapHealthRate = SEnvir.Random.Next(CurrentMap.Info.MonsterHealth + offset, CurrentMap.Info.MaxMonsterHealth + offset);
            MapDamageRate = SEnvir.Random.Next(CurrentMap.Info.MonsterDamage + offset, CurrentMap.Info.MaxMonsterDamage + offset);

            if (MapHealthRate >= CurrentMap.Info.ExperienceRate && MapHealthRate <= CurrentMap.Info.MaxExperienceRate)
                MapExperienceRate = MapHealthRate;
            else
                MapExperienceRate = SEnvir.Random.Next(CurrentMap.Info.ExperienceRate + offset, CurrentMap.Info.MaxExperienceRate + offset);

            MapDropRate = SEnvir.Random.Next(CurrentMap.Info.DropRate + offset, CurrentMap.Info.MaxDropRate + offset);
            MapGoldRate = SEnvir.Random.Next(CurrentMap.Info.GoldRate + offset, CurrentMap.Info.MaxGoldRate + offset);

            MapHealthRate -= offset;
            MapDamageRate -= offset;
            MapExperienceRate -= offset;
            MapDropRate -= offset;
            MapGoldRate -= offset;

            RefreshStats();
            CurrentHP = Stats[Stat.Health];
            DisplayHP = CurrentHP;

            
            RegenTime = SEnvir.Now.AddMilliseconds(SEnvir.Random.Next((int)RegenDelay.TotalMilliseconds));
            SearchTime = SEnvir.Now.AddMilliseconds(SEnvir.Random.Next((int)SearchDelay.TotalMilliseconds));
            RoamTime = SEnvir.Now.AddMilliseconds(SEnvir.Random.Next((int)RoamDelay.TotalMilliseconds));

            
            
            是否压制印回血 = true;
            压制印回血时间 = SEnvir.Now.AddMilliseconds(压制印回血延迟.TotalMilliseconds);

            ActionTime = SEnvir.Now.AddSeconds(1);

            Level = MonsterInfo.Level;

            CoolEye = SEnvir.Random.Next(100) < MonsterInfo.CoolEye;

            AddAllObjects();

            Activate();
        }
        public override void RefreshStats()
        {
            base.RefreshStats();

            Stats.Clear();
            Stats.Add(MonsterInfo.Stats);

            ApplyBonusStats();

            MoveDelay = MonsterInfo.MoveDelay;
            AttackDelay = MonsterInfo.AttackDelay;

            
            if (Config.是否开启宝宝升级功能)
            {
                if (PetOwner != null && PetLevel > 0)
                {

                    if (PetLevel == 1)
                        Stats[Stat.Health] += (int)(Stats[Stat.Health] * Config.宝宝1级加血);
                    else if (PetLevel == 2)
                        Stats[Stat.Health] += (int)(Stats[Stat.Health] * Config.宝宝2级加血);
                    else if (PetLevel == 3)
                        Stats[Stat.Health] += (int)(Stats[Stat.Health] * Config.宝宝3级加血);
                    else if (PetLevel == 4)
                        Stats[Stat.Health] += (int)(Stats[Stat.Health] * Config.宝宝4级加血);
                    else if (PetLevel == 5)
                        Stats[Stat.Health] += (int)(Stats[Stat.Health] * Config.宝宝5级加血);
                    else if (PetLevel == 6)
                        Stats[Stat.Health] += (int)(Stats[Stat.Health] * Config.宝宝6级加血);
                    else if (PetLevel == 7)
                        Stats[Stat.Health] += (int)(Stats[Stat.Health] * Config.宝宝7级加血);


                    Stats[Stat.MinAC] += PetLevel * Config.宝宝每级别增加的最低防御;
                    Stats[Stat.MaxAC] += PetLevel * Config.宝宝每级别增加的最高防御;

                    Stats[Stat.MinMR] += PetLevel * Config.宝宝每级别增加的最低魔御;
                    Stats[Stat.MaxMR] += PetLevel * Config.宝宝每级别增加的最高魔御;

                    Stats[Stat.MinDC] += PetLevel * Config.宝宝每级别增加的最低攻击;
                    Stats[Stat.MaxDC] += PetLevel * Config.宝宝每级别增加的最高攻击;

                    Stats[Stat.MinMC] += PetLevel * Config.宝宝每级别增加的最低自然;
                    Stats[Stat.MaxMC] += PetLevel * Config.宝宝每级别增加的最高自然;

                    Stats[Stat.MinSC] += PetLevel * Config.宝宝每级别增加的最低灵魂;
                    Stats[Stat.MaxSC] += PetLevel * Config.宝宝每级别增加的最高灵魂;

                    Stats[Stat.Accuracy] += Stats[Stat.Accuracy] * PetLevel / Config.宝宝每级别增加的准确;
                    Stats[Stat.Agility] += Stats[Stat.Agility] * PetLevel / Config.宝宝每级别增加的敏捷;

                    MoveDelay = (ushort)Math.Min(ushort.MaxValue, (Math.Max(ushort.MinValue, MoveDelay - PetLevel * Config.宝宝每级别增加的移动速度)));

                    if (MonsterInfo.AI == 103)
                        AttackDelay = (ushort)Math.Min(ushort.MaxValue, (Math.Max(ushort.MinValue, AttackDelay - PetLevel * Config.焰魔宝宝每级别增加的攻击速度)));
                    else
                        AttackDelay = (ushort)Math.Min(ushort.MaxValue, (Math.Max(ushort.MinValue, AttackDelay - PetLevel * Config.宝宝每级别增加的攻击速度)));
                }
            }
            else
            {
                Stats[Stat.Health] += Stats[Stat.Health] * PetLevel / 10;

                Stats[Stat.MinAC] += Stats[Stat.MinAC] * PetLevel / 10;
                Stats[Stat.MaxAC] += Stats[Stat.MaxAC] * PetLevel / 10;

                Stats[Stat.MinMR] += Stats[Stat.MinMR] * PetLevel / 10;
                Stats[Stat.MaxMR] += Stats[Stat.MaxMR] * PetLevel / 10;

                Stats[Stat.MinDC] += Stats[Stat.MinDC] * PetLevel / 10;
                Stats[Stat.MaxDC] += Stats[Stat.MaxDC] * PetLevel / 10;

                Stats[Stat.MinMC] += Stats[Stat.MinMC] * PetLevel / 10;
                Stats[Stat.MaxMC] += Stats[Stat.MaxMC] * PetLevel / 10;

                Stats[Stat.MinSC] += Stats[Stat.MinSC] * PetLevel / 10;
                Stats[Stat.MaxSC] += Stats[Stat.MaxSC] * PetLevel / 10;

                Stats[Stat.Accuracy] += Stats[Stat.Accuracy] * PetLevel / 10;
                Stats[Stat.Agility] += Stats[Stat.Agility] * PetLevel / 10;
            }

            Stats[Stat.CriticalChance] = 1;

            if (Buffs.Any(x => x.Type == BuffType.MagicWeakness))
            {
                Stats[Stat.MinMR] = 0;
                Stats[Stat.MaxMR] = 0;
            }


            foreach (BuffInfo buff in Buffs)
            {
                if (buff.Stats == null) continue;
                Stats.Add(buff.Stats);
            }

            if (PetOwner != null)
            {
                if (PetOwner.Stats[Stat.PetDCPercent] > 0)
                {
                    Stats[Stat.MinDC] += Stats[Stat.MinDC] * PetOwner.Stats[Stat.PetDCPercent] / 100;
                    Stats[Stat.MaxDC] += Stats[Stat.MaxDC] * PetOwner.Stats[Stat.PetDCPercent] / 100;

                    
                    if (!Config.是否开启宝宝升级功能)
                    {
                        foreach (UserMagic magic in Magics)
                        {
                            switch (magic.Info.Magic)
                            {
                                case MagicType.DemonicRecovery:
                                    Stats[Stat.Health] += (magic.Level + 1) * 300;
                                    break;
                            }
                        }
                    }
                }
                if (PetOwner.Stats[Stat.PetAttackSpeed] > 0)
                {
                    
                    
                    if (PetOwner.MW01 == 53 || PetOwner.MW02 == 53 || PetOwner.MW03 == 53)
                    {
                        MingwenInfo Mingweninfo = SEnvir.MingwenInfoList.Binding.FirstOrDefault((MingwenInfo x) => x.MingWenID == 53);
                        AttackDelay -= (ushort)Mingweninfo.Canshu5;
                    }
                    
                    
                    else if (PetOwner.MW01 == 54 || PetOwner.MW02 == 54 || PetOwner.MW03 == 54)
                    {
                        MingwenInfo Mingweninfo = SEnvir.MingwenInfoList.Binding.FirstOrDefault((MingwenInfo x) => x.MingWenID == 54);
                        Stats[Stat.Health] += Mingweninfo.Canshu5;
                    }
                }
            }


            /*
            Stats[Stat.FireResistance] = Math.Min(5, Stats[Stat.FireResistance]);
            Stats[Stat.IceResistance] = Math.Min(5, Stats[Stat.IceResistance]);
            Stats[Stat.LightningResistance] = Math.Min(5, Stats[Stat.LightningResistance]);
            Stats[Stat.WindResistance] = Math.Min(5, Stats[Stat.WindResistance]);
            Stats[Stat.HolyResistance] = Math.Min(5, Stats[Stat.HolyResistance]);
            Stats[Stat.DarkResistance] = Math.Min(5, Stats[Stat.DarkResistance]);
            Stats[Stat.PhantomResistance] = Math.Min(5, Stats[Stat.PhantomResistance]);
            */


            Stats[Stat.Health] += (int)(Stats[Stat.Health] * (long)Stats[Stat.HealthPercent] / 100);
            Stats[Stat.Mana] += (int)(Stats[Stat.Mana] * (long)Stats[Stat.ManaPercent] / 100);

            Stats[Stat.MinDC] += (int)(Stats[Stat.MinDC] * (long)Stats[Stat.DCPercent] / 100);
            Stats[Stat.MaxDC] += (int)(Stats[Stat.MaxDC] * (long)Stats[Stat.DCPercent] / 100);

            Stats[Stat.MinMC] += (int)(Stats[Stat.MinMC] * (long)Stats[Stat.MCPercent] / 100);
            Stats[Stat.MaxMC] += (int)(Stats[Stat.MaxMC] * Stats[Stat.MCPercent] / 100);

            Stats[Stat.MinSC] += (int)(Stats[Stat.MinSC] * (long)Stats[Stat.SCPercent] / 100);
            Stats[Stat.MaxSC] += (int)(Stats[Stat.MaxSC] * (long)Stats[Stat.SCPercent] / 100);

            if (PetOwner == null && CurrentMap != null)
            {
                Stats[Stat.Health] += (int)(Stats[Stat.Health] * (long)MapHealthRate / 100);

                Stats[Stat.MinDC] += (int)(Stats[Stat.MinDC] * (long)MapDamageRate / 100);
                Stats[Stat.MaxDC] += (int)(Stats[Stat.MaxDC] * (long)MapDamageRate / 100);
            }


            Stats[Stat.Health] = Math.Max(1, Stats[Stat.Health]);
            Stats[Stat.Mana] = Math.Max(1, Stats[Stat.Mana]);

            Stats[Stat.MinAC] = Math.Max(0, Stats[Stat.MinAC]);
            Stats[Stat.MaxAC] = Math.Max(0, Stats[Stat.MaxAC]);
            Stats[Stat.MinMR] = Math.Max(0, Stats[Stat.MinMR]);
            Stats[Stat.MaxMR] = Math.Max(0, Stats[Stat.MaxMR]);
            Stats[Stat.MinDC] = Math.Max(0, Stats[Stat.MinDC]);
            Stats[Stat.MaxDC] = Math.Max(0, Stats[Stat.MaxDC]);
            Stats[Stat.MinMC] = Math.Max(0, Stats[Stat.MinMC]);
            Stats[Stat.MaxMC] = Math.Max(0, Stats[Stat.MaxMC]);
            Stats[Stat.MinSC] = Math.Max(0, Stats[Stat.MinSC]);
            Stats[Stat.MaxSC] = Math.Max(0, Stats[Stat.MaxSC]);

            Stats[Stat.MinDC] = Math.Min(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            Stats[Stat.MinMC] = Math.Min(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            Stats[Stat.MinSC] = Math.Min(Stats[Stat.MinSC], Stats[Stat.MaxSC]);

            if (EasterEventMob)
                Stats[Stat.Health] = 1;

            if (ChristmasEventMob)
                Stats[Stat.Health] = 10;

            S.DataObjectMaxHealthMana p = new S.DataObjectMaxHealthMana { ObjectID = ObjectID, Stats = Stats };

            foreach (PlayerObject player in DataSeenByPlayers)
                player.Enqueue(p);


            if (CurrentHP > Stats[Stat.Health]) SetHP(Stats[Stat.Health]);
            if (CurrentMP > Stats[Stat.Mana]) SetMP(Stats[Stat.Mana]);
        }
        public virtual void ApplyBonusStats()
        {

        }

        public override void CleanUp()
        {
            base.CleanUp();

            _Target = null;

            SpawnList?.Clear();
            SpawnList = null;

            _EXPOwner = null;

            Drops?.Clear();


            Magics?.Clear();
        }


        public override void Activate()
        {
            if (Activated) return;

            if (NearByPlayers.Count == 0 && MonsterInfo.ViewRange <= Config.MaxViewRange && !MonsterInfo.IsBoss && PetOwner == null) return;

            Activated = true;
            SEnvir.ActiveObjects.Add(this);
        }
        public override void DeActivate()
        {
            if (!Activated) return;

            if (NearByPlayers.Count > 0 || MonsterInfo.ViewRange > Config.MaxViewRange || Target != null || MonsterInfo.IsBoss || PetOwner != null || ActionList.Count > 0 || CurrentHP < Stats[Stat.Health]) return;

            Activated = false;
            SEnvir.ActiveObjects.Remove(this);
        }



        public override void ProcessAction(DelayedAction action)
        {
            switch (action.Type)
            {
                case ActionType.DelayAttack:
                    Attack((MapObject)action.Data[0], (int)action.Data[1], (Element)action.Data[2]);
                    return;
                case ActionType.DelayMagic:
                    switch ((MagicType)action.Data[0])
                    {
                        case MagicType.FireWall:
                            FireWallEnd((Cell)action.Data[1]);
                            break;
                        case MagicType.DragonRepulse:
                            DragonRepulseEnd((MapObject)action.Data[1]);
                            break;
                        case MagicType.Purification:
                            Purify((MapObject)action.Data[1]);
                            break;
                        case MagicType.MonsterDeathCloud:
                            DeathCloudEnd((Cell)action.Data[1], (bool)action.Data[2], (Point)action.Data[3]);
                            break;
                    }
                    break;
            }

            base.ProcessAction(action);
        }
        public override void Process()
        {
            base.Process();

            if (Dead)
            {
                Target = null;

                if (SEnvir.Now > DeadTime)
                {
                    Despawn();
                    return;
                }
            }

            if (Target?.Node == null || Target.Dead || Target.CurrentMap != CurrentMap || !Functions.InRange(CurrentLocation, Target.CurrentLocation, Config.MaxViewRange) ||
               ((Poison & PoisonType.Abyss) == PoisonType.Abyss && !Functions.InRange(CurrentLocation, Target.CurrentLocation, ViewRange)) || !CanAttackTarget(Target))
                Target = null;

            if (Target != null && Target.Buffs.Any(x => x.Type == BuffType.Cloak) && !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2) && Stats[Stat.IgnoreStealth] == 0)
                Target = null;

            if (Target != null && Target.Buffs.Any(x => x.Type == BuffType.Transparency))
                Target = null;

            ProcessAI();
        }
        public override void ProcessNameColour()
        {
            NameColour = Color.White;
            Color oldColour = NameColour;

            if (SEnvir.Now < ShockTime)
                NameColour = Color.Peru;
            else if (SEnvir.Now < RageTime)
                NameColour = Color.Red;

            
            if (Config.是否开启宝宝升级功能)
            {
                if (PetOwner?.Node != null)
                {
                    switch (PetLevel)
                    {
                        case 1:
                            NameColour = Config.宝宝1级名字颜色;
                            break;
                        case 2:
                            NameColour = Config.宝宝2级名字颜色;
                            break;
                        case 3:
                            NameColour = Config.宝宝3级名字颜色;
                            break;
                        case 4:
                            NameColour = Config.宝宝4级名字颜色;
                            break;
                        case 5:
                            NameColour = Config.宝宝5级名字颜色;
                            break;
                        case 6:
                            NameColour = Config.宝宝6级名字颜色;
                            break;
                        case 7:
                            NameColour = Config.宝宝7级名字颜色;
                            break;
                    }
                }
            }
            
            if (oldColour != NameColour)
                Broadcast(new S.ObjectNameColour { ObjectID = ObjectID, Colour = NameColour });
        }

        public virtual void ProcessAI()
        {
            if (Dead) return;

            if (PetOwner?.Node != null)
            {
                if (Target != null)
                {
                    if (PetOwner.PetMode == PetMode.PvP && Target.Race != ObjectType.Player)
                        Target = null;

                    if (PetOwner.PetMode == PetMode.None || PetOwner.PetMode == PetMode.Move)
                        Target = null;
                }
                if (SEnvir.Now > TameTime)
                    UnTame();
                else if (Visible && !PetOwner.VisibleObjects.Contains(this) && (PetOwner.PetMode == PetMode.Both || PetOwner.PetMode == PetMode.Move || PetOwner.PetMode == PetMode.PvP))
                    PetRecall(); 
            }

            ProcessRegen();
            ProcessSearch();
            ProcessRoam();
            ProcessTarget();
        }
        public override void OnSafeDespawn()
        {
            base.OnSafeDespawn();

            Master?.MinionList.Remove(this);
            Master = null;

            PetOwner?.Pets.Remove(this);
            PetOwner = null;

            if (MinionList != null)
            {
                for (int i = MinionList.Count - 1; i >= 0; i--)
                    MinionList[i].Master = null;

                MinionList.Clear();
            }


            if (SpawnInfo != null)
                SpawnInfo.AliveCount--;

            ProcessEvents();

            SpawnInfo = null;

            EXPOwner = null;
        }


        public void UnTame()
        {
            PetOwner?.Pets.Remove(this);
            PetOwner = null;
            Target = null;
            SearchTime = DateTime.MinValue;
            Magics.Clear();
            PetLevel = 0;
            RefreshStats();

            SetHP(Math.Min(CurrentHP, Stats[Stat.Health] / 10));

            Broadcast(new S.ObjectPetOwnerChanged { ObjectID = ObjectID });
        }
        public void PetRecall()
        {
            Cell cell = PetOwner.CurrentMap.GetCell(Functions.Move(PetOwner.CurrentLocation, PetOwner.Direction, -1));

            if (cell == null || cell.Movements != null)
                cell = PetOwner.CurrentCell;

            Teleport(PetOwner.CurrentMap, cell.Location);
        }

        
        public virtual void ProcessRegen()
        {
            if (SEnvir.Now < RegenTime) return;

            RegenTime = SEnvir.Now + RegenDelay;

            if (CurrentHP >= Stats[Stat.Health]) return;

            int regen = (int)Math.Max(1, Stats[Stat.Health] * 0.02F); 

            ChangeHP(regen);
        }

        public virtual void ProcessSearch()
        {
            if (PetOwner != null)
            {
                ProperSearch();
            }

            else if (Target != null)
            {
                if (!Visible)
                {
                    if (SEnvir.Now < SearchTime) return;
                }
                else if (!CanMove && !CanAttack) return;
                if ((uint)Environment.TickCount - _TargetAttackTick < 4000U && CanAttackTarget(Target))
                    return;
                Target = (MapObject)null;
            }
            else
            {
                if (SEnvir.Now < SearchTime || CurrentMap.Players.Count == 0) return;

                SearchTime = SEnvir.Now + SearchDelay;


                int bestDistance = int.MaxValue;
                List<MapObject> closest = new List<MapObject>();


                
                foreach (PlayerObject player in CurrentMap.Players)
                {
                    int distance;

                    foreach (MonsterObject pet in player.Pets)
                    {
                        if (pet.CurrentMap != CurrentMap) continue;

                        distance = Functions.Distance(pet.CurrentLocation, CurrentLocation);

                        if (distance > ViewRange) continue;

                        if (distance > bestDistance || !ShouldAttackTarget(pet)) continue;

                        if (distance != bestDistance) closest.Clear();

                        closest.Add(pet);
                        bestDistance = distance;
                    }

                    distance = Functions.Distance(player.CurrentLocation, CurrentLocation);

                    if (distance > ViewRange) continue;

                    if (distance > bestDistance || !ShouldAttackTarget(player)) continue;

                    if (distance != bestDistance) closest.Clear();

                    closest.Add(player);
                    bestDistance = distance;
                }

                if (closest.Count == 0) return;

                Target = closest[SEnvir.Random.Next(closest.Count)];
            }
        }
        public void ProperSearch()
        {
            if (Target != null)
            {
                if (!CanMove && !CanAttack && Visible) return;
            }
            else
            {
                if (SEnvir.Now < SearchTime) return;

                SearchTime = SEnvir.Now + SearchDelay;

                if (CurrentMap.Players.Count == 0 && !HalloweenEventMob) return;
            }

            for (int d = 0; d <= ViewRange; d++)
            {
                List<MapObject> closest = new List<MapObject>();
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.Cells[x, y];

                        if (cell?.Objects == null) continue;

                        foreach (MapObject ob in cell.Objects)
                        {
                            if (!ShouldAttackTarget(ob)) continue;

                            closest.Add(ob);
                        }
                    }
                }
                if (closest.Count == 0) continue;

                Target = closest[SEnvir.Random.Next(closest.Count)];

                return;
            }

        }

        public virtual void ProcessRoam()
        {
            if (!CanMove) return;

            if (PetOwner != null)
            {
                if (Target == null)
                    MoveTo(Functions.Move(PetOwner.CurrentLocation, PetOwner.Direction, -1));
                return;
            }

            if (SEnvir.Now < RoamTime || SeenByPlayers.Count == 0) return;

            RoamTime = SEnvir.Now + RoamDelay;


            foreach (MapObject ob in CurrentCell.Objects)
            {
                if (ob == this || !ob.Blocking) continue;

                MirDirection direction = (MirDirection)SEnvir.Random.Next(8);
                int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                for (int d = 0; d < 8; d++)
                {
                    if (Walk(direction)) return;

                    direction = Functions.ShiftDirection(direction, rotation);
                }
                return;
            }

            if (Target != null || SEnvir.Random.Next(10) > 0) return;

            if (SEnvir.Random.Next(3) > 0)
                Walk(Direction);
            else
                Turn((MirDirection)SEnvir.Random.Next(8));
        }
        public virtual void ProcessTarget()
        {
            if (Target == null) return;

            if (!InAttackRange())
            {
                if (CurrentLocation == Target.CurrentLocation)
                {
                    MirDirection direction = (MirDirection)SEnvir.Random.Next(8);
                    int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                    for (int d = 0; d < 8; d++)
                    {
                        if (Walk(direction)) break;

                        direction = Functions.ShiftDirection(direction, rotation);
                    }
                }
                else
                    MoveTo(Target.CurrentLocation);

                return;
            }

            if (!CanAttack) return;

            Attack();
        }

        public void SpawnMinions(int fixedCount, int randomCount, MapObject target)
        {
            int count = Math.Min(MaxMinions - MinionList.Count, SEnvir.Random.Next(randomCount + 1) + fixedCount);

            for (int i = 0; i < count; i++)
            {
                MonsterInfo info = SEnvir.GetMonsterInfo(SpawnList);

                if (info == null) continue;

                MonsterObject mob = GetMonster(info);

                if (!SpawnMinion(mob)) return;

                mob.Target = target;
                mob.Master = this;
                MinionList.Add(mob);
            }
        }
        public virtual bool SpawnMinion(MonsterObject mob)
        {
            return mob.Spawn(CurrentMap.Info, CurrentMap.GetRandomLocation(CurrentLocation, 6));
        }
        public override int Pushed(MirDirection direction, int distance)
        {
            if (!MonsterInfo.CanPush) return 0;

            return base.Pushed(direction, distance);
        }

        protected virtual bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }
        public override bool CanAttackTarget(MapObject ob)
        {
            if (ob == this || ob?.Node == null || ob.Dead || !ob.Visible || ob is Guard || ob is CastleLord) return false;

            switch (ob.Race)
            {
                case ObjectType.Item:
                case ObjectType.NPC:
                case ObjectType.Spell:
                    return false;
            }

            switch (ob.Race)
            {
                case ObjectType.Player:
                    PlayerObject player = (PlayerObject)ob;

                    if (player.GameMaster) return false;

                    if (PetOwner == null) return true;
                    if (PetOwner == player) return false;

                    if (InSafeZone || player.InSafeZone) return false;

                    switch (PetOwner.PetMode)
                    {
                        case PetMode.Move:
                        case PetMode.None:
                            return false;
                    }
                    if (base.CurrentMap.Info.Fight == FightSetting.Event)
                    {
                        return SEnvir.CheckMgMap(PetOwner, player);
                    }
                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return false;
                        case AttackMode.Group:
                            if (PetOwner.InGroup(player))
                                return false;
                            break;
                        case AttackMode.Guild:
                            if (PetOwner.InGuild(player))
                                return false;
                            break;
                        case AttackMode.WarRedBrown:
                            if (player.Stats[Stat.Brown] == 0 && player.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(player))
                                return false;
                            break;
                    }

                    

                    return true;
                case ObjectType.Monster:
                    MonsterObject mob = (MonsterObject)ob;

                    if (PetOwner == null)
                    {
                        if (mob.PetOwner == null)
                            return SEnvir.Now < RageTime; 

                        return true; 
                    }

                    switch (PetOwner.PetMode)
                    {
                        case PetMode.Move:
                        case PetMode.None:
                        case PetMode.PvP:
                            return false;
                    }

                    
                    if (mob.PetOwner == null) return true;

                    
                    if (mob.InSafeZone || InSafeZone) return false;

                    if (PetOwner == mob.PetOwner) return false;

                    if (base.CurrentMap.Info.Fight == FightSetting.Event)
                    {
                        return SEnvir.CheckMgMap(PetOwner, mob.PetOwner);
                    }


                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return false;
                        case AttackMode.Group:
                            if (PetOwner.InGroup(mob.PetOwner))
                                return false;
                            break;
                        case AttackMode.Guild:
                            if (PetOwner.InGuild(mob.PetOwner))
                                return false;
                            break;
                        case AttackMode.WarRedBrown:
                            if (mob.PetOwner.Stats[Stat.Brown] == 0 && mob.PetOwner.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(mob.PetOwner))
                                return false;
                            break;
                    }

                    return true;
                default:
                    throw new NotImplementedException();
            }
        }
        public override bool CanHelpTarget(MapObject ob)
        {
            if (ob?.Node == null || ob.Dead || !ob.Visible || ob is Guard || ob is CastleLord) return false;

            if (ob == this) return true;

            switch (ob.Race)
            {
                case ObjectType.Player:
                    if (PetOwner == null) return false;

                    PlayerObject player = (PlayerObject)ob;

                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return true;

                        case AttackMode.Group:
                            if (PetOwner.InGroup(player))
                                return true;
                            break;

                        case AttackMode.Guild:
                            if (PetOwner.InGuild(player))
                                return true;
                            break;

                        case AttackMode.WarRedBrown:
                            if (player.Stats[Stat.Brown] == 0 && player.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(player))
                                return true;
                            break;
                    }

                    return true;

                case ObjectType.Monster:


                    MonsterObject mob = (MonsterObject)ob;

                    if (PetOwner == null) return mob.PetOwner == null;

                    if (mob.PetOwner == null) return false;

                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return true;

                        case AttackMode.Group:
                            if (PetOwner.InGroup(mob.PetOwner))
                                return true;
                            break;

                        case AttackMode.Guild:
                            if (PetOwner.InGuild(mob.PetOwner))
                                return true;
                            break;

                        case AttackMode.WarRedBrown:
                            if (mob.PetOwner.Stats[Stat.Brown] == 0 && mob.PetOwner.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(mob.PetOwner))
                                return true;
                            break;
                    }

                    return true;

                default:
                    return false;
            }
        }
        public virtual bool ShouldAttackTarget(MapObject ob)
        {
            if (Passive || ob == this || ob?.Node == null || ob.Dead || !ob.Visible || ob is Guard || ob is CastleLord) return false;

            switch (ob.Race)
            {
                case ObjectType.Item:
                case ObjectType.NPC:
                case ObjectType.Spell:
                    return false;
            }

            if (ob.Buffs.Any(x => x.Type == BuffType.Invisibility) && !CoolEye) return false;

            if (ob.Buffs.Any(x => x.Type == BuffType.Cloak) && Stats[Stat.IgnoreStealth] == 0)
            {
                if (!CoolEye) return false;
                if (!Functions.InRange(ob.CurrentLocation, CurrentLocation, 2)) return false;
                if (ob.Level >= Level) return false;
            }

            if (ob.Buffs.Any(x => x.Type == BuffType.Transparency) && ((Poison & PoisonType.Infection) != PoisonType.Infection || Level < 100)) return false;

            switch (ob.Race)
            {
                case ObjectType.Player:
                    PlayerObject player = (PlayerObject)ob;
                    if (player.GameMaster) return false;

                    if (PetOwner == null) return true;
                    if (PetOwner == player) return false;

                    if (InSafeZone || player.InSafeZone) return false;

                    switch (PetOwner.PetMode)
                    {
                        case PetMode.Move:
                        case PetMode.None:
                            return false;
                    }

                    if (base.CurrentMap.Info.Fight == FightSetting.Event)
                    {
                        return SEnvir.CheckMgMap(PetOwner, player);
                    }

                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return false;
                        case AttackMode.Group:
                            if (PetOwner.InGroup(player))
                                return false;
                            break;
                        case AttackMode.Guild:
                            if (PetOwner.InGuild(player))
                                return false;
                            break;
                        case AttackMode.WarRedBrown:
                            if (player.Stats[Stat.Brown] == 0 && player.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(player))
                                return false;
                            break;
                    }

                    

                    if (PetOwner.Pets.Any(x =>
                    {
                        if (x.Target == null) return false;

                        switch (x.Target.Race)
                        {
                            case ObjectType.Player:
                                return x.Target == player;
                            case ObjectType.Monster:
                                return ((MonsterObject)x.Target).PetOwner == player;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    })) return true;

                    if (player.Pets.Any(x =>
                    {
                        if (x.Target == null) return false;

                        switch (x.Target.Race)
                        {
                            case ObjectType.Player:
                                return x.Target == PetOwner;
                            case ObjectType.Monster:
                                return ((MonsterObject)x.Target).PetOwner == PetOwner;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    })) return true;

                    return false;
                case ObjectType.Monster:
                    MonsterObject mob = (MonsterObject)ob;

                    if (PetOwner == null)
                    {
                        if (mob.PetOwner == null)
                            return SEnvir.Now < RageTime; 

                        return true; 
                    }

                    switch (PetOwner.PetMode)
                    {
                        case PetMode.Move:
                        case PetMode.None:
                        case PetMode.PvP:
                            return false;
                    }

                    
                    if (mob.PetOwner == null)
                    {
                        if (mob.EXPOwner == PetOwner || PetOwner.InGroup(mob.EXPOwner) || PetOwner.InGuild(mob.EXPOwner))
                            return true;

                        

                        if (mob.EXPOwner != null) return false; 


                        if (mob.Target == null) return false;

                        PlayerObject mobTarget;

                        if (mob.Target.Race == ObjectType.Monster)
                            mobTarget = ((MonsterObject)mob.Target).PetOwner;
                        else
                            mobTarget = (PlayerObject)mob.Target;

                        if (mobTarget?.Node == null) return false;

                        if (mobTarget == PetOwner || PetOwner.InGroup(mobTarget) || PetOwner.InGuild(mobTarget))
                            return true;

                        return false;
                    }

                    
                    if (mob.InSafeZone || InSafeZone) return false;

                    if (PetOwner == mob.PetOwner) return false;

                    if (base.CurrentMap.Info.Fight == FightSetting.Event)
                    {
                        return SEnvir.CheckMgMap(PetOwner, mob.PetOwner);
                    }

                    switch (PetOwner.AttackMode)
                    {
                        case AttackMode.Peace:
                            return false;
                        case AttackMode.Group:
                            if (PetOwner.InGroup(mob.PetOwner))
                                return false;
                            break;
                        case AttackMode.Guild:
                            if (PetOwner.InGuild(mob.PetOwner))
                                return false;
                            break;
                        case AttackMode.WarRedBrown:
                            if (mob.PetOwner.Stats[Stat.Brown] == 0 && mob.PetOwner.Stats[Stat.PKPoint] < Config.RedPoint && !PetOwner.AtWar(mob.PetOwner))
                                return false;
                            break;
                    }


                    if (PetOwner.Pets.Any(x =>
                    {
                        if (x.Target == null) return false;

                        switch (x.Target.Race)
                        {
                            case ObjectType.Player:
                                return x.Target == mob.PetOwner;
                            case ObjectType.Monster:
                                return ((MonsterObject)x.Target).PetOwner == mob.PetOwner;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    })) return true;

                    if (mob.PetOwner.Pets.Any(x =>
                    {
                        if (x.Target == null) return false;

                        switch (x.Target.Race)
                        {
                            case ObjectType.Player:
                                return x.Target == PetOwner;
                            case ObjectType.Monster:
                                return ((MonsterObject)x.Target).PetOwner == PetOwner;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    })) return true;

                    return false;
                default:
                    throw new NotImplementedException();
            }
        }
        protected virtual void Attack()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            UpdateAttackTime();

            ActionList.Add(new DelayedAction(
                               SEnvir.Now.AddMilliseconds(400),
                               ActionType.DelayAttack,
                               Target,
                               GetDC(),
                               AttackElement));
        }

        public virtual int Attack(MapObject ob, int power, Element element)
        {
            if (ob?.Node == null || ob.Dead) return 0;

            int damage;

            
            if (MonsterInfo.AI == 144)
            {
                ob.Stats[Stat.Agility] = 0;
                ob.Stats[Stat.MaxAC] = 0;
                ob.Stats[Stat.MinAC] = 0;
                ob.Stats[Stat.MaxMR] = 0;
                ob.Stats[Stat.MinMR] = 0;
            }

            if (PoisonList.Any(x => x.Type == PoisonType.Abyss) && SEnvir.Random.Next(2) > 0)
            {
                ob.Dodged();
                return 0;
            }

            if (element == Element.None)
            {
                int accuracy = Stats[Stat.Accuracy];

                if (SEnvir.Random.Next(ob.Stats[Stat.Agility]) > accuracy)
                {
                    ob.Dodged();
                    return 0;
                }

                damage = power - ob.GetAC();
            }
            else
            {
                damage = power - ob.GetMR();
            }

            int res = ob.Stats.GetResistanceValue(element);

            if (res > 0)
                damage -= damage * res / 10;
            else if (res < 0)
                damage -= damage * res / 5;

            if (damage <= 0)
            {
                ob.Blocked();
                return 0;
            }


            damage = ob.Attacked(this, damage, element, true, IgnoreShield);

            if (damage <= 0) return damage;

            LifeSteal += damage * Stats[Stat.LifeSteal] / 100M;

            if (LifeSteal > 1)
            {
                int heal = (int)Math.Floor(LifeSteal);
                LifeSteal -= heal;
                ChangeHP(heal);
            }

            foreach (UserMagic magic in Magics)
                PetOwner?.LevelMagic(magic);

            if (PoisonType == PoisonType.None || SEnvir.Random.Next(PoisonRate) > 0) return damage;

            ob.ApplyPoison(new Poison
            {
                Owner = this,
                Type = PoisonType,
                Value = GetSC(),
                TickFrequency = TimeSpan.FromSeconds(PoisonFrequency),
                TickCount = PoisonTicks,
            });


            return damage;
        }

        #region Magics

        public void AttackMagic(MagicType magic, Element element, bool travel, int damage = 0)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = magic, Targets = new List<uint> { Target.ObjectID }, AttackElement = Element.None });

            UpdateAttackTime();

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500 + (travel ? Functions.Distance(CurrentLocation, Target.CurrentLocation) * 48 : 0)),
                ActionType.DelayAttack,
                Target,
                damage == 0 ? GetDC() : damage,
                element));
        }

        public void AttackAoE(int radius, MagicType magic, Element element, int damage = 0)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = magic, Locations = new List<Point> { Target.CurrentLocation }, AttackElement = Element.None });

            UpdateAttackTime();

            List<MapObject> targets = GetTargets(CurrentMap, Target.CurrentLocation, radius);

            foreach (MapObject ob in targets)
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayAttack,
                    ob,
                    damage == 0 ? GetDC() : damage,
                    element));
            }
        }
        public void SamaGuardianFire()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.SamaGuardianFire, Locations = new List<Point> { Target.CurrentLocation }, AttackElement = Element.None });

            UpdateAttackTime();

            List<MapObject> targets = GetTargets(CurrentMap, Target.CurrentLocation, 5);

            foreach (MapObject ob in targets)
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayAttack,
                    ob,
                    GetDC(),
                    Element.Fire));
            }
        }
        public void LineAoE(int distance, int min, int max, MagicType magic, Element element)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            LineAoE(distance, min, max, magic, element, Direction);
        }
        public virtual void LineAoE(int distance, int min, int max, MagicType magic, Element element, MirDirection dir)
        {
            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();


            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = dir, CurrentLocation = CurrentLocation, Cast = true, Type = magic, Targets = targetIDs, Locations = locations, AttackElement = Element.None });

            UpdateAttackTime();


            for (int d = min; d <= max; d++)
            {
                MirDirection direction = Functions.ShiftDirection(dir, d);

                if (magic == MagicType.LightningBeam || magic == MagicType.BlowEarth || magic == MagicType.ElementalHurricane)
                    locations.Add(Functions.Move(CurrentLocation, direction, distance));

                for (int i = 1; i <= distance; i++)
                {
                    Point location = Functions.Move(CurrentLocation, direction, i);
                    Cell cell = CurrentMap.GetCell(location);

                    if (cell == null) continue;

                    if (magic != MagicType.LightningBeam && magic != MagicType.BlowEarth && magic != MagicType.ElementalHurricane)
                        locations.Add(cell.Location);

                    if (cell.Objects != null)
                    {
                        foreach (MapObject ob in cell.Objects)
                        {
                            if (!CanAttackTarget(ob)) continue;

                            ActionList.Add(new DelayedAction(
                                SEnvir.Now.AddMilliseconds(500 + i * 75),
                                ActionType.DelayAttack,
                                ob,
                                GetDC(),
                                element));
                        }
                    }

                    switch (direction)
                    {
                        case MirDirection.Up:
                        case MirDirection.Right:
                        case MirDirection.Down:
                        case MirDirection.Left:
                            cell = CurrentMap.GetCell(Functions.Move(location, Functions.ShiftDirection(direction, -2)));

                            if (cell?.Objects != null)
                            {
                                foreach (MapObject ob in cell.Objects)
                                {
                                    if (!CanAttackTarget(ob)) continue;

                                    ActionList.Add(new DelayedAction(
                                        SEnvir.Now.AddMilliseconds(500 + i * 75),
                                        ActionType.DelayAttack,
                                        ob,
                                        GetDC() / 2,
                                        element));
                                }
                            }
                            cell = CurrentMap.GetCell(Functions.Move(location, Functions.ShiftDirection(direction, 2)));

                            if (cell?.Objects != null)
                            {
                                foreach (MapObject ob in cell.Objects)
                                {
                                    if (!CanAttackTarget(ob)) continue;

                                    ActionList.Add(new DelayedAction(
                                        SEnvir.Now.AddMilliseconds(500 + i * 75),
                                        ActionType.DelayAttack,
                                        ob,
                                        GetDC() / 2,
                                        element));
                                }
                            }
                            break;
                        case MirDirection.UpRight:
                        case MirDirection.DownRight:
                        case MirDirection.DownLeft:
                        case MirDirection.UpLeft:
                            cell = CurrentMap.GetCell(Functions.Move(location, Functions.ShiftDirection(direction, -1)));

                            if (cell?.Objects != null)
                            {
                                foreach (MapObject ob in cell.Objects)
                                {
                                    if (!CanAttackTarget(ob)) continue;

                                    ActionList.Add(new DelayedAction(
                                        SEnvir.Now.AddMilliseconds(500 + i * 75),
                                        ActionType.DelayAttack,
                                        ob,
                                        GetDC() / 2,
                                        element));
                                }
                            }
                            cell = CurrentMap.GetCell(Functions.Move(location, Functions.ShiftDirection(direction, 1)));

                            if (cell?.Objects != null)
                            {
                                foreach (MapObject ob in cell.Objects)
                                {
                                    if (!CanAttackTarget(ob)) continue;

                                    ActionList.Add(new DelayedAction(
                                        SEnvir.Now.AddMilliseconds(500 + i * 75),
                                        ActionType.DelayAttack,
                                        ob,
                                        GetDC() / 2,
                                        element));
                                }
                            }
                            break;
                    }
                }
            }
        }

        public void FireWall()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.FireWall, Targets = targetIDs, Locations = locations, AttackElement = Element.None });

            UpdateAttackTime();

            List<MapObject> targets = GetTargets(CurrentMap, CurrentLocation, 20);

            if (targets.Count == 0) return;

            Point location = targets[SEnvir.Random.Next(targets.Count)].CurrentLocation;

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(location)));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Up))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Down))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Left))));

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.FireWall,
                CurrentMap.GetCell(Functions.Move(location, MirDirection.Right))));
        }
        public void FireWallEnd(Cell cell)
        {
            if (cell == null) return;

            if (cell.Objects != null)
            {
                for (int i = cell.Objects.Count - 1; i >= 0; i--)
                {
                    if (cell.Objects[i].Race != ObjectType.Spell) continue;

                    SpellObject spell = (SpellObject)cell.Objects[i];

                    if (spell.Effect != SpellEffect.FireWall && spell.Effect != SpellEffect.MonsterFireWall && spell.Effect != SpellEffect.Tempest) continue;

                    spell.Despawn();
                }
            }

            SpellObject ob = new SpellObject
            {
                DisplayLocation = cell.Location,
                TickCount = 15,
                TickFrequency = TimeSpan.FromSeconds(2),
                Owner = this,
                Effect = SpellEffect.MonsterFireWall
            };

            ob.Spawn(cell.Map.Info, cell.Location);

        }

        public void DeathCloud(Point location)
        {
            bool visible = true;
            foreach (Cell cell in CurrentMap.GetCells(location, 0, 2))
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayMagic,
                    MagicType.MonsterDeathCloud,
                    cell,
                    visible,
                    location));

                visible = false;
            }
        }
        public void DeathCloudEnd(Cell cell, bool visible, Point displaylocation)
        {
            if (cell == null) return;

            SpellObject ob = new SpellObject
            {
                DisplayLocation = displaylocation,
                TickCount = 1,
                TickTime = SEnvir.Now.AddMilliseconds(DeathCloudDurationMin + SEnvir.Random.Next(DeathCloudDurationRandom)),
                Owner = this,
                Effect = SpellEffect.MonsterDeathCloud,
                Visible = visible,
            };

            ob.Spawn(cell.Map.Info, cell.Location);

        }
        public void MassLightningBall()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.LightningBall, Targets = targetIDs, Locations = locations, AttackElement = Element.None });

            UpdateAttackTime();

            for (int i = -20; i < 20; i += 5)
                locations.Add(new Point(CurrentLocation.X - 20, CurrentLocation.Y - i));

            for (int i = -20; i < 20; i += 5)
                locations.Add(new Point(CurrentLocation.X + 20, CurrentLocation.Y - i));

            for (int i = -20; i < 20; i += 5)
                locations.Add(new Point(CurrentLocation.X + i, CurrentLocation.Y - 20));

            for (int i = -20; i < 20; i += 5)
                locations.Add(new Point(CurrentLocation.X + i, CurrentLocation.Y + 20));

            List<MapObject> targets = GetTargets(CurrentMap, CurrentLocation, Config.MaxViewRange);

            foreach (MapObject ob in targets)
            {
                targetIDs.Add(ob.ObjectID);

                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500 + Functions.Distance(ob.CurrentLocation, CurrentLocation) * 48),
                    ActionType.DelayAttack,
                    ob,
                    GetDC(),
                    Element.Lightning));
            }
        }
        public void MassThunderBolt()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.ThunderBolt, Targets = targetIDs, Locations = locations, AttackElement = Element.None });

            UpdateAttackTime();


            List<Cell> cells = CurrentMap.GetCells(CurrentLocation, 0, Config.MaxViewRange);
            foreach (Cell cell in cells)
            {
                if (cell.Objects == null)
                {
                    if (SEnvir.Random.Next(50) == 0)
                        locations.Add(cell.Location);

                    continue;
                }

                foreach (MapObject ob in cell.Objects)
                {
                    if (SEnvir.Random.Next(2) > 0) continue;
                    if (!CanAttackTarget(ob)) continue;

                    targetIDs.Add(ob.ObjectID);

                    ActionList.Add(new DelayedAction(
                        SEnvir.Now.AddMilliseconds(500),
                        ActionType.DelayAttack,
                        ob,
                        GetDC(),
                        Element.Lightning));
                }
            }
        }
        /* public void ThunderBolt(int damage)
         {
             Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

             Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.ThunderBolt, Targets = new List<uint> { Target.ObjectID } });

             UpdateAttackTime();

             ActionList.Add(new DelayedAction(
                 SEnvir.Now.AddMilliseconds(500),
                 ActionType.DelayAttack,
                 Target,
                 GetDC(),
                 Element.Lightning));
         }*/
        public void MonsterThunderStorm(int damage)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.MonsterThunderStorm, Locations = new List<Point> { CurrentLocation }, AttackElement = Element.None });

            UpdateAttackTime();

            foreach (MapObject target in GetTargets(CurrentMap, CurrentLocation, 2))
            {
                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayAttack,
                    target,
                    damage,
                    Element.Lightning));
            }

        }

        public void Purification()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.Purification, Targets = new List<uint> { Target.ObjectID }, AttackElement = Element.None });

            UpdateAttackTime();

            ActionList.Add(new DelayedAction(
                SEnvir.Now.AddMilliseconds(500),
                ActionType.DelayMagic,
                MagicType.Purification,
                Target));
        }

        public void MassPurification()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targets = new List<uint>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.Purification, Targets = targets, AttackElement = Element.None });

            UpdateAttackTime();

            List<MapObject> obs = GetAllObjects(CurrentLocation, CartoonGlobals.MagicRange);

            foreach (MapObject ob in obs)
            {
                if (!CanHelpTarget(ob) && !CanAttackTarget(ob)) continue;

                targets.Add(ob.ObjectID);

                ActionList.Add(new DelayedAction(
                    SEnvir.Now.AddMilliseconds(500),
                    ActionType.DelayMagic,
                    MagicType.Purification,
                    ob));
            }

        }

        public void MassCyclone(MagicType type, int chance = 30)
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = type, Targets = targetIDs, Locations = locations, AttackElement = Element.None });

            UpdateAttackTime();

            List<Cell> cells = CurrentMap.GetCells(CurrentLocation, 0, Config.MaxViewRange);
            foreach (Cell cell in cells)
            {
                if (cell.Objects == null)
                {
                    if (SEnvir.Random.Next(chance) == 0)
                        locations.Add(cell.Location);

                    continue;
                }

                foreach (MapObject ob in cell.Objects)
                {
                    if (SEnvir.Random.Next(4) == 0) continue;
                    if (!CanAttackTarget(ob)) continue;

                    targetIDs.Add(ob.ObjectID);

                    ActionList.Add(new DelayedAction(
                        SEnvir.Now.AddMilliseconds(500),
                        ActionType.DelayAttack,
                        ob,
                        GetDC(),
                        Element.Wind));
                }
            }
        }

        public void MassCyclone()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.Cyclone, Targets = targetIDs, Locations = locations, AttackElement = Element.None });

            UpdateAttackTime();

            List<Cell> cells = CurrentMap.GetCells(CurrentLocation, 0, Config.MaxViewRange);
            foreach (Cell cell in cells)
            {
                if (cell.Objects == null)
                {
                    if (SEnvir.Random.Next(30) == 0)
                        locations.Add(cell.Location);

                    continue;
                }

                foreach (MapObject ob in cell.Objects)
                {
                    if (SEnvir.Random.Next(4) == 0) continue;
                    if (!CanAttackTarget(ob)) continue;

                    targetIDs.Add(ob.ObjectID);

                    ActionList.Add(new DelayedAction(
                        SEnvir.Now.AddMilliseconds(500),
                        ActionType.DelayAttack,
                        ob,
                        GetDC(),
                        Element.Wind));
                }
            }
        }
        /*
      public void MonsterIceStorm()
      {
          Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

          List<uint> targetIDs = new List<uint>();
          List<Point> locations = new List<Point> { Target.CurrentLocation };

          Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.MonsterIceStorm, Targets = targetIDs, Locations = locations });

          UpdateAttackTime();

          List<MapObject> targets = GetTargets(CurrentMap, Target.CurrentLocation, 1);

          foreach (MapObject target in targets)
          {
              ActionList.Add(new DelayedAction(
                  SEnvir.Now.AddMilliseconds(500),
                  ActionType.DelayAttack,
                  target,
                  GetDC(),
                  Element.Ice));
          }
      }*/


        public void PoisonousCloud()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.PoisonousCloud, Targets = targetIDs, Locations = locations, AttackElement = Element.None });

            UpdateAttackTime();

            List<Cell> cells = CurrentMap.GetCells(CurrentLocation, 0, 2);

            foreach (Cell cell in cells)
            {
                SpellObject ob = new SpellObject
                {
                    Visible = cell == CurrentCell,
                    DisplayLocation = CurrentLocation,
                    TickCount = 1,
                    TickFrequency = TimeSpan.FromSeconds(20),
                    Owner = this,
                    Effect = SpellEffect.PoisonousCloud,
                    Power = 20
                };

                ob.Spawn(CurrentMap.Info, cell.Location);
            }

        }

        public void DragonRepulse()
        {
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<uint> targetIDs = new List<uint>();
            List<Point> locations = new List<Point>();

            Broadcast(new S.ObjectMagic { ObjectID = ObjectID, Direction = Direction, CurrentLocation = CurrentLocation, Cast = true, Type = MagicType.DragonRepulse, Targets = targetIDs, Locations = locations, AttackElement = Element.None });

            UpdateAttackTime();

            BuffInfo buff = BuffAdd(BuffType.DragonRepulse, TimeSpan.FromSeconds(6), null, true, false, TimeSpan.FromSeconds(1));
            buff.TickTime = TimeSpan.FromMilliseconds(500);
        }
        public void DragonRepulseEnd(MapObject ob)
        {
            if (Attack(ob, GetDC(), AttackElement) > 0)
            {
                MirDirection dir = Functions.DirectionFromPoint(CurrentLocation, ob.CurrentLocation);
                if (ob.Pushed(dir, 1) == 0)
                {
                    int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                    for (int i = 1; i < 2; i++)
                    {
                        if (ob.Pushed(Functions.ShiftDirection(dir, i * rotation), 1) > 0) break;
                        if (ob.Pushed(Functions.ShiftDirection(dir, i * -rotation), 1) > 0) break;
                    }
                }
            }


        }
        #endregion

        public void UpdateAttackTime()
        {
            AttackTime = SEnvir.Now.AddMilliseconds(AttackDelay);
            ActionTime = SEnvir.Now.AddMilliseconds(Math.Min(MoveDelay, AttackDelay - 100));

            Poison poison = PoisonList.FirstOrDefault(x => x.Type == PoisonType.Slow);
            if (poison != null)
            {
                AttackTime += TimeSpan.FromMilliseconds(poison.Value * 100);
                ActionTime += TimeSpan.FromMilliseconds(poison.Value * 100);
            }
            if (PoisonList.Any(x => x.Type == PoisonType.Neutralize))
            {
                AttackTime += TimeSpan.FromMilliseconds(AttackDelay);
                ActionTime += TimeSpan.FromMilliseconds(Math.Min(MoveDelay, AttackDelay - 100));
            }
        }
        public void UpdateMoveTime()
        {
            MoveTime = SEnvir.Now.AddMilliseconds(MoveDelay);
            ActionTime = SEnvir.Now.AddMilliseconds(Math.Min(MoveDelay - 100, AttackDelay));

            Poison poison = PoisonList.FirstOrDefault(x => x.Type == PoisonType.Slow);
            if (poison != null)
            {
                AttackTime += TimeSpan.FromMilliseconds(poison.Value * 100);
                ActionTime += TimeSpan.FromMilliseconds(poison.Value * 100);
            }
            if (PoisonList.Any(x => x.Type == PoisonType.Neutralize))
            {
                AttackTime += TimeSpan.FromMilliseconds(MoveDelay);
                ActionTime += TimeSpan.FromMilliseconds(Math.Min(MoveDelay - 100, AttackDelay));
            }
        }

        public override int Attacked(MapObject attacker, int power, Element element, bool canReflect = true, bool ignoreShield = false, bool canCrit = true, bool canStruck = true)
        {
            if (attacker?.Node == null || power == 0 || Dead || attacker.CurrentMap != CurrentMap || !Functions.InRange(attacker.CurrentLocation, CurrentLocation, Config.MaxViewRange) || Stats[Stat.Invincibility] > 0) return 0;

            PlayerObject player;



            switch (attacker.Race)
            {
                case ObjectType.Player:
                    PlayerTagged = true;
                    player = (PlayerObject)attacker;
                    break;
                case ObjectType.Monster:
                    player = ((MonsterObject)attacker).PetOwner;
                    break;
                default:
                    throw new NotImplementedException();
            }

            ShockTime = DateTime.MinValue;

            if (EXPOwner == null && PetOwner == null)
                EXPOwner = player;

            if (EXPOwner == player && player != null)
                EXPOwnerTime = SEnvir.Now + EXPOwnerDelay;

            

            if (StruckTime != DateTime.MaxValue && SEnvir.Now > StruckTime.AddMilliseconds(300))
            {
                StruckTime = SEnvir.Now;
                Broadcast(new S.ObjectStruck { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, AttackerID = attacker.ObjectID, Element = element });
            }

            if ((Poison & PoisonType.Red) == PoisonType.Red)
                power = (int)(power * 1.2F);

            for (int i = 0; i < attacker.Stats[Stat.Rebirth]; i++)
                power = (int)(power * 1.5F);


            BuffInfo buff = Buffs.FirstOrDefault(x => x.Type == BuffType.MagicShield);

            if (buff != null)
                buff.RemainingTime -= TimeSpan.FromMilliseconds(power * 10);

            power -= power * Stats[Stat.MagicShield] / 100;

            if (SEnvir.Random.Next(100) < attacker.Stats[Stat.CriticalChance] && canCrit && power > 0)
            {
                power += power + (power * attacker.Stats[Stat.CriticalDamage] / 100);
                Critical();
            }

            buff = Buffs.FirstOrDefault(x => x.Type == BuffType.SuperiorMagicShield);

            if (buff != null)
            {
                Stats[Stat.SuperiorMagicShield] -= power;
                if (Stats[Stat.SuperiorMagicShield] <= 0)
                    BuffRemove(buff);
            }
            else
                ChangeHP(-power);
            if (Dead || !CanAttackTarget(attacker))
                return power;
            if (Target == null)
            {
                Target = attacker;
                _TargetAttackTick = (uint)Environment.TickCount;
            }
            /*
            else if (Target != null)
            {
                uint num = (uint)Environment.TickCount - _TargetAttackTick;
                if (Target == attacker)
                    _TargetAttackTick = (uint)Environment.TickCount;
                else if (num > 4500U && SEnvir.Random.Next(10000) <= 9000)
                {
                    Target = attacker;
                    _TargetAttackTick = (uint)Environment.TickCount;
                }
                else if (PetOwner == null && SEnvir.Random.Next(100) <= 40)
                {
                    Target = attacker;
                    _TargetAttackTick = (uint)Environment.TickCount;
                }
            }
            */
            return power;
        }
        public override bool ApplyPoison(Poison p)
        {
            bool res = base.ApplyPoison(p);

            if (res && CanAttackTarget(p.Owner) && Target == null)
                Target = p.Owner;

            if (p.Owner.Race == ObjectType.Player)
                PlayerTagged = true;

            return res;
        }

        public override void Die()
        {
            base.Die();


            YieldReward();

            Master?.MinionList.Remove(this);
            Master = null;

            PetOwner?.Pets.Remove(this);
            PetOwner = null;

            for (int i = MinionList.Count - 1; i >= 0; i--)
                MinionList[i].Master = null;

            MinionList.Clear();

            DeadTime = SEnvir.Now + Config.DeadDuration;

            if (Drops != null)
                DeadTime += Config.HarvestDuration;

            if (SpawnInfo != null)
                SpawnInfo.AliveCount--;

            ProcessEvents();

            SpawnInfo = null;

            EXPOwner = null;
        }

        private void ProcessEvents()
        {
            if (SpawnInfo == null) return;

            foreach (EventTarget target in MonsterInfo.Events)
            {
                if ((DropSet & target.DropSet) != target.DropSet) continue;

                int start = target.Event.CurrentValue;
                int end = Math.Min(target.Event.MaxValue, Math.Max(0, start + target.Value));

                target.Event.CurrentValue = end;

                foreach (EventAction action in target.Event.Actions)
                {
                    if (start >= action.TriggerValue || end < action.TriggerValue) continue;

                    Map map;
                    switch (action.Type)
                    {
                        case EventActionType.GlobalMessage:
                            SEnvir.Broadcast(new S.Chat { Text = action.StringParameter1, Type = MessageType.System });
                            break;
                        case EventActionType.MapMessage:
                            map = SEnvir.GetMap(action.MapParameter1);
                            if (map == null) continue;

                            map.Broadcast(new S.Chat { Text = action.StringParameter1, Type = MessageType.System });
                            break;
                        case EventActionType.PlayerMessage:
                            if (EXPOwner == null) continue;

                            EXPOwner.Broadcast(new S.Chat { Text = action.StringParameter1, Type = MessageType.System });
                            break;
                        case EventActionType.MonsterSpawn:
                            SpawnInfo spawn = SEnvir.Spawns.FirstOrDefault(x => x.Info == action.RespawnParameter1);
                            if (spawn == null) continue;

                            spawn.DoSpawn(true);
                            break;
                        case EventActionType.MonsterPlayerSpawn:

                            MonsterObject mob = GetMonster(action.MonsterParameter1);
                            mob.Spawn(CurrentMap.Info, CurrentMap.GetRandomLocation(CurrentLocation, 10));
                            break;
                        case EventActionType.MovementSettings:
                            break;
                        case EventActionType.PlayerRecall:
                            map = SEnvir.GetMap(action.MapParameter1);
                            if (map == null) continue;

                            for (int i = map.Players.Count - 1; i >= 0; i--)
                            {
                                PlayerObject player = map.Players[i];
                                player.Teleport(action.RegionParameter1);
                            }
                            break;
                        case EventActionType.PlayerEscape:
                            map = SEnvir.GetMap(action.MapParameter1);
                            if (map == null) continue;

                            for (int i = map.Players.Count - 1; i >= 0; i--)
                            {
                                PlayerObject player = map.Players[i];
                                player.Teleport(player.Character.BindPoint.BindRegion);
                            }
                            break;
                        case EventActionType.NpcSpawn:
                            if (Config.是否开启神秘商人活动)
                            {
                                NPCInfo npcplayerInfo = SEnvir.GetNpcInfo(action.SpawnNpc);
                                Map npcplayermap = CurrentMap;
                                if (npcplayermap != null && npcplayerInfo != null && action.IntParameter1 == 0 && action.IntParameter2 == 0)
                                {
                                    new NPCObject() { NPCInfo = npcplayerInfo }.Spawn(CurrentMap.Info, CurrentMap.GetRandomLocation(CurrentLocation, 2));
                                }
                                else if (npcplayermap != null && npcplayerInfo != null && action.IntParameter1 != 0 && action.IntParameter2 != 0)
                                {
                                    new NPCObject() { NPCInfo = npcplayerInfo }.Spawn(CurrentMap.Info, new Point(action.IntParameter1, action.IntParameter2));
                                }
                            }
                            break;
                        case EventActionType.NoticeMessage:
                            SEnvir.Broadcast(new S.Chat { Text = action.StringParameter1, Type = MessageType.Notice });
                            break;
                    }
                }
            }
        }

        protected void YieldReward()
        {
            if (EXPOwner == null || PetOwner != null) return;

            decimal eRate = 1M + ExtraExperienceRate;
            decimal dRate = 1M;
            int totalLevels = 0;
            List<PlayerObject> ePlayers = new List<PlayerObject>();
            List<PlayerObject> dPlayers = new List<PlayerObject>();

            if (EXPOwner.GroupMembers != null)
            {
                int eWarrior = 0, eWizard = 0, eTaoist = 0, eAssassin = 0;
                int dWarrior = 0, dWizard = 0, dTaoist = 0, dAssassin = 0;


                foreach (PlayerObject ob in EXPOwner.GroupMembers)
                {
                    if (ob.CurrentMap != CurrentMap || !Functions.InRange(ob.CurrentLocation, CurrentLocation, Config.MaxViewRange)) continue;

                    switch (ob.Class)
                    {
                        case MirClass.Warrior:
                            dWarrior++;
                            break;
                        case MirClass.Wizard:
                            dWizard++;
                            break;
                        case MirClass.Taoist:
                            dTaoist++;
                            break;
                        case MirClass.Assassin:
                            dAssassin++;
                            break;
                    }

                    dPlayers.Add(ob);

                    if (ob.Dead) continue;

                    switch (ob.Class)
                    {
                        case MirClass.Warrior:
                            eWarrior++;
                            break;
                        case MirClass.Wizard:
                            eWizard++;
                            break;
                        case MirClass.Taoist:
                            eTaoist++;
                            break;
                        case MirClass.Assassin:
                            eAssassin++;
                            break;
                    }

                    ePlayers.Add(ob);
                    totalLevels += ob.Level;
                }

                switch (Math.Min(dWarrior, Math.Min(dWizard, Math.Min(dTaoist, dAssassin))))
                {
                    case 1:
                        dRate *= 1.1M;
                        break;
                    case 2:
                        dRate *= 1.2M;
                        break;
                    case 3:
                        dRate *= 1.3M;
                        break;
                }
                switch (Math.Min(eWarrior, Math.Min(eWizard, Math.Min(eTaoist, eAssassin))))
                {
                    case 1:
                        eRate *= 1.1M;
                        break;
                    case 2:
                        eRate *= 1.25M;
                        break;
                    case 3:
                        eRate *= 1.5M;
                        break;
                }
            }

            if (PetOwner == null && CurrentMap != null)
                eRate *= 1 + MapExperienceRate / 100M;

            decimal exp = Math.Min(Experience * eRate, 500000000);

            if (ePlayers.Count == 0)
            {
                if (!EXPOwner.Dead && EXPOwner.CurrentMap == CurrentMap && Functions.InRange(EXPOwner.CurrentLocation, CurrentLocation, Config.MaxViewRange))
                {
                    if (EXPOwner.Stats[Stat.Rebirth] > 0 && ExtraExperienceRate > 0)
                        exp /= ExtraExperienceRate;

                    EXPOwner.GainExperience(exp, PlayerTagged, Level);
                    
                    EXPOwner.GainedShengwang(MonsterInfo.Shengwang, MonsterInfo.MonsterName, true);

                    OnYieldReward(EXPOwner);
                }
            }
            else
            {
                if (ePlayers.Count > 1)
                    exp += exp * 0.06M * ePlayers.Count; 



                foreach (PlayerObject player in ePlayers)
                {
                    decimal expfinal = exp * player.Level / totalLevels;

                    if (player.Stats[Stat.Rebirth] > 0 && ExtraExperienceRate > 0)
                        expfinal /= ExtraExperienceRate;

                    if (player.GroupMembers[0] == player)
                    {
                        bool flag = false;
                        int num3 = 0;
                        foreach (PlayerObject playerObject2 in ePlayers)
                        {
                            if (playerObject2 != player && player.Character.Account == playerObject2.Character.Account.Referral)
                            {
                                flag = true;
                                ++num3;
                            }
                        }
                        if (flag)
                            expfinal += expfinal * (Config.介绍人组被介绍人经验加成 + num3 * Config.介绍人组被介绍人个人经验加成) / new Decimal(100);
                    }
                    else if (player.Character.Account.Referral != null)
                    {
                        bool flag = false;
                        if (player.GroupMembers[0].Character.Account == player.Character.Account.Referral)
                        {
                            foreach (PlayerObject playerObject2 in ePlayers)
                            {
                                if (playerObject2 != player && player.Character.Account.Referral == playerObject2.Character.Account)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                                expfinal += expfinal * Config.介绍人组新人经验加成 / new Decimal(100);
                        }
                    }
                    if (player.Character.Account.Activated)
                        expfinal += expfinal * Config.激活玩家经验加成 / new Decimal(100);

                    player.GainExperience(expfinal, PlayerTagged, Level);
                    
                    player.GainedShengwang(MonsterInfo.Shengwang, MonsterInfo.MonsterName, true);

                    OnYieldReward(player);
                }
            }

            if (dPlayers.Count == 0)
            {
                if (!EXPOwner.Dead && EXPOwner.CurrentMap == CurrentMap && Functions.InRange(EXPOwner.CurrentLocation, CurrentLocation, Config.MaxViewRange))
                    Drop(EXPOwner, 1, dRate);
            }
            else
            {
                foreach (PlayerObject player in dPlayers)
                    Drop(player, dPlayers.Count, dRate);
            }

            if (MonsterInfo.IsBoss)
                foreach (SConnection con in SEnvir.Connections)
                    con.ReceiveChat($"勇士 {EXPOwner.Character.CharacterName} 杀死了 {MonsterInfo.MonsterName} Boss", MessageType.System);


            
            
            if (EXPOwner.MW01 == 136 || EXPOwner.MW02 == 136 || EXPOwner.MW03 == 136)
            {
                MingwenInfo Mingweninfo = SEnvir.MingwenInfoList.Binding.FirstOrDefault((MingwenInfo x) => x.MingWenID == 136);
                if (SEnvir.Now > 压制印回血时间) return;
                if (EXPOwner.CurrentHP >= EXPOwner.Stats[Stat.Health]) return;

                int second = (int)(压制印回血时间 - SEnvir.Now).TotalSeconds * Mingweninfo.Canshu3;

                if (second > 0)
                    EXPOwner.ChangeHP(second);
            }

            
            if (MonsterInfo.AI == 294)
            {
                if (EXPOwner.Character.Account.GuildMember == null) return;
                EXPOwner.Character.Account.GuildMember.Guild.GuildBosshd01 = 3;

                S.GuildUpdate update = EXPOwner.Character.Account.GuildMember.Guild.GetUpdatePacket();

                foreach (GuildMemberInfo member in EXPOwner.Character.Account.GuildMember.Guild.Members)
                    member.Account.Connection?.Player?.Enqueue(update);
            }
            
            if (MonsterInfo.AI == 295)
            {
                if (EXPOwner.Character.Account.GuildMember == null) return;
                EXPOwner.Character.Account.GuildMember.Guild.GuildFubenhd03 = 3;

                S.GuildUpdate update = EXPOwner.Character.Account.GuildMember.Guild.GetUpdatePacket();

                foreach (GuildMemberInfo member in EXPOwner.Character.Account.GuildMember.Guild.Members)
                    member.Account.Connection?.Player?.Enqueue(update);
            }

        }

        public virtual void OnYieldReward(PlayerObject player)
        {
        }

        public virtual void Drop(PlayerObject owner, int players, decimal rate)
        {
            rate *= 1M + owner.Stats[Stat.DropRate] / 100M;

            rate *= 1M + owner.Stats[Stat.BaseDropRate] / 100M;

            if (PetOwner == null && CurrentMap != null)
                rate *= 1 + MapDropRate / 100M;


            bool result = false;

            List<UserItem> drops = null;
            foreach (DropInfo drop in MonsterInfo.Drops)
            {
                
                
                if (!drop.Duli)
                {
                    if ((drop != null ? (drop.Item.NoMake ? 1 : 0) : 0) != 0 || drop.Chance == 0 ||
                        (DropSet & drop.DropSet) != drop.DropSet) continue;

                    if ((Biaoji & MonsterBiaoji.Koushao) == MonsterBiaoji.Koushao)
                        if (drop.Item.ItemName == "白色口哨") continue;

                    if (drop.EasterEvent && !EasterEventMob) continue;
                    
                    if (drop.圣诞节活动 && !ChristmasEventMob) continue;

                    long amount = Math.Max(1, drop.Amount / 2 + SEnvir.Random.Next(drop.Amount));

                    long chance;
                    if (drop.Item.Effect == ItemEffect.Gold)
                    {
                        if (owner.Character.Account.GoldBot && Level < owner.Level) continue;

                        chance = int.MaxValue / drop.Chance;

                        amount /= players;

                        amount += (int)(amount * owner.Stats[Stat.GoldRate] / 100M);

                        amount += (int)(amount * owner.Stats[Stat.BaseGoldRate] / 100M);

                        if (PetOwner == null && CurrentMap != null)
                            amount += (int)(amount * MapGoldRate / 100M);

                        if (amount == 0) continue;
                    }
                    else
                    {
                        decimal itemrate = rate;
                        switch (drop.Item.ItemType)
                        {
                            case ItemType.Weapon:
                                itemrate *= 1M + owner.Stats[Stat.BaseWeaponDropRate] / 100M;
                                break;
                            case ItemType.Armour:
                                itemrate *= 1M + owner.Stats[Stat.BaseArmourDropRate] / 100M;
                                break;
                            case ItemType.Helmet:
                                itemrate *= 1M + owner.Stats[Stat.BaseHelmetDropRate] / 100M;
                                break;
                            case ItemType.Necklace:
                                itemrate *= 1M + owner.Stats[Stat.BaseNecklaceDropRate] / 100M;
                                break;
                            case ItemType.Bracelet:
                                itemrate *= 1M + owner.Stats[Stat.BaseBraceletDropRate] / 100M;
                                break;
                            case ItemType.Ring:
                                itemrate *= 1M + owner.Stats[Stat.BaseRingDropRate] / 100M;
                                break;
                            case ItemType.Shoes:
                                itemrate *= 1M + owner.Stats[Stat.BaseShoesDropRate] / 100M;
                                break;
                            case ItemType.Ore:
                                itemrate *= 1M + owner.Stats[Stat.BaseOreDropRate] / 100M;
                                break;
                            case ItemType.Book:
                                itemrate *= 1M + owner.Stats[Stat.BaseBookDropRate] / 100M;
                                break;
                            case ItemType.Scroll:
                                itemrate *= 1M + owner.Stats[Stat.BaseScrollDropRate] / 100M;
                                break;
                            case ItemType.Shield:
                                itemrate *= 1M + owner.Stats[Stat.BaseShieldDropRate] / 100M;
                                break;
                            case ItemType.Baoshi:
                                itemrate *= 1M + owner.Stats[Stat.BaseBaoshiDropRate] / 100M;
                                break;
                            case ItemType.Emblem:
                                itemrate *= 1M + owner.Stats[Stat.BaseEmblemDropRate] / 100M;
                                break;
                        }
                        chance = (long)(int.MaxValue / (drop.Chance * players) * itemrate);
                    }


                    UserDrop userDrop = owner.Character.Account.UserDrops.FirstOrDefault(x => x.Item == drop.Item);

                    if (userDrop == null)
                    {
                        userDrop = SEnvir.UserDropList.CreateNewObject();
                        userDrop.Item = drop.Item;
                        userDrop.Account = owner.Character.Account;
                    }

                    decimal progress = chance / (decimal)int.MaxValue;

                    progress *= amount;

                    if (!drop.PartOnly)
                        userDrop.Progress += progress;

                    if (drop.PartOnly ||
                        ((SEnvir.Random.Next() > chance ||
                          (drop.Item.Effect != ItemEffect.Gold && owner.Character.Account.ItemBot)) &&
                         ((long)userDrop.Progress <= userDrop.DropCount || drop.Item.Effect == ItemEffect.Gold)))
                    {
                        if (drop.Item.PartCount <= 1) continue;

                        if (SEnvir.Random.Next() > ((owner.Character.Account.ItemBot || drop.PartOnly)
                                ? chance
                                : (chance * drop.Item.PartCount))) continue;


                        result = true;


                        UserItem item = SEnvir.CreateCommonDropItem(SEnvir.ItemPartInfo);

                        item.AddStat(Stat.ItemIndex, drop.Item.Index, StatSource.Added);
                        item.StatsChanged();


                        item.IsTemporary = true;

                        if (NeedHarvest)
                        {
                            if (drops == null)
                                drops = new List<UserItem>();

                            if (drop.Item.Rarity != Rarity.Common)
                            {
                                owner.Connection.ReceiveChat(
                                    string.Format(owner.Connection.Language.HarvestRare, MonsterInfo.MonsterName),
                                    MessageType.System);

                                foreach (SConnection con in owner.Connection.Observers)
                                    con.ReceiveChat(
                                        string.Format(con.Language.HarvestRare, MonsterInfo.MonsterName),
                                        MessageType.System);
                            }

                            drops.Add(item);
                            continue;
                        }


                        Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;


                        ItemObject ob = new ItemObject
                        {
                            Item = item,
                            Account = owner.Character.Account,
                            MonsterDrop = true,
                        };

                        ob.Spawn(CurrentMap.Info, cell.Location);

                        if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                        {
                            long goldAmount = 0;

                            if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                ob.Account.GuildMember.Guild.GuildTax > 0 && ob.Account.GuildMember.Guild.GuildFunds <= CartoonGlobals.MaxGold)
                                goldAmount =
                                        (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                            ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                ob.Item.ExpireTime);

                            if (owner.Companion.CanGainItems(true, check)) ob.PickUpItem(owner.Companion);
                            /*
                            if (!owner.Character.ZidongJinpiao)
                            {
                                if (owner.Gold < 500000000) return;
                                if (owner.Gold >= 500000000 && owner.Gold < 1000000000)
                                {
                                    ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                    UserItemFlags flags = UserItemFlags.Locked;
                                    ItemCheck checkem = new ItemCheck(jinpiao, 1, flags, TimeSpan.Zero);

                                    if (!owner.CanGainItems(true, checkem))
                                    {
                                        owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                        foreach (SConnection con4 in owner.Connection.Observers)
                                        {
                                            con4.ReceiveChat("背包空间不足", MessageType.System);
                                        }
                                        return;
                                    }
                                    owner.Character.Account.Gold -= 500000000;
                                    owner.GainItem(SEnvir.CreateFreshItem(checkem));
                                }
                                else if (owner.Gold >= 1000000000)
                                {
                                    ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                    UserItemFlags flags = UserItemFlags.Locked;
                                    ItemCheck checkemm = new ItemCheck(jinpiao, 2, flags, TimeSpan.Zero);

                                    if (!owner.CanGainItems(true, checkemm))
                                    {
                                        owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                        foreach (SConnection con4 in owner.Connection.Observers)
                                        {
                                            con4.ReceiveChat("背包空间不足", MessageType.System);
                                        }
                                        return;
                                    }
                                    owner.Character.Account.Gold -= 1000000000;
                                    owner.GainItem(SEnvir.CreateFreshItem(checkemm));
                                }
                            }
                            */
                        }

                        continue;
                    }


                    if (drop.Item.Effect != ItemEffect.Gold &&
                        Math.Floor(userDrop.Progress) > userDrop.DropCount + amount)
                        amount = (long)(userDrop.Progress - userDrop.DropCount);

                    userDrop.DropCount += amount;

                    result = true;
                    while (amount > 0)
                    {
                        UserItem item = null;

                        if (MonsterInfo.Rarity == MonsterRarity.Common)
                            item = SEnvir.CreateCommonDropItem(drop.Item);
                        else if (MonsterInfo.Rarity == MonsterRarity.Superior)
                            item = SEnvir.CreateSuperiorDropItem(drop.Item);
                        else if (MonsterInfo.Rarity == MonsterRarity.Elite)
                            item = SEnvir.CreateEliteDropItem(drop.Item);

                        item.Count = Math.Min(drop.Item.StackSize, amount);
                        amount -= item.Count;

                        item.IsTemporary = true; 

                        if (NeedHarvest)
                        {
                            if (drops == null)
                                drops = new List<UserItem>();

                            if (item.Info.Rarity != Rarity.Common)
                            {
                                owner.Connection.ReceiveChat(
                                    string.Format(owner.Connection.Language.HarvestRare, MonsterInfo.MonsterName),
                                    MessageType.System);

                                foreach (SConnection con in owner.Connection.Observers)
                                    con.ReceiveChat(
                                        string.Format(con.Language.HarvestRare, MonsterInfo.MonsterName),
                                        MessageType.System);
                            }

                            drops.Add(item);
                            continue;
                        }

                        Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;
                        ItemObject ob = new ItemObject
                        {
                            Item = item,
                            Account = owner.Character.Account,
                            MonsterDrop = true,
                        };


                        ob.Spawn(CurrentMap.Info, cell.Location);

                        if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                        {
                            long goldAmount = 0;

                            if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                ob.Account.GuildMember.Guild.GuildTax > 0 && ob.Account.GuildMember.Guild.GuildFunds <= CartoonGlobals.MaxGold)
                                goldAmount =
                                        (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                            ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                ob.Item.ExpireTime);

                            if (owner.Companion.CanGainItems(true, check)) ob.PickUpItem(owner.Companion);

                        }
                    }
                }
                else if (drop.Duli)
                {
                    if ((drop != null ? (drop.Item.NoMake ? 1 : 0) : 0) != 0 || drop.DuliChance == 0 ||
                        (DropSet & drop.DropSet) != drop.DropSet) continue;

                    if ((Biaoji & MonsterBiaoji.Koushao) == MonsterBiaoji.Koushao)
                        if (drop.Item.ItemName == "白色口哨") continue;

                    if (drop.EasterEvent && !EasterEventMob) continue;

                    
                    if (drop.圣诞节活动 && !ChristmasEventMob) continue;

                    long amount = Math.Max(1, drop.Amount / 2 + SEnvir.Random.Next(drop.Amount));

                    long chance;
                    if (drop.Item.Effect == ItemEffect.Gold)
                    {
                        if (owner.Character.Account.GoldBot && Level < owner.Level) continue;

                        chance = int.MaxValue / drop.DuliChance;

                        amount /= players;

                        amount += (int)(amount / 100M);

                        amount += (int)(amount / 100M);

                        if (PetOwner == null && CurrentMap != null)
                            amount += (int)(amount / 100M);

                        if (amount == 0) continue;
                    }
                    else
                    {
                        chance = (int.MaxValue / (drop.DuliChance * players));
                    }


                    UserDrop userDrop = owner.Character.Account.UserDrops.FirstOrDefault(x => x.Item == drop.Item);

                    if (userDrop == null)
                    {
                        userDrop = SEnvir.UserDropList.CreateNewObject();
                        userDrop.Item = drop.Item;
                        userDrop.Account = owner.Character.Account;
                    }

                    decimal progress = chance / (decimal)int.MaxValue;

                    progress *= amount;

                    if (!drop.PartOnly)
                        userDrop.Progress += progress;

                    if (drop.PartOnly ||
                        ((SEnvir.Random.Next() > chance ||
                          (drop.Item.Effect != ItemEffect.Gold && owner.Character.Account.ItemBot)) &&
                         ((long)userDrop.Progress <= userDrop.DropCount || drop.Item.Effect == ItemEffect.Gold)))
                    {
                        if (drop.Item.PartCount <= 1) continue;

                        if (SEnvir.Random.Next() > ((owner.Character.Account.ItemBot || drop.PartOnly)
                                ? chance
                                : (chance * drop.Item.PartCount))) continue;


                        result = true;


                        UserItem item = SEnvir.CreateCommonDropItem(SEnvir.ItemPartInfo);

                        item.AddStat(Stat.ItemIndex, drop.Item.Index, StatSource.Added);
                        item.StatsChanged();


                        item.IsTemporary = true;

                        if (NeedHarvest)
                        {
                            if (drops == null)
                                drops = new List<UserItem>();

                            if (drop.Item.Rarity != Rarity.Common)
                            {
                                owner.Connection.ReceiveChat(
                                    string.Format(owner.Connection.Language.HarvestRare, MonsterInfo.MonsterName),
                                    MessageType.System);

                                foreach (SConnection con in owner.Connection.Observers)
                                    con.ReceiveChat(
                                        string.Format(con.Language.HarvestRare, MonsterInfo.MonsterName),
                                        MessageType.System);
                            }

                            drops.Add(item);
                            continue;
                        }


                        Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;


                        ItemObject ob = new ItemObject
                        {
                            Item = item,
                            Account = owner.Character.Account,
                            MonsterDrop = true,
                        };

                        ob.Spawn(CurrentMap.Info, cell.Location);

                        if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                        {
                            long goldAmount = 0;

                            if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                ob.Account.GuildMember.Guild.GuildTax > 0 && ob.Account.GuildMember.Guild.GuildFunds <= CartoonGlobals.MaxGold)
                                goldAmount =
                                        (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                            ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                ob.Item.ExpireTime);

                            if (owner.Companion.CanGainItems(true, check)) ob.PickUpItem(owner.Companion);
                            /*
                            if (!owner.Character.ZidongJinpiao)
                            {
                                if (owner.Gold < 500000000) return;
                                if (owner.Gold >= 500000000 && owner.Gold < 1000000000)
                                {
                                    ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                    UserItemFlags flags = UserItemFlags.Locked;
                                    ItemCheck checkem = new ItemCheck(jinpiao, 1, flags, TimeSpan.Zero);

                                    if (!owner.CanGainItems(true, checkem))
                                    {
                                        owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                        foreach (SConnection con4 in owner.Connection.Observers)
                                        {
                                            con4.ReceiveChat("背包空间不足", MessageType.System);
                                        }
                                        return;
                                    }
                                    owner.Character.Account.Gold -= 500000000;
                                    owner.GainItem(SEnvir.CreateFreshItem(checkem));
                                }
                                else if (owner.Gold >= 1000000000)
                                {
                                    ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                    UserItemFlags flags = UserItemFlags.Locked;
                                    ItemCheck checkemm = new ItemCheck(jinpiao, 2, flags, TimeSpan.Zero);

                                    if (!owner.CanGainItems(true, checkemm))
                                    {
                                        owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                        foreach (SConnection con4 in owner.Connection.Observers)
                                        {
                                            con4.ReceiveChat("背包空间不足", MessageType.System);
                                        }
                                        return;
                                    }
                                    owner.Character.Account.Gold -= 1000000000;
                                    owner.GainItem(SEnvir.CreateFreshItem(checkemm));
                                }
                            }
                            */
                        }

                        continue;
                    }


                    if (drop.Item.Effect != ItemEffect.Gold &&
                        Math.Floor(userDrop.Progress) > userDrop.DropCount + amount)
                        amount = (long)(userDrop.Progress - userDrop.DropCount);

                    userDrop.DropCount += amount;

                    result = true;
                    while (amount > 0)
                    {
                        UserItem item = null;

                        if (MonsterInfo.Rarity == MonsterRarity.Common)
                            item = SEnvir.CreateCommonDropItem(drop.Item);
                        else if (MonsterInfo.Rarity == MonsterRarity.Superior)
                            item = SEnvir.CreateSuperiorDropItem(drop.Item);
                        else if (MonsterInfo.Rarity == MonsterRarity.Elite)
                            item = SEnvir.CreateEliteDropItem(drop.Item);

                        item.Count = Math.Min(drop.Item.StackSize, amount);
                        amount -= item.Count;

                        item.IsTemporary = true; 

                        if (NeedHarvest)
                        {
                            if (drops == null)
                                drops = new List<UserItem>();

                            if (item.Info.Rarity != Rarity.Common)
                            {
                                owner.Connection.ReceiveChat(
                                    string.Format(owner.Connection.Language.HarvestRare, MonsterInfo.MonsterName),
                                    MessageType.System);

                                foreach (SConnection con in owner.Connection.Observers)
                                    con.ReceiveChat(
                                        string.Format(con.Language.HarvestRare, MonsterInfo.MonsterName),
                                        MessageType.System);
                            }

                            drops.Add(item);
                            continue;
                        }

                        Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;
                        ItemObject ob = new ItemObject
                        {
                            Item = item,
                            Account = owner.Character.Account,
                            MonsterDrop = true,
                        };


                        ob.Spawn(CurrentMap.Info, cell.Location);

                        if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                        {
                            long goldAmount = 0;

                            if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                ob.Account.GuildMember.Guild.GuildTax > 0 && ob.Account.GuildMember.Guild.GuildFunds <= CartoonGlobals.MaxGold)
                                goldAmount =
                                        (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                            ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                ob.Item.ExpireTime);

                            if (owner.Companion.CanGainItems(true, check)) ob.PickUpItem(owner.Companion);
                            /*
                            if (!owner.Character.ZidongJinpiao)
                            {
                                if (owner.Gold < 500000000) return;
                                if (owner.Gold >= 500000000 && owner.Gold < 1000000000)
                                {
                                    ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                    UserItemFlags flags = UserItemFlags.Locked;
                                    ItemCheck checkem = new ItemCheck(jinpiao, 1, flags, TimeSpan.Zero);

                                    if (!owner.CanGainItems(true, checkem))
                                    {
                                        owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                        foreach (SConnection con4 in owner.Connection.Observers)
                                        {
                                            con4.ReceiveChat("背包空间不足", MessageType.System);
                                        }
                                        return;
                                    }
                                    owner.Character.Account.Gold -= 500000000;
                                    owner.GainItem(SEnvir.CreateFreshItem(checkem));
                                }
                                else if (owner.Gold >= 1000000000)
                                {
                                    ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                    UserItemFlags flags = UserItemFlags.Locked;
                                    ItemCheck checkemm = new ItemCheck(jinpiao, 2, flags, TimeSpan.Zero);

                                    if (!owner.CanGainItems(true, checkemm))
                                    {
                                        owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                        foreach (SConnection con4 in owner.Connection.Observers)
                                        {
                                            con4.ReceiveChat("背包空间不足", MessageType.System);
                                        }
                                        return;
                                    }
                                    owner.Character.Account.Gold -= 1000000000;
                                    owner.GainItem(SEnvir.CreateFreshItem(checkemm));
                                }
                            }
                            */
                        }
                    }
                }
                
            }

            foreach (UserQuest quest in owner.Character.Quests)
            {
                
                if (quest.Completed) continue;
                bool changed = false;

                foreach (QuestTask task in quest.QuestInfo.Tasks)
                {
                    bool valid = false;
                    int count = 0;
                    foreach (QuestTaskMonsterDetails details in task.MonsterDetails)
                    {
                        if (details.Monster != MonsterInfo) continue;
                        if (details.Map != null && CurrentMap.Info != details.Map) continue;

                        if (SEnvir.Random.Next(details.Chance) > 0) continue;

                        if ((DropSet & details.DropSet) != details.DropSet) continue;

                        valid = true;
                        count = details.Amount;
                        break;
                    }

                    if (!valid) continue;

                    UserQuestTask userTask = quest.Tasks.FirstOrDefault(x => x.Task == task);

                    if (userTask == null)
                    {
                        userTask = SEnvir.UserQuestTaskList.CreateNewObject();
                        userTask.Task = task;
                        userTask.Quest = quest;
                    }

                    if (userTask.Completed) continue;

                    switch (task.Task)
                    {
                        case QuestTaskType.KillMonster:
                            userTask.Amount = Math.Min(task.Amount, userTask.Amount + count);
                            changed = true;
                            break;
                        case QuestTaskType.GainItem:
                            if (task.ItemParameter == null) continue;

                            UserItem item = SEnvir.CreateCommonDropItem(task.ItemParameter);
                            item.Count = count;
                            item.UserTask = userTask;
                            item.Flags |= UserItemFlags.QuestItem;

                            item.IsTemporary = true; 

                            if (NeedHarvest)
                            {
                                if (drops == null)
                                    drops = new List<UserItem>();

                                drops.Add(item);
                                continue;
                            }


                            Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;
                            ItemObject ob = new ItemObject
                            {
                                Item = item,
                                Account = owner.Character.Account,
                                MonsterDrop = true,
                            };



                            ob.Spawn(CurrentMap.Info, cell.Location);

                            userTask.Objects.Add(ob);

                            if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                            {
                                long goldAmount = 0;


                                if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                    ob.Account.GuildMember.Guild.GuildTax > 0 && ob.Account.GuildMember.Guild.GuildFunds <= CartoonGlobals.MaxGold)
                                    goldAmount = (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                                ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                    ob.Item.ExpireTime);

                                if (owner.Companion.CanGainItems(true, check)) ob.PickUpItem(owner.Companion);
                                /*
                                if (!owner.Character.ZidongJinpiao)
                                {
                                    if (owner.Gold < 500000000) return;
                                    if (owner.Gold >= 500000000 && owner.Gold < 1000000000)
                                    {
                                        ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                        UserItemFlags flags = UserItemFlags.Locked;
                                        ItemCheck checkem = new ItemCheck(jinpiao, 1, flags, TimeSpan.Zero);

                                        if (!owner.CanGainItems(true, checkem))
                                        {
                                            owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                            foreach (SConnection con4 in owner.Connection.Observers)
                                            {
                                                con4.ReceiveChat("背包空间不足", MessageType.System);
                                            }
                                            return;
                                        }
                                        owner.Character.Account.Gold -= 500000000;
                                        owner.GainItem(SEnvir.CreateFreshItem(checkem));
                                        owner.GoldChanged();
                                    }
                                    else if (owner.Gold >= 1000000000)
                                    {
                                        ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                        UserItemFlags flags = UserItemFlags.Locked;
                                        ItemCheck checkemm = new ItemCheck(jinpiao, 2, flags, TimeSpan.Zero);

                                        if (!owner.CanGainItems(true, checkemm))
                                        {
                                            owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                            foreach (SConnection con4 in owner.Connection.Observers)
                                            {
                                                con4.ReceiveChat("背包空间不足", MessageType.System);
                                            }
                                            return;
                                        }
                                        owner.Character.Account.Gold -= 1000000000;
                                        owner.GainItem(SEnvir.CreateFreshItem(checkemm));
                                        owner.GoldChanged();
                                    }
                                }
                                */
                            }
                            break;
                    }

                }

                if (changed)
                    owner.Enqueue(new S.QuestChanged { Quest = quest.ToClientInfo() });
            }

            foreach (MeiriUserQuest quest in owner.Character.Account.MeiriQuests)
            {
                
                if (quest.Completed) continue;

                bool changed = false;

                if (!owner.MeiriQuestCanCompleted(quest.QuestInfo)) continue;

                foreach (MeiriQuestTask task in quest.QuestInfo.Tasks)
                {
                    bool valid = false;
                    int count = 0;
                    foreach (MeiriQuestTaskMonsterDetails details in task.MonsterDetails)
                    {
                        if (details.Monster != MonsterInfo) continue;
                        if (details.Map != null && CurrentMap.Info != details.Map) continue;

                        if (SEnvir.Random.Next(details.Chance) > 0) continue;

                        if ((DropSet & details.DropSet) != details.DropSet) continue;

                        valid = true;
                        count = details.Amount;
                        break;
                    }

                    if (!valid) continue;

                    MeiriUserQuestTask userTask = quest.Tasks.FirstOrDefault(x => x.Task == task);

                    if (userTask == null)
                    {
                        userTask = SEnvir.MeiriUserQuestTaskList.CreateNewObject();
                        userTask.Task = task;
                        userTask.Quest = quest;
                    }

                    if (userTask.Completed) continue;

                    switch (task.Task)
                    {
                        case MeiriQuestTaskType.KillMonster:
                            userTask.Amount = Math.Min(task.Amount, userTask.Amount + count);
                            changed = true;
                            break;
                        case MeiriQuestTaskType.GainItem:
                            if (task.ItemParameter == null) continue;

                            UserItem item = SEnvir.CreateCommonDropItem(task.ItemParameter);
                            item.Count = count;
                            item.MeiriUserTask = userTask;
                            item.Flags |= UserItemFlags.QuestItem;

                            item.IsTemporary = true; 

                            if (NeedHarvest)
                            {
                                if (drops == null)
                                    drops = new List<UserItem>();

                                drops.Add(item);
                                continue;
                            }


                            Cell cell = GetDropLocation(Config.DropDistance, owner) ?? CurrentCell;
                            ItemObject ob = new ItemObject
                            {
                                Item = item,
                                Account = owner.Character.Account,
                                MonsterDrop = true,
                            };



                            ob.Spawn(CurrentMap.Info, cell.Location);

                            userTask.Objects.Add(ob);

                            if (owner.Stats[Stat.CompanionCollection] > 0 && owner.Companion != null)
                            {
                                long goldAmount = 0;

                                if (ob.Item.Info.Effect == ItemEffect.Gold && ob.Account.GuildMember != null &&
                                    ob.Account.GuildMember.Guild.GuildTax > 0 && ob.Account.GuildMember.Guild.GuildFunds <= CartoonGlobals.MaxGold)
                                    goldAmount = (long)Math.Ceiling(ob.Item.Count * ob.Account.GuildMember.Guild.GuildTax);

                                ItemCheck check = new ItemCheck(ob.Item, ob.Item.Count - goldAmount, ob.Item.Flags,
                                    ob.Item.ExpireTime);

                                if (owner.Companion.CanGainItems(true, check)) ob.PickUpItem(owner.Companion);
                                /*
                                if (!owner.Character.ZidongJinpiao)
                                {
                                    if (owner.Gold < 500000000) return;
                                    if (owner.Gold >= 500000000 && owner.Gold < 1000000000)
                                    {
                                        ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                        UserItemFlags flags = UserItemFlags.Locked;
                                        ItemCheck checkem = new ItemCheck(jinpiao, 1, flags, TimeSpan.Zero);

                                        if (!owner.CanGainItems(true, checkem))
                                        {
                                            owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                            foreach (SConnection con4 in owner.Connection.Observers)
                                            {
                                                con4.ReceiveChat("背包空间不足", MessageType.System);
                                            }
                                            return;
                                        }
                                        owner.Character.Account.Gold -= 500000000;
                                        owner.GainItem(SEnvir.CreateFreshItem(checkem));
                                        owner.GoldChanged();
                                    }
                                    else if (owner.Gold >= 1000000000)
                                    {
                                        ItemInfo jinpiao = SEnvir.GetItemInfo("金票");
                                        UserItemFlags flags = UserItemFlags.Locked;
                                        ItemCheck checkemm = new ItemCheck(jinpiao, 2, flags, TimeSpan.Zero);

                                        if (!owner.CanGainItems(true, checkemm))
                                        {
                                            owner.Connection.ReceiveChat("背包空间不足", MessageType.System);
                                            foreach (SConnection con4 in owner.Connection.Observers)
                                            {
                                                con4.ReceiveChat("背包空间不足", MessageType.System);
                                            }
                                            return;
                                        }
                                        owner.Character.Account.Gold -= 1000000000;
                                        owner.GainItem(SEnvir.CreateFreshItem(checkemm));
                                        owner.GoldChanged();
                                    }
                                }
                                */
                            }
                            break;
                    }

                }

                if (changed)
                    owner.Enqueue(new S.MeiriQuestChanged { Quest = quest.ToClientInfo() });
            }

            if (result && owner.Companion != null)
                owner.Companion.SearchTime = DateTime.MinValue;

            if (!NeedHarvest) return;

            if (Drops == null)
                Drops = new Dictionary<AccountInfo, List<UserItem>>();

            Drops[owner.Character.Account] = drops;
        }

        public Dictionary<int, bool> CompanionMemory = new Dictionary<int, bool>();

        public void FilterItem(string str)
        {
            if (str.Length <= 0)
                return;

            string[] filterItem = str.Split(';');

            foreach (string filter in filterItem)
            {
                string[] item = filter.Split(',');
                int key = 0, val = 0;
                if (int.TryParse(item[0], out key) && int.TryParse(item[1], out val))
                {
                    if (val != 0)
                    {
                        if (!CompanionMemory.ContainsKey(key))
                        {
                            CompanionMemory.Add(key, true);
                        }
                    }
                    else
                    {
                        CompanionMemory.Remove(key);
                    }

                }
            }
        }

        public void PickUp(int x, int y, int itemIdx)
        {
            if (Dead)
                return;

            if (x < 0)
                return;
            if (x >= CurrentMap.Width)
                return;

            int distance = Functions.Distance(new Point(x, y), CurrentLocation);

            
            if (distance > Stats[Stat.PickUpRadius])
                return;

            Cell cell = CurrentMap.Cells[x, y]; 

            if (cell?.Objects == null)
                return;

            foreach (MapObject cellObject in cell.Objects)
            {
                if (cellObject.Race != ObjectType.Item)
                    continue;

                ItemObject item = (ItemObject)cellObject;

                if (itemIdx != -1)
                {
                    if (item.Item.Info.Index == itemIdx)
                    {
                        item.PickUpItem(this);
                        return;
                    }
                }
                else
                {
                    item.PickUpItem(this);
                    return;
                }
            }
        }
        
        public void PetExp(decimal amount)
        {
            if (PetLevel >= Config.宝宝最高等级) return;

            
            MonsterObject kulou = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.Skeleton && !x.Dead);
            MonsterObject chaoqiangkulou = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.JinSkeleton && !x.Dead);
            MonsterObject shenshou = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.Shinsu && !x.Dead);
            MonsterObject yanmo = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.InfernalSoldier && !x.Dead);

            MonsterObject JunwangYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.JunwangYi && !x.Dead);
            MonsterObject JunwangEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.JunwangEr && !x.Dead);
            MonsterObject JunwangSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.JunwangSan && !x.Dead);

            MonsterObject ToutianYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ToutianYi && !x.Dead);
            MonsterObject ToutianEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ToutianEr && !x.Dead);
            MonsterObject ToutianSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ToutianSan && !x.Dead);

            MonsterObject JialanYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.JialanYi && !x.Dead);
            MonsterObject JialanEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.JialanEr && !x.Dead);
            MonsterObject JialanSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.JialanSan && !x.Dead);

            MonsterObject LingShouYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.LingShouYi && !x.Dead);
            MonsterObject LingShouEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.LingShouEr && !x.Dead);
            MonsterObject LingShouSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.LingShouSan && !x.Dead);

            MonsterObject XueShouYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.XueShouYi && !x.Dead);
            MonsterObject XueShouEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.XueShouEr && !x.Dead);
            MonsterObject XueShouSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.XueShouSan && !x.Dead);

            MonsterObject ShengShouYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ShengShouYi && !x.Dead);
            MonsterObject ShengShouEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ShengShouEr && !x.Dead);
            MonsterObject ShengShouSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ShengShouSan && !x.Dead);

            MonsterObject ChaoJunwangYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoJunwangYi && !x.Dead);
            MonsterObject ChaoJunwangEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoJunwangEr && !x.Dead);
            MonsterObject ChaoJunwangSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoJunwangSan && !x.Dead);

            MonsterObject ChaoToutianYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoToutianYi && !x.Dead);
            MonsterObject ChaoToutianEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoToutianEr && !x.Dead);
            MonsterObject ChaoToutianSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoToutianSan && !x.Dead);

            MonsterObject ChaoJialanYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoJialanYi && !x.Dead);
            MonsterObject ChaoJialanEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoJialanEr && !x.Dead);
            MonsterObject ChaoJialanSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ChaoJialanSan && !x.Dead);

            MonsterObject HongyueYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.HongyueYi && !x.Dead);
            MonsterObject HongyueEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.HongyueEr && !x.Dead);
            MonsterObject HongyueSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.HongyueSan && !x.Dead);

            MonsterObject HuoyanYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.HuoyanYi && !x.Dead);
            MonsterObject HuoyanEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.HuoyanEr && !x.Dead);
            MonsterObject HuoyanSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.HuoyanSan && !x.Dead);

            MonsterObject ZhenqiangYi = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ZhenqiangYi && !x.Dead);
            MonsterObject ZhenqiangEr = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ZhenqiangEr && !x.Dead);
            MonsterObject ZhenqiangSan = PetOwner.Pets.FirstOrDefault(x => x.MonsterInfo.Flag == MonsterFlag.ZhenqiangSan && !x.Dead);

            if (Config.按照杀死的怪物经验来升级)
            {
                if (kulou != null || chaoqiangkulou != null || shenshou != null || yanmo != null ||
                    JunwangYi != null || JunwangEr != null || JunwangSan != null ||
                    ToutianYi != null || ToutianEr != null || ToutianSan != null ||
                    JialanYi != null || JialanEr != null || JialanSan != null ||
                    LingShouYi != null || LingShouEr != null || LingShouSan != null ||
                    XueShouYi != null || XueShouEr != null || XueShouSan != null ||
                    ShengShouYi != null || ShengShouEr != null || ShengShouSan != null ||
                    ChaoJunwangYi != null || ChaoJunwangEr != null || ChaoJunwangSan != null ||
                    ChaoToutianYi != null || ChaoToutianEr != null || ChaoToutianSan != null ||
                    ChaoJialanYi != null || ChaoJialanEr != null || ChaoJialanSan != null ||
                    HongyueYi != null || HongyueEr != null || HongyueSan != null ||
                    HuoyanYi != null || HuoyanEr != null || HuoyanSan != null ||
                    ZhenqiangYi != null || ZhenqiangEr != null || ZhenqiangSan != null)
                    amount *= 3;
            }

            PetExperience += amount;

            if (Config.按照杀死的怪物经验来升级)
            {
                if (PetExperience < (PetLevel + 1) * Config.宝宝每级别升级经验) return;

                PetExperience = (PetExperience - ((PetLevel + 1) * Config.宝宝每级别升级经验));
            }
            else if (Config.按照杀死的怪物数量来升级)
            {
                if (PetExperience < Config.宝宝每级别升级数量) return;

                PetExperience = PetExperience - Config.宝宝每级别升级数量;
            }
            PetLevel++;
            RefreshStats();
            
            SetHP(Stats[Stat.Health]);
        }

        public virtual void Turn(MirDirection direction)
        {
            if (!CanMove) return;

            UpdateMoveTime();

            Direction = direction;

            Broadcast(new S.ObjectTurn { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
        }
        public virtual bool Walk(MirDirection direction)
        {
            if (!CanMove) return false;

            Cell cell = CurrentMap.GetCell(Functions.Move(CurrentLocation, direction));
            if (cell == null) return false;

            if (cell.IsBlocking(this, false)) return false;

            if (AvoidFireWall && cell.Objects != null)
            {
                foreach (MapObject ob in cell.Objects)
                {
                    if (ob.Race != ObjectType.Spell) continue;
                    SpellObject spell = (SpellObject)ob;

                    switch (spell.Effect)
                    {
                        case SpellEffect.FireWall:
                        case SpellEffect.MonsterFireWall:
                        case SpellEffect.Tempest:
                            break;
                        default:
                            continue;
                    }

                    if (spell.Owner == null || !spell.Owner.CanAttackTarget(this)) continue;

                    return false;
                }
            }

            BuffRemove(BuffType.Invisibility);
            BuffRemove(BuffType.Transparency);

            Direction = direction;

            UpdateMoveTime();



            PreventSpellCheck = true;
            CurrentCell = cell; 
            PreventSpellCheck = false;

            RemoveAllObjects();
            AddAllObjects();

            Broadcast(new S.ObjectMove { ObjectID = ObjectID, Direction = direction, Location = CurrentLocation, Distance = 1 });
            CheckSpellObjects();
            return true;
        }
        protected virtual void MoveTo(Point target)
        {
            if (CurrentLocation == target) return;

            if (Functions.InRange(target, CurrentLocation, 1))
            {
                Cell cell = CurrentMap.GetCell(target);

                if (cell == null || cell.IsBlocking(this, false)) return;
            }

            MirDirection direction = Functions.DirectionFromPoint(CurrentLocation, target);

            int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

            for (int d = 0; d < 8; d++)
            {
                if (Walk(direction)) return;

                direction = Functions.ShiftDirection(direction, rotation);
            }
        }

        public override BuffInfo BuffAdd(BuffType type, TimeSpan remainingTicks, Stats stats, bool visible, bool pause, TimeSpan tickRate)
        {
            BuffInfo info = base.BuffAdd(type, remainingTicks, stats, visible, pause, tickRate);

            info.IsTemporary = true;

            return info;
        }

        protected override void OnLocationChanged()
        {
            base.OnLocationChanged();

            if (CurrentCell == null) return;

            InSafeZone = CurrentCell.SafeZone != null;
        }

        public void HarvestChanged()
        {
            Skeleton = true;

            if (Drops == null)
                DeadTime -= Config.HarvestDuration;

            foreach (PlayerObject player in SeenByPlayers)
                if (Drops == null || !Drops.ContainsKey(player.Character.Account))
                    player.Enqueue(new S.ObjectHarvested { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
        }
        public override Packet GetInfoPacket(PlayerObject ob)
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                MonsterIndex = MonsterInfo.Index,

                Location = CurrentLocation,

                NameColour = NameColour,
                Direction = Direction,
                Dead = Dead,

                PetOwner = PetOwner?.Name,

                Skeleton = NeedHarvest && Skeleton && (Drops == null || !Drops.ContainsKey(ob.Character.Account)),

                Poison = Poison,

                EasterEvent = EasterEventMob,
                HalloweenEvent = HalloweenEventMob,
                ChristmasEvent = ChristmasEventMob,

                Buffs = Buffs.Where(x => x.Visible).Select(x => x.Type).ToList()
            };
        }
        public override Packet GetDataPacket(PlayerObject ob)
        {
            return new S.DataObjectMonster
            {
                ObjectID = ObjectID,

                MonsterIndex = MonsterInfo.Index,

                MapIndex = CurrentMap.Info.Index,
                CurrentLocation = CurrentLocation,

                Health = DisplayHP,
                Stats = Stats,
                Dead = Dead,

                PetOwner = PetOwner?.Name,
            };
        }
    }
}