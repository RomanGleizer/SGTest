namespace SGTest.Forms
{
    partial class OutputForm
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
            OutputTypeDropdown = new ComboBox();
            OutputTypeText = new Label();
            OutputDataFromDatabaseButton = new Button();
            SuspendLayout();
            // 
            // OutputTypeDropdown
            // 
            OutputTypeDropdown.FormattingEnabled = true;
            OutputTypeDropdown.Location = new Point(301, 176);
            OutputTypeDropdown.Name = "OutputTypeDropdown";
            OutputTypeDropdown.Size = new Size(192, 28);
            OutputTypeDropdown.TabIndex = 7;
            OutputTypeDropdown.SelectedIndexChanged += OutputTypeDropdown_SelectedIndexChanged;
            // 
            // OutputTypeText
            // 
            OutputTypeText.AutoSize = true;
            OutputTypeText.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            OutputTypeText.Location = new Point(316, 126);
            OutputTypeText.Name = "OutputTypeText";
            OutputTypeText.Size = new Size(163, 34);
            OutputTypeText.TabIndex = 6;
            OutputTypeText.Text = "Тип вывода";
            // 
            // OutputDataFromDatabaseButton
            // 
            OutputDataFromDatabaseButton.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            OutputDataFromDatabaseButton.ForeColor = Color.Black;
            OutputDataFromDatabaseButton.Location = new Point(301, 240);
            OutputDataFromDatabaseButton.Name = "OutputDataFromDatabaseButton";
            OutputDataFromDatabaseButton.Size = new Size(192, 43);
            OutputDataFromDatabaseButton.TabIndex = 8;
            OutputDataFromDatabaseButton.Text = "Вывести данные";
            OutputDataFromDatabaseButton.UseVisualStyleBackColor = true;
            OutputDataFromDatabaseButton.Click += OutputDataFromDatabaseButton_Click;
            // 
            // OutputForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(OutputDataFromDatabaseButton);
            Controls.Add(OutputTypeDropdown);
            Controls.Add(OutputTypeText);
            Name = "OutputForm";
            Text = "Вывод";
            Load += OutputForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox OutputTypeDropdown;
        private Label OutputTypeText;
        private Button OutputDataFromDatabaseButton;
    }
}