namespace VirtualGrid.WinFormsDemo
{
    partial class MainMenuForm
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
            this._tinyCounterExampleButton = new System.Windows.Forms.Button();
            this._todoListExampleButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _tinyCounterExampleButton
            // 
            this._tinyCounterExampleButton.Location = new System.Drawing.Point(13, 13);
            this._tinyCounterExampleButton.Name = "_tinyCounterExampleButton";
            this._tinyCounterExampleButton.Size = new System.Drawing.Size(250, 25);
            this._tinyCounterExampleButton.TabIndex = 0;
            this._tinyCounterExampleButton.Text = "小さいカウンタ (TinyCounterForm)";
            this._tinyCounterExampleButton.UseVisualStyleBackColor = true;
            this._tinyCounterExampleButton.Click += new System.EventHandler(this._tinyCounterExampleButton_Click);
            // 
            // _todoListExampleButton
            // 
            this._todoListExampleButton.Location = new System.Drawing.Point(13, 44);
            this._todoListExampleButton.Name = "_todoListExampleButton";
            this._todoListExampleButton.Size = new System.Drawing.Size(250, 25);
            this._todoListExampleButton.TabIndex = 1;
            this._todoListExampleButton.Text = "TODO リスト (TodoListForm)";
            this._todoListExampleButton.UseVisualStyleBackColor = true;
            this._todoListExampleButton.Click += new System.EventHandler(this._todoListExampleButton_Click);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._todoListExampleButton);
            this.Controls.Add(this._tinyCounterExampleButton);
            this.Name = "MainMenuForm";
            this.Text = "MainMenuForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _tinyCounterExampleButton;
        private System.Windows.Forms.Button _todoListExampleButton;
    }
}