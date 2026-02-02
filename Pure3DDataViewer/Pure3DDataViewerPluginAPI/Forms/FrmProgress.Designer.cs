namespace Pure3DDataViewerPluginAPI.Forms;

partial class FrmProgress
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
        PBProgress = new ProgressBar();
        SuspendLayout();
        // 
        // PBProgress
        // 
        PBProgress.Dock = DockStyle.Fill;
        PBProgress.Location = new Point(0, 0);
        PBProgress.Name = "PBProgress";
        PBProgress.Size = new Size(250, 23);
        PBProgress.TabIndex = 0;
        // 
        // FrmProgress
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(250, 23);
        Controls.Add(PBProgress);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmProgress";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Import Progress";
        ResumeLayout(false);
    }

    #endregion

    private ProgressBar PBProgress;
}