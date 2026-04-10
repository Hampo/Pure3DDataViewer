namespace LocatorEditor.Editors.Controls.TypeData;

partial class EventEditor
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
        LblEvent = new Label();
        CBEvent = new ComboBox();
        CBParameter = new CheckBox();
        NTBParameterUint = new Pure3DDataViewerPluginAPI.Controls.NumericTextBox();
        LblNoParameter = new Label();
        NTBParameterInt = new Pure3DDataViewerPluginAPI.Controls.NumericTextBox();
        NTBParameterFloat = new Pure3DDataViewerPluginAPI.Controls.NumericTextBox();
        CPParameter = new Pure3DDataViewerPluginAPI.Controls.ColorPicker();
        CBParameterValue = new CheckBox();
        SuspendLayout();
        // 
        // LblEvent
        // 
        LblEvent.AutoSize = true;
        LblEvent.Location = new Point(47, 6);
        LblEvent.Name = "LblEvent";
        LblEvent.Size = new Size(39, 15);
        LblEvent.TabIndex = 0;
        LblEvent.Text = "Event:";
        // 
        // CBEvent
        // 
        CBEvent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        CBEvent.DropDownStyle = ComboBoxStyle.DropDownList;
        CBEvent.FormattingEnabled = true;
        CBEvent.Location = new Point(92, 3);
        CBEvent.Name = "CBEvent";
        CBEvent.Size = new Size(355, 23);
        CBEvent.TabIndex = 1;
        CBEvent.SelectedValueChanged += CBEvent_SelectedValueChanged;
        // 
        // CBParameter
        // 
        CBParameter.AutoSize = true;
        CBParameter.Location = new Point(3, 34);
        CBParameter.Name = "CBParameter";
        CBParameter.Size = new Size(83, 19);
        CBParameter.TabIndex = 2;
        CBParameter.Text = "Parameter:";
        CBParameter.UseVisualStyleBackColor = true;
        CBParameter.CheckedChanged += CBParameter_CheckedChanged;
        // 
        // NTBParameterUint
        // 
        NTBParameterUint.BackColor = Color.White;
        NTBParameterUint.InvalidColor = Color.Pink;
        NTBParameterUint.Location = new Point(92, 32);
        NTBParameterUint.Name = "NTBParameterUint";
        NTBParameterUint.NumericType = Pure3DDataViewerPluginAPI.Controls.NumericTextBox.NumericTypes.UInt;
        NTBParameterUint.Size = new Size(355, 23);
        NTBParameterUint.TabIndex = 3;
        NTBParameterUint.ValidColor = Color.White;
        NTBParameterUint.Value = null;
        NTBParameterUint.TextChanged += NTBParameterUint_TextChanged;
        // 
        // LblNoParameter
        // 
        LblNoParameter.AutoSize = true;
        LblNoParameter.Location = new Point(6, 35);
        LblNoParameter.Name = "LblNoParameter";
        LblNoParameter.Size = new Size(80, 15);
        LblNoParameter.TabIndex = 4;
        LblNoParameter.Text = "No Parameter";
        // 
        // NTBParameterInt
        // 
        NTBParameterInt.BackColor = Color.White;
        NTBParameterInt.InvalidColor = Color.Pink;
        NTBParameterInt.Location = new Point(92, 32);
        NTBParameterInt.Name = "NTBParameterInt";
        NTBParameterInt.Size = new Size(355, 23);
        NTBParameterInt.TabIndex = 5;
        NTBParameterInt.ValidColor = Color.White;
        NTBParameterInt.Value = null;
        NTBParameterInt.TextChanged += NTBParameterInt_TextChanged;
        // 
        // NTBParameterFloat
        // 
        NTBParameterFloat.BackColor = Color.White;
        NTBParameterFloat.InvalidColor = Color.Pink;
        NTBParameterFloat.Location = new Point(92, 32);
        NTBParameterFloat.Name = "NTBParameterFloat";
        NTBParameterFloat.NumericType = Pure3DDataViewerPluginAPI.Controls.NumericTextBox.NumericTypes.Float;
        NTBParameterFloat.Size = new Size(355, 23);
        NTBParameterFloat.TabIndex = 6;
        NTBParameterFloat.ValidColor = Color.White;
        NTBParameterFloat.Value = null;
        NTBParameterFloat.TextChanged += NTBParameterFloat_TextChanged;
        // 
        // CPParameter
        // 
        CPParameter.Location = new Point(92, 32);
        CPParameter.Name = "CPParameter";
        CPParameter.Size = new Size(355, 23);
        CPParameter.TabIndex = 7;
        CPParameter.Value = Color.FromArgb(0, 240, 240, 240);
        CPParameter.ValueChanged += CPParameter_ValueChanged;
        // 
        // CBParameterValue
        // 
        CBParameterValue.AutoSize = true;
        CBParameterValue.Location = new Point(92, 36);
        CBParameterValue.Name = "CBParameterValue";
        CBParameterValue.Size = new Size(15, 14);
        CBParameterValue.TabIndex = 8;
        CBParameterValue.UseVisualStyleBackColor = true;
        CBParameterValue.CheckedChanged += CBParameterValue_CheckedChanged;
        // 
        // EventEditor
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(CBParameterValue);
        Controls.Add(CPParameter);
        Controls.Add(NTBParameterFloat);
        Controls.Add(NTBParameterInt);
        Controls.Add(NTBParameterUint);
        Controls.Add(CBParameter);
        Controls.Add(LblNoParameter);
        Controls.Add(CBEvent);
        Controls.Add(LblEvent);
        Name = "EventEditor";
        Size = new Size(450, 280);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label LblEvent;
    private ComboBox CBEvent;
    private CheckBox CBParameter;
    private Pure3DDataViewerPluginAPI.Controls.NumericTextBox NTBParameterUint;
    private Label LblNoParameter;
    private Pure3DDataViewerPluginAPI.Controls.NumericTextBox NTBParameterInt;
    private Pure3DDataViewerPluginAPI.Controls.NumericTextBox NTBParameterFloat;
    private Pure3DDataViewerPluginAPI.Controls.ColorPicker CPParameter;
    private CheckBox CBParameterValue;
}
