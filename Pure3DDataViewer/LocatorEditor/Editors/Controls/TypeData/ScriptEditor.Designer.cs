namespace LocatorEditor.Editors.Controls.TypeData;

partial class ScriptEditor
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
        LblKey = new Label();
        TxtKey = new TextBox();
        SuspendLayout();
        // 
        // LblKey
        // 
        LblKey.AutoSize = true;
        LblKey.Location = new Point(3, 6);
        LblKey.Name = "LblKey";
        LblKey.Size = new Size(29, 15);
        LblKey.TabIndex = 0;
        LblKey.Text = "Key:";
        // 
        // TxtKey
        // 
        TxtKey.Location = new Point(38, 3);
        TxtKey.Name = "TxtKey";
        TxtKey.Size = new Size(409, 23);
        TxtKey.TabIndex = 1;
        TxtKey.Leave += TxtKey_Leave;
        // 
        // ScriptEditor
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(TxtKey);
        Controls.Add(LblKey);
        Name = "ScriptEditor";
        Size = new Size(450, 280);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label LblKey;
    private TextBox TxtKey;
}
