namespace Pure3DDataViewerPluginAPI.Controls;

partial class ColorPicker
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
        NUDAlpha = new NumericUpDown();
        PnlColour = new Panel();
        ((System.ComponentModel.ISupportInitialize)NUDAlpha).BeginInit();
        SuspendLayout();
        // 
        // NUDAlpha
        // 
        NUDAlpha.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        NUDAlpha.Location = new Point(29, 0);
        NUDAlpha.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        NUDAlpha.Name = "NUDAlpha";
        NUDAlpha.Size = new Size(228, 23);
        NUDAlpha.TabIndex = 7;
        // 
        // PnlColour
        // 
        PnlColour.BorderStyle = BorderStyle.FixedSingle;
        PnlColour.Cursor = Cursors.Hand;
        PnlColour.Location = new Point(0, 0);
        PnlColour.Name = "PnlColour";
        PnlColour.Size = new Size(23, 23);
        PnlColour.TabIndex = 6;
        PnlColour.Click += PnlColour_Click;
        // 
        // ColorPicker
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(NUDAlpha);
        Controls.Add(PnlColour);
        Name = "ColorPicker";
        Size = new Size(257, 23);
        ((System.ComponentModel.ISupportInitialize)NUDAlpha).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private NumericUpDown NUDAlpha;
    private Panel PnlColour;
}
