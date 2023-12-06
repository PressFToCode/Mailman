namespace Mailman
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBoxMap = new PictureBox();
            btnStart = new Button();
            textBoxCapacity = new TextBox();
            label1 = new Label();
            MoveTimer = new System.Windows.Forms.Timer(components);
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMap).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxMap
            // 
            pictureBoxMap.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxMap.Location = new Point(150, 49);
            pictureBoxMap.Name = "pictureBoxMap";
            pictureBoxMap.Size = new Size(632, 412);
            pictureBoxMap.TabIndex = 0;
            pictureBoxMap.TabStop = false;
            pictureBoxMap.Paint += pictureBoxMap_Paint;
            pictureBoxMap.MouseClick += pictureBox1_MouseClick;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(815, 49);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 1;
            btnStart.Text = "Старт";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // textBoxCapacity
            // 
            textBoxCapacity.Location = new Point(815, 135);
            textBoxCapacity.Name = "textBoxCapacity";
            textBoxCapacity.Size = new Size(125, 27);
            textBoxCapacity.TabIndex = 2;
            textBoxCapacity.TextChanged += textBoxCapacity_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(815, 98);
            label1.Name = "label1";
            label1.Size = new Size(100, 20);
            label1.TabIndex = 3;
            label1.Text = "Вместимость";
            // 
            // MoveTimer
            // 
            MoveTimer.Tick += MoveTimer_Tick;
            // 
            // button1
            // 
            button1.Location = new Point(815, 432);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 4;
            button1.Text = "Очистить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1030, 537);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(textBoxCapacity);
            Controls.Add(btnStart);
            Controls.Add(pictureBoxMap);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxMap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxMap;
        private Button btnStart;
        private TextBox textBoxCapacity;
        private Label label1;
        private System.Windows.Forms.Timer MoveTimer;
        private Button button1;
    }
}
