namespace SGTest.Forms
{
    partial class ImportDataForm
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
            ImportTypeText = new Label();
            ImportTypeDropdown = new ComboBox();
            ImportDataToDatabaseButton = new Button();
            SelectFileButton = new Button();
            FilePathTextBox = new TextBox();
            SuspendLayout();
            // 
            // ImportTypeText
            // 
            ImportTypeText.AutoSize = true;
            ImportTypeText.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            ImportTypeText.Location = new Point(324, 203);
            ImportTypeText.Name = "ImportTypeText";
            ImportTypeText.Size = new Size(180, 34);
            ImportTypeText.TabIndex = 3;
            ImportTypeText.Text = "Тип импорта";
            ImportTypeText.Click += ImportTypeText_Click;
            // 
            // ImportTypeDropdown
            // 
            ImportTypeDropdown.FormattingEnabled = true;
            ImportTypeDropdown.Location = new Point(318, 254);
            ImportTypeDropdown.Name = "ImportTypeDropdown";
            ImportTypeDropdown.Size = new Size(192, 28);
            ImportTypeDropdown.TabIndex = 5;
            ImportTypeDropdown.SelectedIndexChanged += ImportTypeDropdown_SelectedIndexChanged;
            // 
            // ImportDataToDatabaseButton
            // 
            ImportDataToDatabaseButton.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            ImportDataToDatabaseButton.ForeColor = Color.Black;
            ImportDataToDatabaseButton.Location = new Point(318, 367);
            ImportDataToDatabaseButton.Name = "ImportDataToDatabaseButton";
            ImportDataToDatabaseButton.Size = new Size(192, 43);
            ImportDataToDatabaseButton.TabIndex = 7;
            ImportDataToDatabaseButton.Text = "Импортировать";
            ImportDataToDatabaseButton.UseVisualStyleBackColor = true;
            ImportDataToDatabaseButton.Click += ImportDataToDatabaseButton_Click;
            // 
            // SelectFileButton
            // 
            SelectFileButton.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            SelectFileButton.Location = new Point(318, 43);
            SelectFileButton.Name = "SelectFileButton";
            SelectFileButton.Size = new Size(192, 41);
            SelectFileButton.TabIndex = 8;
            SelectFileButton.Text = "Выберите файл";
            SelectFileButton.UseVisualStyleBackColor = true;
            SelectFileButton.Click += SelectFileButton_Click;
            // 
            // FilePathTextBox
            // 
            FilePathTextBox.BackColor = Color.White;
            FilePathTextBox.Location = new Point(318, 109);
            FilePathTextBox.Name = "FilePathTextBox";
            FilePathTextBox.Size = new Size(192, 27);
            FilePathTextBox.TabIndex = 9;
            // 
            // ImportDataForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(FilePathTextBox);
            Controls.Add(SelectFileButton);
            Controls.Add(ImportDataToDatabaseButton);
            Controls.Add(ImportTypeDropdown);
            Controls.Add(ImportTypeText);
            Name = "ImportDataForm";
            Text = "Импорт данных";
            Load += ImportDataForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label ImportTypeText;
        private ComboBox ImportTypeDropdown;
        private Button ImportDataToDatabaseButton;
        private Button SelectFileButton;
        private TextBox FilePathTextBox;
    }
}