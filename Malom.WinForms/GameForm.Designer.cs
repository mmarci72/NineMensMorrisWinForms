namespace Malom.WinForms
{
    partial class GameForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSaveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLoadGame = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1031, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "File";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameMenuItem,
            this.toolStripSaveGame,
            this.toolStripLoadGame});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(46, 24);
            this.fileMenu.Text = "File";
            // 
            // newGameMenuItem
            // 
            this.newGameMenuItem.Name = "newGameMenuItem";
            this.newGameMenuItem.Size = new System.Drawing.Size(168, 26);
            this.newGameMenuItem.Text = "New Game";
            this.newGameMenuItem.Click += new System.EventHandler(this.NewGameClick);
            // 
            // toolStripSaveGame
            // 
            this.toolStripSaveGame.Name = "toolStripSaveGame";
            this.toolStripSaveGame.Size = new System.Drawing.Size(168, 26);
            this.toolStripSaveGame.Text = "Save Game";
            // 
            // toolStripLoadGame
            // 
            this.toolStripLoadGame.Name = "toolStripLoadGame";
            this.toolStripLoadGame.Size = new System.Drawing.Size(168, 26);
            this.toolStripLoadGame.Text = "Load Game";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Malom Table (*.stl)|*.stl";
            this.saveFileDialog.Title = "Save Game";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Game Table (*.stl)|*.stl";
            this.openFileDialog.Title = "Load Game";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1031, 590);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GameForm";
            this.Text = "Malom";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem newGameMenuItem;
        private ToolStripMenuItem toolStripSaveGame;
        private ToolStripMenuItem toolStripLoadGame;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
    }
}