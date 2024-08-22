﻿using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using CartoonMirDB;
using Server.Envir;

namespace Server.DBModels
{
    [UserObject]
    public sealed class AccountInfo : DBObject
    {
        public string EMailAddress
        {
            get { return _EMailAddress; }
            set
            {
                if (_EMailAddress == value) return;

                var oldValue = _EMailAddress;
                _EMailAddress = value;

                OnChanged(oldValue, value, "EMailAddress");
            }
        }
        private string _EMailAddress;
        
        public byte[] Password
        {
            get { return _Password; }
            set
            {
                if (_Password == value) return;

                var oldValue = _Password;
                _Password = value;

                OnChanged(oldValue, value, "Password");
            }
        }
        private byte[] _Password;

        public string Passwords
        {
            get { return _Passwords; }
            set
            {
                if (_Passwords == value) return;

                var oldValue = _Passwords;
                _Passwords = value;

                OnChanged(oldValue, value, "Passwords");
            }
        }
        private string _Passwords;

        public string RealName
        {
            get { return _RealName; }
            set
            {
                if (_RealName == value) return;

                var oldValue = _RealName;
                _RealName = value;

                OnChanged(oldValue, value, "RealName");
            }
        }
        private string _RealName;

        public DateTime BirthDate
        {
            get { return _BirthDate; }
            set
            {
                if (_BirthDate == value) return;

                var oldValue = _BirthDate;
                _BirthDate = value;

                OnChanged(oldValue, value, "BirthDate");
            }
        }
        private DateTime _BirthDate;

        public string Question
        {
            get { return _Question; }
            set
            {
                if (_Question == value) return;

                var oldValue = _Question;
                _Question = value;

                OnChanged(oldValue, value, "Question");
            }
        }
        private string _Question;

        public string Answer
        {
            get { return _Answer; }
            set
            {
                if (_Answer == value) return;

                var oldValue = _Answer;
                _Answer = value;

                OnChanged(oldValue, value, "Answer");
            }
        }
        private string _Answer;

        [Association("Referrals")]
        public AccountInfo Referral
        {
            get { return _Referral; }
            set
            {
                if (_Referral == value) return;

                var oldValue = _Referral;
                _Referral = value;

                OnChanged(oldValue, value, "Referral");
            }
        }
        private AccountInfo _Referral;
        
        public string CreationIP
        {
            get { return _CreationIP; }
            set
            {
                if (_CreationIP == value) return;

                var oldValue = _CreationIP;
                _CreationIP = value;

                OnChanged(oldValue, value, "CreationIP");
            }
        }
        private string _CreationIP;
        
        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set
            {
                if (_CreationDate == value) return;

                var oldValue = _CreationDate;
                _CreationDate = value;

                OnChanged(oldValue, value, "CreationDate");
            }
        }
        private DateTime _CreationDate;

        public string LastIP
        {
            get { return _LastIP; }
            set
            {
                if (_LastIP == value) return;

                var oldValue = _LastIP;
                _LastIP = value;

                OnChanged(oldValue, value, "LastIP");
            }
        }
        private string _LastIP;

        public DateTime LastLogin
        {
            get { return _LastLogin; }
            set
            {
                if (_LastLogin == value) return;

                var oldValue = _LastLogin;
                _LastLogin = value;

                OnChanged(oldValue, value, "LastLogin");
            }
        }
        private DateTime _LastLogin;

        public string ActivationKey
        {
            get { return _ActivationKey; }
            set
            {
                if (_ActivationKey == value) return;

                var oldValue = _ActivationKey;
                _ActivationKey = value;

                OnChanged(oldValue, value, "ActivationKey");
            }
        }
        private string _ActivationKey;

        public DateTime ActivationTime
        {
            get { return _ActivationTime; }
            set
            {
                if (_ActivationTime == value) return;

                var oldValue = _ActivationTime;
                _ActivationTime = value;

                OnChanged(oldValue, value, "ActivationTime");
            }
        }
        private DateTime _ActivationTime;
        
        public bool Activated
        {
            get { return _Activated; }
            set
            {
                if (_Activated == value) return;

                var oldValue = _Activated;
                _Activated = value;

                OnChanged(oldValue, value, "Activated");
            }
        }
        private bool _Activated;

