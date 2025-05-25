namespace Nintai_kids
{
    partial class PictureSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PictureSelector));
            lbltext = new Label();
            label1 = new Label();
            txtName = new TextBox();
            button1 = new Button();
            txtDescripcion = new TextBox();
            label2 = new Label();
            label3 = new Label();
            groupBox1 = new GroupBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            trackPitchValue = new TrackBar();
            lblPitchValue = new Label();
            comboSpeed = new ComboBox();
            comboGenero = new ComboBox();
            label8 = new Label();
            comboVoces = new ComboBox();
            btnPlay = new Button();
            btnSaveChanges = new Button();
            groupBox2 = new GroupBox();
            lblAlert = new Label();
            picAlert = new PictureBox();
            groupBox9 = new GroupBox();
            btnStop = new Button();
            comboBoxAmbiente = new ComboBox();
            label37 = new Label();
            btPlayAmbiente = new Button();
            panelAlert = new Panel();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackPitchValue).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAlert).BeginInit();
            groupBox9.SuspendLayout();
            panelAlert.SuspendLayout();
            SuspendLayout();
            // 
            // lbltext
            // 
            lbltext.AutoSize = true;
            lbltext.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lbltext.Location = new Point(27, 28);
            lbltext.Margin = new Padding(4, 0, 4, 0);
            lbltext.Name = "lbltext";
            lbltext.Size = new Size(65, 25);
            lbltext.TabIndex = 0;
            lbltext.Text = "label1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(67, 76);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(167, 25);
            label1.TabIndex = 1;
            label1.Text = "Nombre del item:";
            // 
            // txtName
            // 
            txtName.Location = new Point(269, 73);
            txtName.Margin = new Padding(4);
            txtName.Name = "txtName";
            txtName.Size = new Size(512, 32);
            txtName.TabIndex = 2;
            txtName.TextChanged += txtName_TextChanged;
            txtName.KeyDown += txtName_KeyDown;
            // 
            // button1
            // 
            button1.Location = new Point(229, 521);
            button1.Name = "button1";
            button1.Size = new Size(512, 56);
            button1.TabIndex = 12;
            button1.Text = "Ok";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(269, 113);
            txtDescripcion.Margin = new Padding(4);
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(512, 72);
            txtDescripcion.TabIndex = 3;
            txtDescripcion.TextChanged += txtDescripcion_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(108, 113);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(126, 25);
            label2.TabIndex = 4;
            label2.Text = "Descripcion :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.Indigo;
            label3.Location = new Point(7, 28);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(294, 50);
            label3.TabIndex = 5;
            label3.Text = "*No uses la palabra \"Narrador\" \r\ncomo nombre";
            label3.Click += label3_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.ForeColor = Color.RoyalBlue;
            groupBox1.Location = new Point(827, 28);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(318, 157);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tips:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label4.ForeColor = Color.Indigo;
            label4.Location = new Point(7, 85);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(261, 50);
            label4.TabIndex = 6;
            label4.Text = "*No uses parabras repetidas\r\n entre personajes";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(132, 20);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(50, 25);
            label5.TabIndex = 7;
            label5.Text = "Voz:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(115, 65);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(72, 25);
            label6.TabIndex = 8;
            label6.Text = "Speed:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(121, 110);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(61, 25);
            label7.TabIndex = 9;
            label7.Text = "Pitch:";
            // 
            // trackPitchValue
            // 
            trackPitchValue.BackColor = Color.White;
            trackPitchValue.Location = new Point(217, 110);
            trackPitchValue.Maximum = 20;
            trackPitchValue.Minimum = -20;
            trackPitchValue.Name = "trackPitchValue";
            trackPitchValue.Size = new Size(447, 56);
            trackPitchValue.TabIndex = 10;
            trackPitchValue.Scroll += trackPitchValue_Scroll;
            // 
            // lblPitchValue
            // 
            lblPitchValue.AutoSize = true;
            lblPitchValue.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblPitchValue.Location = new Point(668, 125);
            lblPitchValue.Margin = new Padding(4, 0, 4, 0);
            lblPitchValue.Name = "lblPitchValue";
            lblPitchValue.Size = new Size(38, 25);
            lblPitchValue.TabIndex = 13;
            lblPitchValue.Text = "0st";
            // 
            // comboSpeed
            // 
            comboSpeed.FormattingEnabled = true;
            comboSpeed.Location = new Point(217, 57);
            comboSpeed.Name = "comboSpeed";
            comboSpeed.Size = new Size(512, 33);
            comboSpeed.TabIndex = 14;
            comboSpeed.SelectedIndexChanged += comboSpeed_SelectedIndexChanged;
            // 
            // comboGenero
            // 
            comboGenero.FormattingEnabled = true;
            comboGenero.Location = new Point(840, 54);
            comboGenero.Name = "comboGenero";
            comboGenero.Size = new Size(253, 33);
            comboGenero.TabIndex = 15;
            comboGenero.SelectedIndexChanged += comboGenero_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(750, 57);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(83, 25);
            label8.TabIndex = 16;
            label8.Text = "Genero:";
            // 
            // comboVoces
            // 
            comboVoces.FormattingEnabled = true;
            comboVoces.Location = new Point(217, 17);
            comboVoces.Name = "comboVoces";
            comboVoces.Size = new Size(396, 33);
            comboVoces.TabIndex = 17;
            comboVoces.SelectedIndexChanged += comboVoces_SelectedIndexChanged;
            // 
            // btnPlay
            // 
            btnPlay.BackgroundImage = (Image)resources.GetObject("btnPlay.BackgroundImage");
            btnPlay.BackgroundImageLayout = ImageLayout.Stretch;
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.Location = new Point(619, 13);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(110, 39);
            btnPlay.TabIndex = 18;
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.BackColor = Color.MidnightBlue;
            btnSaveChanges.ForeColor = Color.White;
            btnSaveChanges.Location = new Point(893, 528);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(212, 43);
            btnSaveChanges.TabIndex = 19;
            btnSaveChanges.Text = "Save Changes";
            btnSaveChanges.UseVisualStyleBackColor = false;
            btnSaveChanges.Click += btnSaveChanges_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.Gainsboro;
            groupBox2.Controls.Add(panelAlert);
            groupBox2.Controls.Add(comboVoces);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(btnPlay);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(trackPitchValue);
            groupBox2.Controls.Add(comboGenero);
            groupBox2.Controls.Add(lblPitchValue);
            groupBox2.Controls.Add(comboSpeed);
            groupBox2.Location = new Point(12, 192);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1133, 178);
            groupBox2.TabIndex = 20;
            groupBox2.TabStop = false;
            groupBox2.Text = "Solo personajes";
            // 
            // lblAlert
            // 
            lblAlert.AutoSize = true;
            lblAlert.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblAlert.Location = new Point(73, 5);
            lblAlert.Margin = new Padding(4, 0, 4, 0);
            lblAlert.Name = "lblAlert";
            lblAlert.Size = new Size(83, 25);
            lblAlert.TabIndex = 20;
            lblAlert.Text = "Genero:";
            lblAlert.Visible = false;
            // 
            // picAlert
            // 
            picAlert.Image = (Image)resources.GetObject("picAlert.Image");
            picAlert.Location = new Point(8, 5);
            picAlert.Name = "picAlert";
            picAlert.Size = new Size(57, 54);
            picAlert.SizeMode = PictureBoxSizeMode.StretchImage;
            picAlert.TabIndex = 19;
            picAlert.TabStop = false;
            picAlert.Visible = false;
            // 
            // groupBox9
            // 
            groupBox9.BackColor = Color.Linen;
            groupBox9.Controls.Add(btnStop);
            groupBox9.Controls.Add(comboBoxAmbiente);
            groupBox9.Controls.Add(label37);
            groupBox9.Controls.Add(btPlayAmbiente);
            groupBox9.Location = new Point(12, 376);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(1133, 98);
            groupBox9.TabIndex = 44;
            groupBox9.TabStop = false;
            groupBox9.Text = "Solo para escenas";
            // 
            // btnStop
            // 
            btnStop.BackgroundImage = (Image)resources.GetObject("btnStop.BackgroundImage");
            btnStop.BackgroundImageLayout = ImageLayout.Stretch;
            btnStop.Cursor = Cursors.Hand;
            btnStop.Location = new Point(745, 38);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(40, 39);
            btnStop.TabIndex = 38;
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // comboBoxAmbiente
            // 
            comboBoxAmbiente.FormattingEnabled = true;
            comboBoxAmbiente.Location = new Point(217, 44);
            comboBoxAmbiente.Name = "comboBoxAmbiente";
            comboBoxAmbiente.Size = new Size(396, 33);
            comboBoxAmbiente.TabIndex = 36;
            comboBoxAmbiente.SelectedIndexChanged += comboBoxAmbiente_SelectedIndexChanged;
            // 
            // label37
            // 
            label37.AutoSize = true;
            label37.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            label37.Location = new Point(82, 38);
            label37.Margin = new Padding(4, 0, 4, 0);
            label37.Name = "label37";
            label37.Size = new Size(100, 50);
            label37.TabIndex = 35;
            label37.Text = "Audio de \r\nambiente:";
            // 
            // btPlayAmbiente
            // 
            btPlayAmbiente.BackgroundImage = (Image)resources.GetObject("btPlayAmbiente.BackgroundImage");
            btPlayAmbiente.BackgroundImageLayout = ImageLayout.Stretch;
            btPlayAmbiente.Cursor = Cursors.Hand;
            btPlayAmbiente.Location = new Point(619, 38);
            btPlayAmbiente.Name = "btPlayAmbiente";
            btPlayAmbiente.Size = new Size(110, 39);
            btPlayAmbiente.TabIndex = 37;
            btPlayAmbiente.UseVisualStyleBackColor = true;
            btPlayAmbiente.Click += btPlayAmbiente_Click;
            // 
            // panelAlert
            // 
            panelAlert.BackColor = Color.Yellow;
            panelAlert.Controls.Add(picAlert);
            panelAlert.Controls.Add(lblAlert);
            panelAlert.Location = new Point(736, 93);
            panelAlert.Name = "panelAlert";
            panelAlert.Size = new Size(391, 79);
            panelAlert.TabIndex = 21;
            panelAlert.Visible = false;
            // 
            // PictureSelector
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1157, 612);
            Controls.Add(groupBox9);
            Controls.Add(groupBox2);
            Controls.Add(btnSaveChanges);
            Controls.Add(groupBox1);
            Controls.Add(txtDescripcion);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(txtName);
            Controls.Add(label1);
            Controls.Add(lbltext);
            Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PictureSelector";
            Text = "Selection";
            FormClosing += PictureSelector_FormClosing;
            Load += PictureSelector_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackPitchValue).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAlert).EndInit();
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            panelAlert.ResumeLayout(false);
            panelAlert.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbltext;
        private Label label1;
        private TextBox txtName;
        private Button button1;
        private TextBox txtDescripcion;
        private Label label2;
        private Label label3;
        private GroupBox groupBox1;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TrackBar trackPitchValue;
        private Label lblPitchValue;
        private ComboBox comboSpeed;
        private ComboBox comboGenero;
        private Label label8;
        private ComboBox comboVoces;
        private Button btnPlay;
        private Button btnSaveChanges;
        private GroupBox groupBox2;
        private GroupBox groupBox9;
        private Button btnStop;
        private ComboBox comboBoxAmbiente;
        private Label label37;
        private Button btPlayAmbiente;
        private Label lblAlert;
        private PictureBox picAlert;
        private Panel panelAlert;
    }
}