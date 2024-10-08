﻿namespace Server.Views
{
    partial class CompanionInfoView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanionInfoView));
            ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            SaveButton = new DevExpress.XtraBars.BarButtonItem();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            CompanionInfoGridControl = new DevExpress.XtraGrid.GridControl();
            CompanionInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            MonsterInfoLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            tabPane1 = new DevExpress.XtraBars.Navigation.TabPane();
            tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            tabNavigationPage2 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            CompanionLevelInfoGridControl = new DevExpress.XtraGrid.GridControl();
            CompanionLevelInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            tabNavigationPage3 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            CompanionSkillInfoGridControl = new DevExpress.XtraGrid.GridControl();
            CompanionSkillInfoGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(CompanionInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(CompanionInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(MonsterInfoLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(tabPane1)).BeginInit();
            tabPane1.SuspendLayout();
            tabNavigationPage1.SuspendLayout();
            tabNavigationPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(CompanionLevelInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(CompanionLevelInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemLookUpEdit1)).BeginInit();
            tabNavigationPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(CompanionSkillInfoGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(CompanionSkillInfoGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemLookUpEdit2)).BeginInit();
            SuspendLayout();
            // 
            // ribbon
            // 
            ribbon.ExpandCollapseItem.Id = 0;
            ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            ribbon.ExpandCollapseItem,
            SaveButton});
            ribbon.Location = new System.Drawing.Point(0, 0);
            ribbon.MaxItemId = 2;
            ribbon.Name = "ribbon";
            ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            ribbonPage1});
            ribbon.Size = new System.Drawing.Size(972, 144);
            // 
            // SaveButton
            // 
            SaveButton.Caption = "Save Database";
            SaveButton.Glyph = ((System.Drawing.Image)(resources.GetObject("SaveButton.Glyph")));
            SaveButton.Id = 1;
            SaveButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("SaveButton.LargeGlyph")));
            SaveButton.LargeWidth = 60;
            SaveButton.Name = "SaveButton";
            SaveButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SaveButton_ItemClick);
            // 
            // ribbonPage1
            // 
            ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            ribbonPageGroup1});
            ribbonPage1.Name = "ribbonPage1";
            ribbonPage1.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            ribbonPageGroup1.AllowTextClipping = false;
            ribbonPageGroup1.ItemLinks.Add(SaveButton);
            ribbonPageGroup1.Name = "ribbonPageGroup1";
            ribbonPageGroup1.ShowCaptionButton = false;
            ribbonPageGroup1.Text = "Saving";
            // 
            // CompanionInfoGridControl
            // 
            CompanionInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            CompanionInfoGridControl.Location = new System.Drawing.Point(0, 0);
            CompanionInfoGridControl.MainView = CompanionInfoGridView;
            CompanionInfoGridControl.MenuManager = ribbon;
            CompanionInfoGridControl.Name = "CompanionInfoGridControl";
            CompanionInfoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            MonsterInfoLookUpEdit});
            CompanionInfoGridControl.Size = new System.Drawing.Size(954, 355);
            CompanionInfoGridControl.TabIndex = 2;
            CompanionInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            CompanionInfoGridView});
            // 
            // CompanionInfoGridView
            // 
            CompanionInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            gridColumn1,
            gridColumn3,
            gridColumn4});
            CompanionInfoGridView.GridControl = CompanionInfoGridControl;
            CompanionInfoGridView.Name = "CompanionInfoGridView";
            CompanionInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            CompanionInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            CompanionInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            CompanionInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            CompanionInfoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            gridColumn1.ColumnEdit = MonsterInfoLookUpEdit;
            gridColumn1.FieldName = "MonsterInfo";
            gridColumn1.Name = "gridColumn1";
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 0;
            // 
            // MonsterInfoLookUpEdit
            // 
            MonsterInfoLookUpEdit.AutoHeight = false;
            MonsterInfoLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            MonsterInfoLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            MonsterInfoLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MonsterName", "Monster Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AI", "AI"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Level", "Level"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Experience", "Experience"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsBoss", "Is Boss")});
            MonsterInfoLookUpEdit.DisplayMember = "MonsterName";
            MonsterInfoLookUpEdit.Name = "MonsterInfoLookUpEdit";
            MonsterInfoLookUpEdit.NullText = "[Monster is null]";
            // 
            // gridColumn3
            // 
            gridColumn3.FieldName = "Price";
            gridColumn3.Name = "gridColumn3";
            gridColumn3.Visible = true;
            gridColumn3.VisibleIndex = 1;
            // 
            // gridColumn4
            // 
            gridColumn4.FieldName = "Available";
            gridColumn4.Name = "gridColumn4";
            gridColumn4.Visible = true;
            gridColumn4.VisibleIndex = 2;
            // 
            // tabPane1
            // 
            tabPane1.Controls.Add(tabNavigationPage1);
            tabPane1.Controls.Add(tabNavigationPage2);
            tabPane1.Controls.Add(tabNavigationPage3);
            tabPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabPane1.Location = new System.Drawing.Point(0, 144);
            tabPane1.Name = "tabPane1";
            tabPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            tabNavigationPage1,
            tabNavigationPage2,
            tabNavigationPage3});
            tabPane1.RegularSize = new System.Drawing.Size(972, 400);
            tabPane1.SelectedPage = tabNavigationPage1;
            tabPane1.Size = new System.Drawing.Size(972, 400);
            tabPane1.TabIndex = 3;
            // 
            // tabNavigationPage1
            // 
            tabNavigationPage1.Caption = "Companion Info";
            tabNavigationPage1.Controls.Add(CompanionInfoGridControl);
            tabNavigationPage1.Name = "tabNavigationPage1";
            tabNavigationPage1.Size = new System.Drawing.Size(954, 355);
            // 
            // tabNavigationPage2
            // 
            tabNavigationPage2.Caption = "Companion Level Info";
            tabNavigationPage2.Controls.Add(CompanionLevelInfoGridControl);
            tabNavigationPage2.Name = "tabNavigationPage2";
            tabNavigationPage2.Size = new System.Drawing.Size(960, 358);
            // 
            // CompanionLevelInfoGridControl
            // 
            CompanionLevelInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            CompanionLevelInfoGridControl.Location = new System.Drawing.Point(0, 0);
            CompanionLevelInfoGridControl.MainView = CompanionLevelInfoGridView;
            CompanionLevelInfoGridControl.MenuManager = ribbon;
            CompanionLevelInfoGridControl.Name = "CompanionLevelInfoGridControl";
            CompanionLevelInfoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            repositoryItemLookUpEdit1});
            CompanionLevelInfoGridControl.Size = new System.Drawing.Size(960, 358);
            CompanionLevelInfoGridControl.TabIndex = 3;
            CompanionLevelInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            CompanionLevelInfoGridView});
            // 
            // CompanionLevelInfoGridView
            // 
            CompanionLevelInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            gridColumn2,
            gridColumn5,
            gridColumn6,
            gridColumn7,
            gridColumn8});
            CompanionLevelInfoGridView.GridControl = CompanionLevelInfoGridControl;
            CompanionLevelInfoGridView.Name = "CompanionLevelInfoGridView";
            CompanionLevelInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            CompanionLevelInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            CompanionLevelInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            CompanionLevelInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            CompanionLevelInfoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn2
            // 
            gridColumn2.FieldName = "Level";
            gridColumn2.Name = "gridColumn2";
            gridColumn2.Visible = true;
            gridColumn2.VisibleIndex = 0;
            // 
            // gridColumn5
            // 
            gridColumn5.FieldName = "MaxExperience";
            gridColumn5.Name = "gridColumn5";
            gridColumn5.Visible = true;
            gridColumn5.VisibleIndex = 1;
            // 
            // gridColumn6
            // 
            gridColumn6.FieldName = "InventorySpace";
            gridColumn6.Name = "gridColumn6";
            gridColumn6.Visible = true;
            gridColumn6.VisibleIndex = 2;
            // 
            // gridColumn7
            // 
            gridColumn7.FieldName = "InventoryWeight";
            gridColumn7.Name = "gridColumn7";
            gridColumn7.Visible = true;
            gridColumn7.VisibleIndex = 3;
            // 
            // gridColumn8
            // 
            gridColumn8.FieldName = "MaxHunger";
            gridColumn8.Name = "gridColumn8";
            gridColumn8.Visible = true;
            gridColumn8.VisibleIndex = 4;
            // 
            // repositoryItemLookUpEdit1
            // 
            repositoryItemLookUpEdit1.AutoHeight = false;
            repositoryItemLookUpEdit1.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MonsterName", "Monster Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AI", "AI"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Level", "Level"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Experience", "Experience"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsBoss", "Is Boss")});
            repositoryItemLookUpEdit1.DisplayMember = "MonsterName";
            repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            repositoryItemLookUpEdit1.NullText = "[Monster is null]";
            // 
            // tabNavigationPage3
            // 
            tabNavigationPage3.Caption = "Companion Skill Info";
            tabNavigationPage3.Controls.Add(CompanionSkillInfoGridControl);
            tabNavigationPage3.Name = "tabNavigationPage3";
            tabNavigationPage3.Size = new System.Drawing.Size(960, 358);
            // 
            // CompanionSkillInfoGridControl
            // 
            CompanionSkillInfoGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            CompanionSkillInfoGridControl.Location = new System.Drawing.Point(0, 0);
            CompanionSkillInfoGridControl.MainView = CompanionSkillInfoGridView;
            CompanionSkillInfoGridControl.MenuManager = ribbon;
            CompanionSkillInfoGridControl.Name = "CompanionSkillInfoGridControl";
            CompanionSkillInfoGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            repositoryItemLookUpEdit2});
            CompanionSkillInfoGridControl.Size = new System.Drawing.Size(960, 358);
            CompanionSkillInfoGridControl.TabIndex = 4;
            CompanionSkillInfoGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            CompanionSkillInfoGridView});
            // 
            // CompanionSkillInfoGridView
            // 
            CompanionSkillInfoGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            gridColumn9,
            gridColumn10,
            gridColumn11,
            gridColumn12,
            gridColumn13});
            CompanionSkillInfoGridView.GridControl = CompanionSkillInfoGridControl;
            CompanionSkillInfoGridView.Name = "CompanionSkillInfoGridView";
            CompanionSkillInfoGridView.OptionsView.EnableAppearanceEvenRow = true;
            CompanionSkillInfoGridView.OptionsView.EnableAppearanceOddRow = true;
            CompanionSkillInfoGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            CompanionSkillInfoGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            CompanionSkillInfoGridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn9
            // 
            gridColumn9.FieldName = "Level";
            gridColumn9.Name = "gridColumn9";
            gridColumn9.Visible = true;
            gridColumn9.VisibleIndex = 0;
            // 
            // gridColumn10
            // 
            gridColumn10.FieldName = "StatType";
            gridColumn10.Name = "gridColumn10";
            gridColumn10.Visible = true;
            gridColumn10.VisibleIndex = 1;
            // 
            // gridColumn11
            // 
            gridColumn11.FieldName = "ImgIndex";
            gridColumn11.Name = "gridColumn11";
            gridColumn11.Visible = true;
            gridColumn11.VisibleIndex = 2;
            // 
            // gridColumn12
            // 
            gridColumn12.FieldName = "MaxAmount";
            gridColumn12.Name = "gridColumn12";
            gridColumn12.Visible = true;
            gridColumn12.VisibleIndex = 3;
            // 
            // gridColumn13
            // 
            gridColumn13.FieldName = "Weight";
            gridColumn13.Name = "gridColumn13";
            gridColumn13.Visible = true;
            gridColumn13.VisibleIndex = 4;
            // 
            // repositoryItemLookUpEdit2
            // 
            repositoryItemLookUpEdit2.AutoHeight = false;
            repositoryItemLookUpEdit2.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            repositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MonsterName", "Monster Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AI", "AI"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Level", "Level"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Experience", "Experience"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsBoss", "Is Boss")});
            repositoryItemLookUpEdit2.DisplayMember = "MonsterName";
            repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            repositoryItemLookUpEdit2.NullText = "[Monster is null]";
            // 
            // CompanionInfoView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(972, 544);
            Controls.Add(tabPane1);
            Controls.Add(ribbon);
            Name = "CompanionInfoView";
            Ribbon = ribbon;
            Text = "CompanionInfoView";
            ((System.ComponentModel.ISupportInitialize)(ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(CompanionInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(CompanionInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(MonsterInfoLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(tabPane1)).EndInit();
            tabPane1.ResumeLayout(false);
            tabNavigationPage1.ResumeLayout(false);
            tabNavigationPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(CompanionLevelInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(CompanionLevelInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemLookUpEdit1)).EndInit();
            tabNavigationPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(CompanionSkillInfoGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(CompanionSkillInfoGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemLookUpEdit2)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SaveButton;
        private DevExpress.XtraGrid.GridControl CompanionInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CompanionInfoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraBars.Navigation.TabPane tabPane1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MonsterInfoLookUpEdit;
        private DevExpress.XtraGrid.GridControl CompanionLevelInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CompanionLevelInfoGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage3;
        private DevExpress.XtraGrid.GridControl CompanionSkillInfoGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CompanionSkillInfoGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
    }
}