        public string ResetKey
        {
            get { return _ResetKey; }
            set
            {
                if (_ResetKey == value) return;

                var oldValue = _ResetKey;
                _ResetKey = value;

                OnChanged(oldValue, value, "ResetKey");
            }
        }
        private string _ResetKey;

        public DateTime ResetTime
        {
            get { return _ResetTime; }
            set
            {
                if (_ResetTime == value) return;

                var oldValue = _ResetTime;
                _ResetTime = value;

                OnChanged(oldValue, value, "ResetTime");
            }
        }
        private DateTime _ResetTime;

        public DateTime PasswordTime
        {
            get { return _PasswordTime; }
            set
            {
                if (_PasswordTime == value) return;

                var oldValue = _PasswordTime;
                _PasswordTime = value;

                OnChanged(oldValue, value, "PasswordTime");
            }
        }
        private DateTime _PasswordTime;

        public DateTime ChatBanExpiry
        {
            get { return _ChatBanExpiry; }
            set
            {
                if (_ChatBanExpiry == value) return;

                var oldValue = _ChatBanExpiry;
                _ChatBanExpiry = value;

                OnChanged(oldValue, value, "ChatBanExpiry");
            }
        }
        private DateTime _ChatBanExpiry;

        public bool Banned
        {
            get { return _Banned; }
            set
            {
                if (_Banned == value) return;

                var oldValue = _Banned;
                _Banned = value;

                OnChanged(oldValue, value, "Banned");
            }
        }
        private bool _Banned;

