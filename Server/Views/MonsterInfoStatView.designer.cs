﻿namespace Server.Views
{
    partial class MonsterInfoStatView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonsterInfoStatView));
            ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            SaveButton = new DevExpress.XtraBars.BarButtonItem();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            MonsterInfoStatGridControl = new DevExpress.XtraGrid.GridControl();
            MonsterInfoStatGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            MonsterLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            StatImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(MonsterInfoStatGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(MonsterInfoStatGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(MonsterLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(StatImageComboBox)).BeginInit();
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
            ribbon.Size = new System.Drawing.Size(692, 147);
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
            // MonsterInfoStatGridControl
            // 
            MonsterInfoStatGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            MonsterInfoStatGridControl.Location = new System.Drawing.Point(0, 147);
            MonsterInfoStatGridControl.MainView = MonsterInfoStatGridView;
            MonsterInfoStatGridControl.MenuManager = ribbon;
            MonsterInfoStatGridControl.Name = "MonsterInfoStatGridControl";
            MonsterInfoStatGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            MonsterLookUpEdit,
            StatImageComboBox});
            MonsterInfoStatGridControl.Size = new System.Drawing.Size(692, 326);
            MonsterInfoStatGridControl.TabIndex = 2;
            MonsterInfoStatGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            MonsterInfoStatGridView});
            // 
            // MonsterInfoStatGridView
            // 
            MonsterInfoStatGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            gridColumn1,
            gridColumn2,
            gridColumn3});
            MonsterInfoStatGridView.GridControl = MonsterInfoStatGridControl;
            MonsterInfoStatGridView.Name = "MonsterInfoStatGridView";
            MonsterInfoStatGridView.OptionsView.EnableAppearanceEvenRow = true;
            MonsterInfoStatGridView.OptionsView.EnableAppearanceOddRow = true;
            MonsterInfoStatGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            MonsterInfoStatGridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            MonsterInfoStatGridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            gridColumn1.ColumnEdit = MonsterLookUpEdit;
            gridColumn1.FieldName = "Monster";
            gridColumn1.Name = "gridColumn1";
            gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            gridColumn1.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 0;
            // 
            // MonsterLookUpEdit
            // 
            MonsterLookUpEdit.AutoHeight = false;
            MonsterLookUpEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            MonsterLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            MonsterLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Index", "Index"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MonsterName", "Monster Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AI", "AI"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Level", "Level"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Experience", "Experience"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("IsBoss", "IsBoss")});
            MonsterLookUpEdit.DisplayMember = "MonsterName";
            MonsterLookUpEdit.Name = "MonsterLookUpEdit";
            MonsterLookUpEdit.NullText = "[Monster is null]";
            // 
            // gridColumn2
            // 
            gridColumn2.ColumnEdit = StatImageComboBox;
            gridColumn2.FieldName = "Stat";
            gridColumn2.Name = "gridColumn2";
            gridColumn2.Visible = true;
            gridColumn2.VisibleIndex = 1;
            // 
            // StatImageComboBox
            // 
            StatImageComboBox.AutoHeight = false;
            StatImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            StatImageComboBox.Name = "StatImageComboBox";
            // 
            // gridColumn3
            // 
            gridColumn3.FieldName = "Amount";
            gridColumn3.Name = "gridColumn3";
            gridColumn3.Visible = true;
            gridColumn3.VisibleIndex = 2;
            // 
            // MonsterInfoStatView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(692, 473);
            Controls.Add(MonsterInfoStatGridControl);
            Controls.Add(ribbon);
            Name = "MonsterInfoStatView";
            Ribbon = ribbon;
            Text = "Monster Info Stat";
            ((System.ComponentModel.ISupportInitialize)(ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(MonsterInfoStatGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(MonsterInfoStatGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(MonsterLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(StatImageComboBox)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem SaveButton;
        private DevExpress.XtraGrid.GridControl MonsterInfoStatGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView MonsterInfoStatGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit MonsterLookUpEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox StatImageComboBox;
    }
}