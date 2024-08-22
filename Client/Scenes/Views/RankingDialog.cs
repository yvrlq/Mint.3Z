﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Client.Controls;
using Client.Envir;
using Client.UserModels;
using Library;
using S =  Library.Network.ServerPackets;
using C = Library.Network.ClientPackets;
using System.Reflection;
using System.ComponentModel;


namespace Client.Scenes.Views
{
    public sealed class RankingDialog : DXWindow
    {
        #region Properties
        #region StartIndex

        public int StartIndex
        {
            get => _StartIndex;
            set
            {
                if (_StartIndex == value) return;

                int oldValue = _StartIndex;
                _StartIndex = value;

                OnStartIndexChanged(oldValue, value);
            }
        }
        private int _StartIndex;
        public event EventHandler<EventArgs> StartIndexChanged;
        public void OnStartIndexChanged(int oValue, int nValue)
        {
            UpdateTime = CEnvir.Now.AddMilliseconds(250);
            
            if (nValue > oValue)
                for (int i = 0; i < Lines.Length; i++)
                {
                    if (nValue - oValue + i < Lines.Length)
                    {
                        if (Lines[i + nValue - oValue].Rank != null)
                        {
                            Lines[i].Rank = Lines[i + nValue - oValue].Rank;
                        }
                        else
                            Lines[i].Loading = true;
                    }
                    else
                        Lines[i].Loading = true;

                }
            else
                for (int i = Lines.Length - 1; i >= 0; i--)
                {
                    if (nValue - oValue + i >= 0)
                    {
                        if (Lines[i + nValue - oValue].Rank != null)
                        Lines[i].Rank = Lines[i + nValue - oValue].Rank;
                        else
                            Lines[i].Loading = true;
                    }
                    else
                        Lines[i].Loading = true;
                }



            StartIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Class

        public RequiredClass Class
        {
            get => _Class;
            set
            {
                if (_Class == value) return;

                RequiredClass oldValue = _Class;
                _Class = value;

                OnClassChanged(oldValue, value);
            }
        }
        private RequiredClass _Class;
        public event EventHandler<EventArgs> ClassChanged;
        public void OnClassChanged(RequiredClass oValue, RequiredClass nValue)
        {
            ScrollBar.Value = 0;
            UpdateTime = CEnvir.Now;

            foreach (RankingLine line in Lines)
                line.Loading = true;


            ClassChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region OnlineOnly

        public bool OnlineOnly
        {
            get => _OnlineOnly;
            set
            {
                if (_OnlineOnly == value) return;

                bool oldValue = _OnlineOnly;
                _OnlineOnly = value;

                OnOnlineOnlyChanged(oldValue, value);
            }
        }
        private bool _OnlineOnly;
        public event EventHandler<EventArgs> OnlineOnlyChanged;
        public void OnOnlineOnlyChanged(bool oValue, bool nValue)
        {
            ScrollBar.Value = 0;
            UpdateTime = CEnvir.Now;

            foreach (RankingLine line in Lines)
                line.Loading = true;


            OnlineOnlyChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Observable

        public bool Observable
        {
            get => _Observable;
            set
            {
                if (_Observable == value) return;

                bool oldValue = _Observable;
                _Observable = value;

                OnObserverableChanged(oldValue, value);
            }
        }
        private bool _Observable;
        public event EventHandler<EventArgs> ObserverableChanged;
        public void OnObserverableChanged(bool oValue, bool nValue)
        {
            if (Observable)
            {
                ObserverButton.Index = 121;
                ObserverButton.Hint = "观察者模式: 允许";
            }
            else
            {
                ObserverButton.Index = 141;
                ObserverButton.Hint = "观察者模式: 拒绝";
            }

            ObserverableChanged?.Invoke(this, EventArgs.Empty);

        }

        #endregion
        
        public DateTime UpdateTime;

        private DXControl Panel;
        private DXVScrollBar ScrollBar;

        public DXComboBox RequiredClassBox;
        public DXCheckBox OnlineOnlyBox;
        public DXButton ObserverButton;

        public RankingLine[] Lines;

        public override WindowType Type => WindowType.RankingBox;
        public override bool CustomSize => false;
        public override bool AutomaticVisiblity => true;
        #endregion

        public RankingDialog()
        {
            SetClientSize(new Size(410, 480));

            TitleLabel.Text = "排行榜";
            
            Panel = new DXControl
            {
                Location = new Point(ClientArea.Location.X + 1, ClientArea.Location.Y+20),
                Size = new Size(ClientArea.Size.Width - 1, ClientArea.Size.Height - 20),
                Parent = this,
            };

            Lines = new RankingLine[20];
            ScrollBar = new DXVScrollBar
            {
                Parent = Panel,
                Size = new Size(14, Panel.Size.Height - 3 - 21),
                Location = new Point(Panel.Size.Width - 16, 1 +22),
                VisibleSize = Lines.Length,
                Change = 5,
            };
            ScrollBar.ValueChanged += (o, e) => StartIndex = ScrollBar.Value;
            MouseWheel += ScrollBar.DoMouseWheel;

            new RankingLine
            {
                Parent = Panel,
                Header = true,
            };

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new RankingLine
                {
                    Parent = Panel,
                    Location = new Point(0, 22* (i +1)),
                    Visible = false,
                };
                Lines[i].MouseWheel += ScrollBar.DoMouseWheel;
            }


            ObserverButton = new DXButton
            {
                Parent = this,
                LibraryFile = LibraryFile.GameInter2,
                Index = 141,
                Hint = "观察者模式: 拒绝",
            };
            ObserverButton.MouseClick += (o, e) =>
            {
                if (GameScene.Game == null) return;
                if (GameScene.Game.Observer) return;
                if (!GameScene.Game.User.InSafeZone)
                {
                    GameScene.Game.ReceiveChat("你只能在安全区内更改观察者模式.", MessageType.System);
                    return;
                }

                CEnvir.Enqueue(new C.ObservableSwitch { Allow = !Observable });
            };
            ObserverButton.Location = new Point(ClientArea.Right - ObserverButton.Size.Width, ClientArea.Top);

            OnlineOnlyBox = new DXCheckBox
            {
                Parent = this,
                Label = { Text = "在线" },
            };
            OnlineOnlyBox.CheckedChanged += (o, e) =>
            {
                OnlineOnly = OnlineOnlyBox.Checked;
                Config.RankingOnline = OnlineOnly;
            };
            OnlineOnlyBox.Location = new Point(269 - OnlineOnlyBox.Size.Width + ClientArea.X, ClientArea.Y);

            RequiredClassBox = new DXComboBox
            {
                Parent = this,
                Size = new Size(100, DXComboBox.DefaultNormalHeight),
                Location = new Point(ClientArea.X + 43, ClientArea.Y)
            };
            RequiredClassBox.SelectedItemChanged += (o, e) =>
            {
                Class = (RequiredClass?) RequiredClassBox.SelectedItem ?? RequiredClass.All;
                Config.RankingClass = (int)Class;
            };
            


            new DXListBoxItem
            {
                Parent = RequiredClassBox.ListBox,
                Label = { Text = "全部" },
                Item = RequiredClass.All
            };

            new DXListBoxItem
            {
                Parent = RequiredClassBox.ListBox,
                Label = { Text = "战士" },
                Item = RequiredClass.Warrior
            };
            new DXListBoxItem
            {
                Parent = RequiredClassBox.ListBox,
                Label = { Text = "法师" },
                Item = RequiredClass.Wizard
            };
            new DXListBoxItem
            {
                Parent = RequiredClassBox.ListBox,
                Label = { Text = "道士" },
                Item = RequiredClass.Taoist
            };

            new DXListBoxItem
            {
                Parent = RequiredClassBox.ListBox,
                Label = { Text = "刺客" },
                Item = RequiredClass.Assassin
            };


            DXLabel label = new DXLabel
            {
                Parent = this,
                Text = "职业:",
            };
            label.Location = new Point(RequiredClassBox.Location.X - label.Size.Width - 5, RequiredClassBox.Location.Y + (RequiredClassBox.Size.Height - label.Size.Height) / 2);

            RequiredClassBox.ListBox.SelectItem((RequiredClass)Config.RankingClass);
            OnlineOnlyBox.Checked = Config.RankingOnline;
        }

        #region Methods

        public override void Process()
        {
            base.Process();

            if (CEnvir.Now < UpdateTime) return;

            UpdateTime = CEnvir.Now.AddSeconds(10);

            CEnvir.Enqueue(new C.RankRequest
            {
                Class = Class,
                OnlineOnly = OnlineOnly,
                StartIndex = StartIndex,
            });
        }
        public void Update(S.Rankings p)
        {
            if (p.Class != Class || p.OnlineOnly != OnlineOnly) return;

            ScrollBar.MaxValue = p.Total;

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i].Loading = false;
                Lines[i].Rank = i >= p.Ranks.Count ? null : p.Ranks[i];
            }
        }
        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _StartIndex = 0;
                StartIndexChanged = null;

