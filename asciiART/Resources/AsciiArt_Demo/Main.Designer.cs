namespace AsciiArt_Demo
{
  partial class Main
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
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.CmdConvert = new System.Windows.Forms.Button();
            this.CmdSelectFile = new System.Windows.Forms.Button();
            this.txtImgPath = new System.Windows.Forms.TextBox();
            this.fdOpen = new System.Windows.Forms.OpenFileDialog();
            this.pbAsciify = new System.Windows.Forms.ProgressBar();
            this.chkColour = new System.Windows.Forms.CheckBox();
            this.mAsciiImageSize = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Browser
            // 
            this.Browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Browser.Location = new System.Drawing.Point(13, 13);
            this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(753, 380);
            this.Browser.TabIndex = 0;
            this.Browser.WebBrowserShortcutsEnabled = false;
            // 
            // CmdConvert
            // 
            this.CmdConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CmdConvert.Location = new System.Drawing.Point(691, 399);
            this.CmdConvert.Name = "CmdConvert";
            this.CmdConvert.Size = new System.Drawing.Size(75, 23);
            this.CmdConvert.TabIndex = 1;
            this.CmdConvert.Text = "Convert";
            this.CmdConvert.UseVisualStyleBackColor = true;
            this.CmdConvert.Click += new System.EventHandler(this.CmdConvert_Click);
            // 
            // CmdSelectFile
            // 
            this.CmdSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CmdSelectFile.Location = new System.Drawing.Point(435, 399);
            this.CmdSelectFile.Name = "CmdSelectFile";
            this.CmdSelectFile.Size = new System.Drawing.Size(34, 23);
            this.CmdSelectFile.TabIndex = 2;
            this.CmdSelectFile.Text = "...";
            this.CmdSelectFile.UseVisualStyleBackColor = true;
            this.CmdSelectFile.Click += new System.EventHandler(this.CmdSelectImage_Click);
            // 
            // txtImgPath
            // 
            this.txtImgPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImgPath.Location = new System.Drawing.Point(12, 401);
            this.txtImgPath.Name = "txtImgPath";
            this.txtImgPath.Size = new System.Drawing.Size(417, 20);
            this.txtImgPath.TabIndex = 3;
            // 
            // fdOpen
            // 
            this.fdOpen.Title = "Select image file";
            // 
            // pbAsciify
            // 
            this.pbAsciify.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbAsciify.Location = new System.Drawing.Point(12, 428);
            this.pbAsciify.Name = "pbAsciify";
            this.pbAsciify.Size = new System.Drawing.Size(754, 23);
            this.pbAsciify.TabIndex = 5;
            // 
            // chkColour
            // 
            this.chkColour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkColour.AutoSize = true;
            this.chkColour.Location = new System.Drawing.Point(627, 403);
            this.chkColour.Name = "chkColour";
            this.chkColour.Size = new System.Drawing.Size(56, 17);
            this.chkColour.TabIndex = 6;
            this.chkColour.Text = "Colour";
            this.chkColour.UseVisualStyleBackColor = true;
            // 
            // mAsciiImageSize
            // 
            this.mAsciiImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mAsciiImageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mAsciiImageSize.FormattingEnabled = true;
            this.mAsciiImageSize.Items.AddRange(new object[] {
            "micro [320x240]",
            "tiny [480x360]",
            "small [640x480]",
            "medium [800x600]",
            "large [1024x768]",
            "extra large [1280x1024]",
            "super large [1600x1200]"});
            this.mAsciiImageSize.Location = new System.Drawing.Point(475, 399);
            this.mAsciiImageSize.Name = "mAsciiImageSize";
            this.mAsciiImageSize.Size = new System.Drawing.Size(146, 21);
            this.mAsciiImageSize.TabIndex = 7;
            // 
            // Main
            // 
            this.AcceptButton = this.CmdConvert;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 463);
            this.Controls.Add(this.mAsciiImageSize);
            this.Controls.Add(this.chkColour);
            this.Controls.Add(this.pbAsciify);
            this.Controls.Add(this.txtImgPath);
            this.Controls.Add(this.CmdSelectFile);
            this.Controls.Add(this.CmdConvert);
            this.Controls.Add(this.Browser);
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Ascii Art";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.WebBrowser Browser;
    private System.Windows.Forms.Button CmdConvert;
    private System.Windows.Forms.Button CmdSelectFile;
    private System.Windows.Forms.TextBox txtImgPath;
    private System.Windows.Forms.OpenFileDialog fdOpen;
    private System.Windows.Forms.ProgressBar pbAsciify;
    private System.Windows.Forms.CheckBox chkColour;
    private System.Windows.Forms.ComboBox mAsciiImageSize;
  }
}

