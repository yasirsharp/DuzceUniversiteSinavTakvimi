namespace FrontendAdmin
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.bolumYonetim = new System.Windows.Forms.TabPage();
            this.gbBolumYonetim = new System.Windows.Forms.GroupBox();
            this.btnBolumKaydet = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblId = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dersYonetim = new System.Windows.Forms.TabPage();
            this.akademikPersonelYonetim = new System.Windows.Forms.TabPage();
            this.bolumDersAkademikPersonelYonetim = new System.Windows.Forms.TabPage();
            this.derslikYonetim = new System.Windows.Forms.TabPage();
            this.sinavTakvim = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.bolumYonetim.SuspendLayout();
            this.gbBolumYonetim.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.bolumYonetim);
            this.tabControl1.Controls.Add(this.dersYonetim);
            this.tabControl1.Controls.Add(this.akademikPersonelYonetim);
            this.tabControl1.Controls.Add(this.bolumDersAkademikPersonelYonetim);
            this.tabControl1.Controls.Add(this.derslikYonetim);
            this.tabControl1.Controls.Add(this.sinavTakvim);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1410, 705);
            this.tabControl1.TabIndex = 0;
            // 
            // bolumYonetim
            // 
            this.bolumYonetim.Controls.Add(this.gbBolumYonetim);
            this.bolumYonetim.Controls.Add(this.label2);
            this.bolumYonetim.Controls.Add(this.label1);
            this.bolumYonetim.Location = new System.Drawing.Point(4, 25);
            this.bolumYonetim.Name = "bolumYonetim";
            this.bolumYonetim.Padding = new System.Windows.Forms.Padding(3);
            this.bolumYonetim.Size = new System.Drawing.Size(1402, 676);
            this.bolumYonetim.TabIndex = 0;
            this.bolumYonetim.Text = "Bölümler";
            this.bolumYonetim.UseVisualStyleBackColor = true;
            // 
            // gbBolumYonetim
            // 
            this.gbBolumYonetim.Controls.Add(this.btnBolumKaydet);
            this.gbBolumYonetim.Controls.Add(this.textBox1);
            this.gbBolumYonetim.Controls.Add(this.lblId);
            this.gbBolumYonetim.Location = new System.Drawing.Point(706, 26);
            this.gbBolumYonetim.Name = "gbBolumYonetim";
            this.gbBolumYonetim.Size = new System.Drawing.Size(667, 152);
            this.gbBolumYonetim.TabIndex = 2;
            this.gbBolumYonetim.TabStop = false;
            this.gbBolumYonetim.Text = "Yeni Bölüm Ekle";
            // 
            // btnBolumKaydet
            // 
            this.btnBolumKaydet.Location = new System.Drawing.Point(19, 114);
            this.btnBolumKaydet.Name = "btnBolumKaydet";
            this.btnBolumKaydet.Size = new System.Drawing.Size(627, 23);
            this.btnBolumKaydet.TabIndex = 2;
            this.btnBolumKaydet.Text = "Kaydet";
            this.btnBolumKaydet.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(19, 72);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(627, 22);
            this.textBox1.TabIndex = 1;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(16, 34);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(0, 16);
            this.lblId.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(153, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bölüm Adı";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(45, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Id";
            // 
            // dersYonetim
            // 
            this.dersYonetim.Location = new System.Drawing.Point(4, 25);
            this.dersYonetim.Name = "dersYonetim";
            this.dersYonetim.Padding = new System.Windows.Forms.Padding(3);
            this.dersYonetim.Size = new System.Drawing.Size(1402, 676);
            this.dersYonetim.TabIndex = 1;
            this.dersYonetim.Text = "Dersler";
            this.dersYonetim.UseVisualStyleBackColor = true;
            // 
            // akademikPersonelYonetim
            // 
            this.akademikPersonelYonetim.Location = new System.Drawing.Point(4, 25);
            this.akademikPersonelYonetim.Name = "akademikPersonelYonetim";
            this.akademikPersonelYonetim.Size = new System.Drawing.Size(1402, 676);
            this.akademikPersonelYonetim.TabIndex = 2;
            this.akademikPersonelYonetim.Text = "Akademik Personeller";
            this.akademikPersonelYonetim.UseVisualStyleBackColor = true;
            // 
            // bolumDersAkademikPersonelYonetim
            // 
            this.bolumDersAkademikPersonelYonetim.Location = new System.Drawing.Point(4, 25);
            this.bolumDersAkademikPersonelYonetim.Name = "bolumDersAkademikPersonelYonetim";
            this.bolumDersAkademikPersonelYonetim.Size = new System.Drawing.Size(1402, 676);
            this.bolumDersAkademikPersonelYonetim.TabIndex = 3;
            this.bolumDersAkademikPersonelYonetim.Text = "Bolum-Ders-Akademik Personel";
            this.bolumDersAkademikPersonelYonetim.UseVisualStyleBackColor = true;
            // 
            // derslikYonetim
            // 
            this.derslikYonetim.Location = new System.Drawing.Point(4, 25);
            this.derslikYonetim.Name = "derslikYonetim";
            this.derslikYonetim.Size = new System.Drawing.Size(1402, 676);
            this.derslikYonetim.TabIndex = 4;
            this.derslikYonetim.Text = "Derslikler";
            this.derslikYonetim.UseVisualStyleBackColor = true;
            // 
            // sinavTakvim
            // 
            this.sinavTakvim.Location = new System.Drawing.Point(4, 25);
            this.sinavTakvim.Name = "sinavTakvim";
            this.sinavTakvim.Size = new System.Drawing.Size(1402, 676);
            this.sinavTakvim.TabIndex = 5;
            this.sinavTakvim.Text = "Sınav Takvimi";
            this.sinavTakvim.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1411, 707);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.bolumYonetim.ResumeLayout(false);
            this.bolumYonetim.PerformLayout();
            this.gbBolumYonetim.ResumeLayout(false);
            this.gbBolumYonetim.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage bolumYonetim;
        private System.Windows.Forms.TabPage dersYonetim;
        private System.Windows.Forms.TabPage akademikPersonelYonetim;
        private System.Windows.Forms.TabPage bolumDersAkademikPersonelYonetim;
        private System.Windows.Forms.TabPage derslikYonetim;
        private System.Windows.Forms.TabPage sinavTakvim;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbBolumYonetim;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Button btnBolumKaydet;
    }
}