                _Class = 0;
                ClassChanged = null;

                _OnlineOnly = false;
                OnlineOnlyChanged = null;

                _Observable = false;
                ObserverableChanged = null;

                UpdateTime = DateTime.MinValue;

                if (Panel != null)
                {
                    if (!Panel.IsDisposed)
                        Panel.Dispose();

                    Panel = null;
                }

                if (ScrollBar != null)
                {
                    if (!ScrollBar.IsDisposed)
                        ScrollBar.Dispose();

                    ScrollBar = null;
                }

                if (RequiredClassBox != null)
                {
                    if (!RequiredClassBox.IsDisposed)
                        RequiredClassBox.Dispose();

                    RequiredClassBox = null;
                }

                if (OnlineOnlyBox != null)
                {
                    if (!OnlineOnlyBox.IsDisposed)
                        OnlineOnlyBox.Dispose();

                    OnlineOnlyBox = null;
                }

                if (ObserverButton != null)
                {
                    if (!ObserverButton.IsDisposed)
                        ObserverButton.Dispose();

                    ObserverButton = null;
                }

                if (Lines != null)
                {
                    for (int i = 0; i < Lines.Length; i++)
                    {
                        if (Lines[i] != null)
                        {
                            if (!Lines[i].IsDisposed)
                                Lines[i].Dispose();

                            Lines[i] = null;
                        }
                    }

                    Lines = null;
                }


            }

        }

        #endregion
    }

    public sealed class RankingLine : DXControl
    {
        #region Properties

        #region Header

        public bool Header
        {
            get => _Header;
            set
            {
                if (_Header == value) return;

                bool oldValue = _Header;
                _Header = value;

                OnHeaderChanged(oldValue, value);
            }
        }
        private bool _Header;
        public event EventHandler<EventArgs> HeaderChanged;
        public void OnHeaderChanged(bool oValue, bool nValue)
        {
            RankLabel.Text = "排行";
            NameLabel.Text = "名字";
            ClassLabel.Text = "职业";
            LevelLabel.Text = "等级";
            ZhuanshenLabel.Text = "转生";

            DrawTexture = false;

            RankLabel.ForeColour = Color.FromArgb(198, 166, 99);
            NameLabel.ForeColour = Color.FromArgb(198, 166, 99);
            ClassLabel.ForeColour = Color.FromArgb(198, 166, 99);
            LevelLabel.ForeColour = Color.FromArgb(198, 166, 99);
            ZhuanshenLabel.ForeColour = Color.FromArgb(198, 166, 99);

            OnlineImage.Visible = false;

            ObseverButton.Dispose();
            HeaderChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Rank

        public RankInfo Rank
        {
            get => _Rank;
            set
            {
                if (_Rank == value) return;

                RankInfo oldValue = _Rank;
                _Rank = value;

                OnRankChanged(oldValue, value);
            }
        }
        private RankInfo _Rank;
        public event EventHandler<EventArgs> RankChanged;
        public void OnRankChanged(RankInfo oValue, RankInfo nValue)
        {
            if (Rank == null)
            {
                RankLabel.Text = "";
                NameLabel.Text = "";
                ClassLabel.Text = "";
                LevelLabel.Text = "";
                ZhuanshenLabel.Text = "";

                ObseverButton.Enabled = false;
                Visible = !Loading;
            }
            else
            {
                Visible = true;
                RankLabel.Text = Rank.Rank.ToString();
                NameLabel.Text = Rank.Name;

                Type itemType = Rank.Class.GetType();
                MemberInfo[] infos = itemType.GetMember(Rank.Class.ToString());

                DescriptionAttribute description = infos[0].GetCustomAttribute<DescriptionAttribute>();

                ClassLabel.Text = $"{description?.Description ?? Rank.Class.ToString()}";
                RankLabel.ForeColour = Color.Silver;
                NameLabel.ForeColour = Color.Silver;
                ClassLabel.ForeColour = Color.Silver;
                LevelLabel.ForeColour = Color.Silver;
                ZhuanshenLabel.ForeColour = Color.Red;

                OnlineImage.Index = Rank.Online ? 3625 : 3624;

                ObseverButton.Enabled = Rank.Online && Rank.Observable;

                decimal percent = 0;

                /*
                if (Rank.Level < CartoonGlobals.ExperienceList.Count)
                    percent = Math.Min(1, Math.Max(0, CartoonGlobals.ExperienceList[Rank.Level] > 0 ? Rank.Experience/CartoonGlobals.ExperienceList[Rank.Level] : 0));
                */

                Rank.Level -= Rank.Zhuanshen * 5000;

                if (Rank.Level < 0) return;

                if (Rank.Level < CartoonGlobals.ExperienceList.Count)
                    percent = Math.Min(1, Math.Max(0, CartoonGlobals.ExperienceList[Rank.Level] > 0 ? Rank.Experience / CartoonGlobals.ExperienceList[Rank.Level] : 0));

                LevelLabel.Text = $"{Rank.Level} - {percent:0.##%}";

                if (Rank.Zhuanshen > 0)
                    ZhuanshenLabel.Text = Rank.Zhuanshen == 1 ? string.Format("{0} 转", Rank.Zhuanshen) : string.Format("{0} 转", Rank.Zhuanshen);
                else ZhuanshenLabel.Text = "";
            }

            RankChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Loading

        public bool Loading
        {
            get => _Loading;
            set
            {
                if (_Loading == value) return;

                bool oldValue = _Loading;
                _Loading = value;

                OnLoadingChanged(oldValue, value);
            }
        }
        private bool _Loading;
        public event EventHandler<EventArgs> LoadingChanged;
        public void OnLoadingChanged(bool oValue, bool nValue)
        {
            if (!Loading)
            {
                RankLabel.Text = "";
                NameLabel.Text = "";
                ClassLabel.Text = "";
                LevelLabel.Text = "";
                ZhuanshenLabel.Text = "";


                Visible = false;
                return;
            }

            Rank = null;
            NameLabel.Text = "更新...";
            NameLabel.ForeColour = Color.Orange;
            Visible = true;

            LoadingChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public DXLabel RankLabel, NameLabel, ClassLabel, LevelLabel, ZhuanshenLabel;
        public DXButton ObseverButton;
        public DXImageControl OnlineImage;

        #endregion

        public RankingLine()
        {
            Size = new Size(390, 20);
            DrawTexture = true;
            BackColour = Color.FromArgb(25, 20, 0);

            OnlineImage = new DXImageControl
            {
                Parent = this,
                LibraryFile = LibraryFile.GameInter,
                Index = 3624,
                Location = new Point(8, 4),
                IsControl = false,
            };

            RankLabel = new DXLabel
            {
                Parent = this,
              
                AutoSize = false,
                Location = new Point(OnlineImage.Location.X + OnlineImage.Size.Width - 4, 0),
                Size = new Size(40, 18),
               
                ForeColour = Color.White,
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                IsControl = false,
            };

            NameLabel = new DXLabel
            {
                Parent = this,
                AutoSize = false,
                Location = new Point(RankLabel.Location.X + RankLabel.Size.Width + 1, 0),
                Size = new Size(100, 18),
                ForeColour = Color.White,
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                IsControl = false,
            };

            ClassLabel = new DXLabel
            {
                Parent = this,
                AutoSize = false,
                Location = new Point(NameLabel.Location.X + NameLabel.Size.Width + 1, 0),
                Size = new Size(50, 18),
                ForeColour = Color.White,
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                IsControl = false,
            };

            LevelLabel = new DXLabel
            {
                Parent = this,
                AutoSize = false,
                Location = new Point(ClassLabel.Location.X + ClassLabel.Size.Width + 1, 0),
                Size = new Size(70, 18),
                ForeColour = Color.White,
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                IsControl = false,
            };

            ZhuanshenLabel = new DXLabel
            {
                Parent = this,
                AutoSize = false,
                Location = new Point(LevelLabel.Location.X + LevelLabel.Size.Width + 1, 0),
                Size = new Size(50, 18),
                ForeColour = Color.White,
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                IsControl = false,
            };

            ObseverButton = new DXButton
            {
                Parent = this,
                Location = new Point(ZhuanshenLabel.Location.X + ZhuanshenLabel.Size.Width + 5, 1),
                ButtonType = ButtonType.SmallButton,
                Label = { Text = "观察" },
                Enabled = false,
                Size = new Size(53, SmallButtonHeight)
            };
            ObseverButton.MouseClick += (o, e) =>
            {
                if (GameScene.Game != null && CEnvir.Now < GameScene.Game.User.CombatTime.AddSeconds(10) && !GameScene.Game.Observer)
                {
                    GameScene.Game.ReceiveChat("在战斗中不能观察.", MessageType.System);
                    return;
                }

                CEnvir.Enqueue(new C.ObserverRequest { Name = Rank.Name });
            };

        }

        #region Methods

        public override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (GameScene.Game == null || Rank == null) return;

            CEnvir.Enqueue(new C.Inspect { Index = Rank.Index });
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();
            if (Header) return;
            BackColour = Color.FromArgb(80, 80, 125);
        }

        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            if (Header) return;

            BackColour = Color.FromArgb(25, 20, 0);
        }
        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _Header = false;
                HeaderChanged = null;

                _Rank = null;
                RankChanged = null;

                _Loading = false;
                LoadingChanged = null;

                if (RankLabel != null)
                {
                    if (!RankLabel.IsDisposed)
                        RankLabel.Dispose();

                    RankLabel = null;
                }

                if (NameLabel != null)
                {
                    if (!NameLabel.IsDisposed)
                        NameLabel.Dispose();

                    NameLabel = null;
                }

                if (ClassLabel != null)
                {
                    if (!ClassLabel.IsDisposed)
                        ClassLabel.Dispose();

                    ClassLabel = null;
                }

                if (LevelLabel != null)
                {
                    if (!LevelLabel.IsDisposed)
                        LevelLabel.Dispose();

                    LevelLabel = null;
                }

                if (ZhuanshenLabel != null)
                {
                    if (!ZhuanshenLabel.IsDisposed)
                        ZhuanshenLabel.Dispose();

                    ZhuanshenLabel = null;
                }

                if (ObseverButton != null)
                {
                    if (!ObseverButton.IsDisposed)
                        ObseverButton.Dispose();

                    ObseverButton = null;
                }

                if (OnlineImage != null)
                {
                    if (!OnlineImage.IsDisposed)
                        OnlineImage.Dispose();

                    OnlineImage = null;
                }
            }

        }

        #endregion
    }

}
