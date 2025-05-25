namespace Nintai_kids
{
    partial class ExtraSelector
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
            flowPanelPersonajes = new FlowLayoutPanel();
            label7 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // flowPanelPersonajes
            // 
            flowPanelPersonajes.AutoScroll = true;
            flowPanelPersonajes.BackColor = Color.Silver;
            flowPanelPersonajes.Location = new Point(12, 64);
            flowPanelPersonajes.Name = "flowPanelPersonajes";
            flowPanelPersonajes.Size = new Size(1329, 270);
            flowPanelPersonajes.TabIndex = 19;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label7.ForeColor = Color.Navy;
            label7.Location = new Point(12, 9);
            label7.Name = "label7";
            label7.Size = new Size(295, 41);
            label7.TabIndex = 10;
            label7.Text = "Selecciona un asset:";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(427, 354);
            button1.Name = "button1";
            button1.Size = new Size(434, 49);
            button1.TabIndex = 0;
            button1.Text = "Terminar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ExtraSelector
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1353, 427);
            Controls.Add(button1);
            Controls.Add(label7);
            Controls.Add(flowPanelPersonajes);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExtraSelector";
            Text = "ExtraSelector";
            Load += ExtraSelector_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowPanelPersonajes;
        private Label label7;
        private Button button1;
    }
}