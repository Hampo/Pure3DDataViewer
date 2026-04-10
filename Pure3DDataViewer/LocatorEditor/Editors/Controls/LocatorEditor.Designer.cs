namespace LocatorEditor.Editors.Controls;

partial class LocatorEditor
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        GBPosition = new GroupBox();
        BtnFromGameIncludingTriggers = new Button();
        BtnTeleportInGame = new Button();
        BtnFromGameExcludingTriggers = new Button();
        LblPositionZ = new Label();
        NTBPositionZ = new Pure3DDataViewerPluginAPI.Controls.NumericTextBox();
        LblPositionY = new Label();
        NTBPositionY = new Pure3DDataViewerPluginAPI.Controls.NumericTextBox();
        LblPositionX = new Label();
        NTBPositionX = new Pure3DDataViewerPluginAPI.Controls.NumericTextBox();
        GBTypeDataEditor = new GroupBox();
        GBPosition.SuspendLayout();
        SuspendLayout();
        // 
        // GBPosition
        // 
        GBPosition.Controls.Add(BtnFromGameIncludingTriggers);
        GBPosition.Controls.Add(BtnTeleportInGame);
        GBPosition.Controls.Add(BtnFromGameExcludingTriggers);
        GBPosition.Controls.Add(LblPositionZ);
        GBPosition.Controls.Add(NTBPositionZ);
        GBPosition.Controls.Add(LblPositionY);
        GBPosition.Controls.Add(NTBPositionY);
        GBPosition.Controls.Add(LblPositionX);
        GBPosition.Controls.Add(NTBPositionX);
        GBPosition.Dock = DockStyle.Top;
        GBPosition.Location = new Point(0, 0);
        GBPosition.Name = "GBPosition";
        GBPosition.Size = new Size(450, 140);
        GBPosition.TabIndex = 0;
        GBPosition.TabStop = false;
        GBPosition.Text = "Position";
        // 
        // BtnFromGameIncludingTriggers
        // 
        BtnFromGameIncludingTriggers.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnFromGameIncludingTriggers.Location = new Point(298, 109);
        BtnFromGameIncludingTriggers.Name = "BtnFromGameIncludingTriggers";
        BtnFromGameIncludingTriggers.Size = new Size(146, 23);
        BtnFromGameIncludingTriggers.TabIndex = 8;
        BtnFromGameIncludingTriggers.Text = "From Game (w/ Triggers)";
        BtnFromGameIncludingTriggers.UseVisualStyleBackColor = true;
        BtnFromGameIncludingTriggers.Click += BtnFromGameIncludingTriggers_Click;
        // 
        // BtnTeleportInGame
        // 
        BtnTeleportInGame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnTeleportInGame.Location = new Point(6, 109);
        BtnTeleportInGame.Name = "BtnTeleportInGame";
        BtnTeleportInGame.Size = new Size(127, 23);
        BtnTeleportInGame.TabIndex = 7;
        BtnTeleportInGame.Text = "Teleport To In Game";
        BtnTeleportInGame.UseVisualStyleBackColor = true;
        BtnTeleportInGame.Click += BtnTeleportInGame_Click;
        // 
        // BtnFromGameExcludingTriggers
        // 
        BtnFromGameExcludingTriggers.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnFromGameExcludingTriggers.Location = new Point(139, 109);
        BtnFromGameExcludingTriggers.Name = "BtnFromGameExcludingTriggers";
        BtnFromGameExcludingTriggers.Size = new Size(153, 23);
        BtnFromGameExcludingTriggers.TabIndex = 6;
        BtnFromGameExcludingTriggers.Text = "From Game (w/o Triggers)";
        BtnFromGameExcludingTriggers.UseVisualStyleBackColor = true;
        BtnFromGameExcludingTriggers.Click += BtnFromGameExcludingTriggers_Click;
        // 
        // LblPositionZ
        // 
        LblPositionZ.AutoSize = true;
        LblPositionZ.Location = new Point(6, 83);
        LblPositionZ.Name = "LblPositionZ";
        LblPositionZ.Size = new Size(17, 15);
        LblPositionZ.TabIndex = 5;
        LblPositionZ.Text = "Z:";
        // 
        // NTBPositionZ
        // 
        NTBPositionZ.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        NTBPositionZ.BackColor = Color.White;
        NTBPositionZ.InvalidColor = Color.Pink;
        NTBPositionZ.Location = new Point(29, 80);
        NTBPositionZ.Name = "NTBPositionZ";
        NTBPositionZ.NumericType = Pure3DDataViewerPluginAPI.Controls.NumericTextBox.NumericTypes.Float;
        NTBPositionZ.Size = new Size(415, 23);
        NTBPositionZ.TabIndex = 4;
        NTBPositionZ.ValidColor = Color.White;
        NTBPositionZ.Value = null;
        NTBPositionZ.Leave += NTBPosition_Leave;
        // 
        // LblPositionY
        // 
        LblPositionY.AutoSize = true;
        LblPositionY.Location = new Point(6, 54);
        LblPositionY.Name = "LblPositionY";
        LblPositionY.Size = new Size(17, 15);
        LblPositionY.TabIndex = 3;
        LblPositionY.Text = "Y:";
        // 
        // NTBPositionY
        // 
        NTBPositionY.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        NTBPositionY.BackColor = Color.White;
        NTBPositionY.InvalidColor = Color.Pink;
        NTBPositionY.Location = new Point(29, 51);
        NTBPositionY.Name = "NTBPositionY";
        NTBPositionY.NumericType = Pure3DDataViewerPluginAPI.Controls.NumericTextBox.NumericTypes.Float;
        NTBPositionY.Size = new Size(415, 23);
        NTBPositionY.TabIndex = 2;
        NTBPositionY.ValidColor = Color.White;
        NTBPositionY.Value = null;
        NTBPositionY.Leave += NTBPosition_Leave;
        // 
        // LblPositionX
        // 
        LblPositionX.AutoSize = true;
        LblPositionX.Location = new Point(6, 25);
        LblPositionX.Name = "LblPositionX";
        LblPositionX.Size = new Size(17, 15);
        LblPositionX.TabIndex = 1;
        LblPositionX.Text = "X:";
        // 
        // NTBPositionX
        // 
        NTBPositionX.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        NTBPositionX.BackColor = Color.White;
        NTBPositionX.InvalidColor = Color.Pink;
        NTBPositionX.Location = new Point(29, 22);
        NTBPositionX.Name = "NTBPositionX";
        NTBPositionX.NumericType = Pure3DDataViewerPluginAPI.Controls.NumericTextBox.NumericTypes.Float;
        NTBPositionX.Size = new Size(415, 23);
        NTBPositionX.TabIndex = 0;
        NTBPositionX.ValidColor = Color.White;
        NTBPositionX.Value = null;
        NTBPositionX.Leave += NTBPosition_Leave;
        // 
        // GBTypeDataEditor
        // 
        GBTypeDataEditor.Dock = DockStyle.Fill;
        GBTypeDataEditor.Location = new Point(0, 140);
        GBTypeDataEditor.Name = "GBTypeDataEditor";
        GBTypeDataEditor.Size = new Size(450, 280);
        GBTypeDataEditor.TabIndex = 0;
        GBTypeDataEditor.TabStop = false;
        GBTypeDataEditor.Text = "Type Data";
        // 
        // LocatorEditor
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(GBTypeDataEditor);
        Controls.Add(GBPosition);
        Name = "LocatorEditor";
        Size = new Size(450, 420);
        GBPosition.ResumeLayout(false);
        GBPosition.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
    private GroupBox GBPosition;
    private Label LblPositionZ;
    private Pure3DDataViewerPluginAPI.Controls.NumericTextBox NTBPositionZ;
    private Label LblPositionY;
    private Pure3DDataViewerPluginAPI.Controls.NumericTextBox NTBPositionY;
    private Label LblPositionX;
    private Pure3DDataViewerPluginAPI.Controls.NumericTextBox NTBPositionX;
    private Button BtnTeleportInGame;
    private Button BtnFromGameExcludingTriggers;
    private Button BtnFromGameIncludingTriggers;
    private GroupBox GBTypeDataEditor;
}
