namespace SGTest
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataImportButton = new Button();
            DataOutputButton = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // DataImportButton
            // 
            DataImportButton.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            DataImportButton.Location = new Point(300, 123);
            DataImportButton.Name = "DataImportButton";
            DataImportButton.Size = new Size(210, 40);
            DataImportButton.TabIndex = 0;
            DataImportButton.Text = "Импорт данных";
            DataImportButton.UseVisualStyleBackColor = true;
            DataImportButton.Click += this.DataImportButton_Click;
            // 
            // DataOutputButton
            // 
            DataOutputButton.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            DataOutputButton.Location = new Point(300, 201);
            DataOutputButton.Name = "DataOutputButton";
            DataOutputButton.Size = new Size(210, 40);
            DataOutputButton.TabIndex = 1;
            DataOutputButton.Text = "Вывод данных";
            DataOutputButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.Location = new Point(300, 272);
            button1.Name = "button1";
            button1.Size = new Size(210, 40);
            button1.TabIndex = 2;
            button1.Text = "Выход";
            button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(802, 453);
            Controls.Add(button1);
            Controls.Add(DataOutputButton);
            Controls.Add(DataImportButton);
            Name = "MainForm";
            Text = "Главная";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button DataImportButton;
        private Button DataOutputButton;
        private Button button1;
    }
}