        public DateTime ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate == value) return;

                var oldValue = _ExpiryDate;
                _ExpiryDate = value;

                OnChanged(oldValue, value, "ExpiryDate");
            }
        }
        private DateTime _ExpiryDate;

        public string BanReason
        {
            get { return _BanReason; }
            set
            {
                if (_BanReason == value) return;

                var oldValue = _BanReason;
                _BanReason = value;

                OnChanged(oldValue, value, "BanReason");
            }
        }
        private string _BanReason;

        public long Gold
        {
            get { return _Gold; }
            set
            {
                if (_Gold == value) return;

                var oldValue = _Gold;
                _Gold = value;

                OnChanged(oldValue, value, "Gold");
            }
        }
        private long _Gold;

        public DateTime FlashTime
        {
            get
            {
                return _FlashTime;
            }
            set
            {
                if (_FlashTime == value)
                    return;
                DateTime flashTime = _FlashTime;
                _FlashTime = value;
                OnChanged((object)flashTime, (object)value, nameof(FlashTime));
            }
        }
        private DateTime _FlashTime;

        public long AutoTime
        {
            get
            {
                return _AutoTime;
            }
            set
            {
                if (_AutoTime == value)
                    return;
                long autoTime = _AutoTime;
                _AutoTime = value;
                OnChanged((object)autoTime, (object)value, nameof(AutoTime));
            }
        }
        private long _AutoTime;

        public int Huiyuan
        {
            get { return _Huiyuan; }
            set
            {
                if (_Huiyuan == value) return;

                var oldValue = _Huiyuan;
                _Huiyuan = value;

                OnChanged(oldValue, value, "Huiyuan");
            }
        }
        private int _Huiyuan;
       
        
        
        public int Zanzhujilu
        {
            get { return _Zanzhujilu; }
            set
            {
                if (_Zanzhujilu == value) return;

                var oldValue = _Zanzhujilu;
                _Zanzhujilu = value;

                OnChanged(oldValue, value, "Zanzhujilu");
            }
        }
        private int _Zanzhujilu;

        
        public decimal GameGoldPrice
        {
            get { return _GameGoldPrice; }
            set
            {
                if (_GameGoldPrice == value) return;

                var oldValue = _GameGoldPrice;
                _GameGoldPrice = value;

                OnChanged(oldValue, value, "GameGoldPrice");
            }
        }
        private decimal _GameGoldPrice;
        
        public int GameGold
        {
            get { return _GameGold; }
            set
            {
                if (_GameGold == value) return;

                var oldValue = _GameGold;
                _GameGold = value;

                OnChanged(oldValue, value, "GameGold");
            }
        }
        private int _GameGold;
        
        public bool AllowGroup
        {
            get { return _AllowGroup; }
            set
            {
                if (_AllowGroup == value) return;

                var oldValue = _AllowGroup;
                _AllowGroup = value;

                OnChanged(oldValue, value, "AllowGroup");
            }
        }
        private bool _AllowGroup;

        public bool AllowTrade
        {
            get { return _AllowTrade; }
            set
            {
                if (_AllowTrade == value) return;

                var oldValue = _AllowTrade;
                _AllowTrade = value;

                OnChanged(oldValue, value, "AllowTrade");
            }
        }
        private bool _AllowTrade;

        public bool AllowGuild
        {
            get { return _AllowGuild; }
            set
            {
                if (_AllowGuild == value) return;

                var oldValue = _AllowGuild;
                _AllowGuild = value;

                OnChanged(oldValue, value, "AllowGuild");
            }
        }
        private bool _AllowGuild;

        public bool AllowGroupRecall
        {
            get { return _AllowGroupRecall; }
            set
            {
                if (_AllowGroupRecall == value) return;

                var oldValue = _AllowGroupRecall;
                _AllowGroupRecall = value;

                OnChanged(oldValue, value, "AllowGroupRecall");
            }
        }
        private bool _AllowGroupRecall;
        


        [Association("Member")]
        public GuildMemberInfo GuildMember
        {
            get { return _GuildMember; }
            set
            {
                if (_GuildMember == value) return;

                var oldValue = _GuildMember;
                _GuildMember = value;

                OnChanged(oldValue, value, "GuildMember");
            }
        }
        private GuildMemberInfo _GuildMember;

        public DateTime GlobalTime
        {
            get { return _GlobalTime; }
            set
            {
                if (_GlobalTime == value) return;

                var oldValue = _GlobalTime;
                _GlobalTime = value;

                OnChanged(oldValue, value, "GlobalTime");
            }
        }
        private DateTime _GlobalTime;

        public HorseType Horse
        {
            get { return _Horse; }
            set
            {
                if (_Horse == value) return;

                var oldValue = _Horse;
                _Horse = value;

                OnChanged(oldValue, value, "Horse");
            }
        }
        private HorseType _Horse;
        
        public int HuntGold
        {
            get { return _HuntGold; }
            set
            {
                if (_HuntGold == value) return;

                var oldValue = _HuntGold;
                _HuntGold = value;

                OnChanged(oldValue, value, "HuntGold");
            }
        }
        private int _HuntGold;

        
        public int Shengwang
        {
            get { return _Shengwang; }
            set
            {
                if (_Shengwang == value) return;

                var oldValue = _Shengwang;
                _Shengwang = value;

                OnChanged(oldValue, value, "Shengwang");
            }
        }
        private int _Shengwang;
        
        public int Fubendian
        {
            get { return _Fubendian; }
            set
            {
                if (_Fubendian == value) return;

                var oldValue = _Fubendian;
                _Fubendian = value;

                OnChanged(oldValue, value, "Fubendian");
            }
        }
        private int _Fubendian;

        public bool Admin
        {
            get { return _Admin; }
            set
            {
                if (_Admin == value) return;

                var oldValue = _Admin;
                _Admin = value;

                OnChanged(oldValue, value, "Admin");
            }
        }
        private bool _Admin;

        public int StorageSize
        {
            get { return _StorageSize; }
            set
            {
                if (_StorageSize == value) return;

                var oldValue = _StorageSize;
                _StorageSize = value;

                OnChanged(oldValue, value, "StorageSize");
            }
        }
        private int _StorageSize;

        public int PatchGridSize
        {
            get
            {
                return _PatchGridSize;
            }
            set
            {
                if (_PatchGridSize == value)
                    return;
                int patchGridSize = _PatchGridSize;
                _PatchGridSize = value;
                OnChanged(patchGridSize, value, nameof(PatchGridSize));
            }
        }
        private int _PatchGridSize;

        
        public int BaoshiGridSize
        {
            get
            {
                return _BaoshiGridSize;
            }
            set
            {
                if (_BaoshiGridSize == value)
                    return;
                int BaoshiGridSize = _BaoshiGridSize;
                _BaoshiGridSize = value;
                OnChanged(BaoshiGridSize, value, nameof(BaoshiGridSize));
            }
        }
        private int _BaoshiGridSize;

        public string LastSum
        {
            get { return _LastSum; }
            set
            {
                if (_LastSum == value) return;

                var oldValue = _LastSum;
                _LastSum = value;

                OnChanged(oldValue, value, "LastSum");
            }
        }
        private string _LastSum;

        public bool GoldBot
        {
            get { return _GoldBot; }
            set
            {
                if (_GoldBot == value) return;

                var oldValue = _GoldBot;
                _GoldBot = value;

                OnChanged(oldValue, value, "GoldBot");
            }
        }
        private bool _GoldBot;

        public bool ItemBot
        {
            get { return _ItemBot; }
            set
            {
                if (_ItemBot == value) return;

                var oldValue = _ItemBot;
                _ItemBot = value;

                OnChanged(oldValue, value, "ItemBot");
            }
        }
        private bool _ItemBot;
        

        public DateTime GuildTime
        {
            get { return _GuildTime; }
            set
            {
                if (_GuildTime == value) return;

                var oldValue = _GuildTime;
                _GuildTime = value;

                OnChanged(oldValue, value, "GuildTime");
            }
        }
        private DateTime _GuildTime;

        public bool Observer
        {
            get { return _Observer; }
            set
            {
                if (_Observer == value) return;

                var oldValue = _Observer;
                _Observer = value;

                OnChanged(oldValue, value, "Observer");
            }
        }
        private bool _Observer;

        public bool TempAdmin;

        public int DailyRandomQuestResets
        {
            get { return _DailyRandomQuestResets; }
            set
            {
                if (_DailyRandomQuestResets == value) return;

                var oldValue = _DailyRandomQuestResets;
                _DailyRandomQuestResets = value;

                OnChanged(oldValue, value, "DailyRandomQuestResets");
            }
        }
        private int _DailyRandomQuestResets;

        public bool HasDailyRandom
        {
            get { return _HasDailyRandom; }
            set
            {
                if (_HasDailyRandom == value) return;

                var oldValue = _HasDailyRandom;
                _HasDailyRandom = value;

                OnChanged(oldValue, value, "HasDailyRandom");
            }
        }
        private bool _HasDailyRandom;

        public DateTime LastMeiriQuestGained
        {
            get { return _LastMeiriQuestGained; }
            set
            {
                if (_LastMeiriQuestGained == value) return;

                var oldValue = _LastMeiriQuestGained;
                _LastMeiriQuestGained = value;

                OnChanged(oldValue, value, "LastMeiriQuestGained");
            }
        }
        private DateTime _LastMeiriQuestGained;


        [Association("Items")]
        public DBBindingList<UserItem> Items { get; set; }

        [Association("Referrals")]
        public DBBindingList<AccountInfo> Referrals { get; set; }

        [Association("Characters")]
        public DBBindingList<CharacterInfo> Characters { get; set; }

        [Association("Buffs")]
        public DBBindingList<BuffInfo> Buffs { get; set; }

        [Association("Auctions")]
        public DBBindingList<AuctionInfo> Auctions { get; set; }
        
        [Association("Mail")]
        public DBBindingList<MailInfo> Mail { get; set; }
        
        [Association("UserDrops")]
        public DBBindingList<UserDrop> UserDrops { get; set; }

        [Association("Companions")]
        public DBBindingList<UserCompanion> Companions { get; set; }

        [Association("CompanionUnlocks")]
        public DBBindingList<UserCompanionUnlock> CompanionUnlocks { get; set; }

        [Association("HorseUnlocks")]
        public DBBindingList<UserHorseUnlock> HorseUnlocks { get; set; }

        [Association("Horses")]
        public DBBindingList<UserHorse> Horses { get; set; }


        [Association("BlockingList")]
        public DBBindingList<BlockInfo> BlockingList { get; set; } 

        [Association("BlockedByList")]
        public DBBindingList<BlockInfo> BlockedByList { get; set; } 

        [Association("Payments")]
        public DBBindingList<GameGoldPayment> Payments { get; set; }

        [Association("StoreSales")]
        public DBBindingList<GameStoreSale> StoreSales { get; set; }

        [Association("Fortunes")]
        public DBBindingList<UserFortuneInfo> Fortunes { get; set; }

        [Association("Crafting")]
        public DBBindingList<UserCrafting> Crafting { get; set; }

        [Association("ShenmiCount")]
        public DBBindingList<UserShenmiCount> ShenmiCount { get; set; }

        [Association("MeiriQuests", true)]
        public DBBindingList<MeiriUserQuest> MeiriQuests { get; set; }

        public CharacterInfo LastCharacter
        {
            get { return _LastCharacter; }
            set
            {
                if (_LastCharacter == value) return;

                var oldValue = _LastCharacter;
                _LastCharacter = value;

                OnChanged(oldValue, value, "LastCharacter");
            }
        }
        private CharacterInfo _LastCharacter;
        

        public int WrongPasswordCount;
        public SConnection Connection;
        public string Key;

        protected override void OnCreated()
        {
            base.OnCreated();

            StorageSize = CartoonGlobals.StorageSize;

            BuffInfo buff = SEnvir.BuffInfoList.CreateNewObject();
            
            buff.Account = this;
            buff.Type = BuffType.HuntGold;
            buff.TickFrequency = TimeSpan.FromMinutes(1);
            buff.Stats = new Stats { [Stat.AvailableHuntGoldCap] = 15 };
            buff.RemainingTime = TimeSpan.MaxValue;
            
            Zanzhujilu = 0;
            
            GameGoldPrice = 0;

            UserCrafting craftingClothing = SEnvir.UserCraftInfoList.CreateNewObject();
            craftingClothing.Account = this;
            craftingClothing.Type = CraftType.Clothing;
            craftingClothing.Level = 1;
            craftingClothing.Exp = 0;

            UserCrafting craftingConsumables = SEnvir.UserCraftInfoList.CreateNewObject();
            craftingConsumables.Account = this;
            craftingConsumables.Type = CraftType.Consumables;
            craftingConsumables.Level = 1;
            craftingConsumables.Exp = 0;

            UserCrafting craftingJewelry = SEnvir.UserCraftInfoList.CreateNewObject();
            craftingJewelry.Account = this;
            craftingJewelry.Type = CraftType.Jewelry;
            craftingJewelry.Level = 1;
            craftingJewelry.Exp = 0;

            UserCrafting craftingSmithing = SEnvir.UserCraftInfoList.CreateNewObject();
            craftingSmithing.Account = this;
            craftingSmithing.Type = CraftType.Smithing;
            craftingSmithing.Level = 1;
            craftingSmithing.Exp = 0;

            UserCrafting craftingRusted = SEnvir.UserCraftInfoList.CreateNewObject();
            craftingRusted.Account = this;
            craftingRusted.Type = CraftType.Rusted;
            craftingRusted.Level = 1;
            craftingRusted.Exp = 0;

            UserShenmiCount shenmiClothing = SEnvir.UserShenmiInfoList.CreateNewObject();
            shenmiClothing.Account = this;
            shenmiClothing.ShenmiCountyi = 0;
            shenmiClothing.ShenmiCounter = 0;
            shenmiClothing.ShenmiCountsan = 0;
            shenmiClothing.ShenmiCountsi = 0;
            shenmiClothing.ShenmiCountwu = 0;
            shenmiClothing.ShenmiCountliu = 0;
            shenmiClothing.ShenmiCountqi = 0;
            shenmiClothing.ShenmiCountba = 0;
            shenmiClothing.ShenmiCountjiu = 0;
            shenmiClothing.ShenmiCountshi = 0;

            DailyRandomQuestResets = 2;
            HasDailyRandom = false;
            LastMeiriQuestGained = DateTime.Now;
            
            Fubendian = CartoonGlobals.Fubendian;
        }
        protected override void OnLoaded()
        {
            base.OnLoaded();

            if (StorageSize == 0)
                StorageSize = CartoonGlobals.StorageSize;
            
            Buffs.First(x => x.Type == BuffType.HuntGold).Stats = new Stats { [Stat.AvailableHuntGoldCap] = 15 };
            
            
        }

        public int HightestLevel()
        {
            int count = 0;

            foreach (CharacterInfo character in Characters)
                if (character.Level > count)
                    count = character.Level;

            return count;
        }

        public List<SelectInfo> GetSelectInfo()
        {
            List<SelectInfo> characters = new List<SelectInfo>();

            foreach (CharacterInfo character in Characters)
            {
                if (character.Deleted) continue;

                characters.Add(character.ToSelectInfo());
            }

            return characters;
        }

        public override string ToString()
        {
            return EMailAddress;
        }
    }
}
