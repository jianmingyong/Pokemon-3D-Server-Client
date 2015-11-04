namespace Pokemon_3D_Server_Client_GUI
{
    partial class Downloader
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
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn6 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumn1);
            this.objectListView1.AllColumns.Add(this.olvColumn2);
            this.objectListView1.AllColumns.Add(this.olvColumn3);
            this.objectListView1.AllColumns.Add(this.olvColumn4);
            this.objectListView1.AllColumns.Add(this.olvColumn6);
            this.objectListView1.AllColumns.Add(this.olvColumn5);
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3,
            this.olvColumn4,
            this.olvColumn6,
            this.olvColumn5});
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.Location = new System.Drawing.Point(12, 12);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.Size = new System.Drawing.Size(1022, 527);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "ID";
            this.olvColumn1.AspectToStringFormat = "";
            this.olvColumn1.Groupable = false;
            this.olvColumn1.IsEditable = false;
            this.olvColumn1.Text = "ID";
            this.olvColumn1.Width = 50;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Name";
            this.olvColumn2.Text = "File Name";
            this.olvColumn2.Width = 300;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "CurrentBytes";
            this.olvColumn3.AspectToStringFormat = "";
            this.olvColumn3.Text = "Current Bytes";
            this.olvColumn3.Width = 100;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "TotalBytes";
            this.olvColumn4.AspectToStringFormat = "";
            this.olvColumn4.Text = "Total Bytes";
            this.olvColumn4.Width = 100;
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "Speed";
            this.olvColumn6.AspectToStringFormat = "";
            this.olvColumn6.Text = "Speed";
            this.olvColumn6.Width = 100;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "Status";
            this.olvColumn5.Text = "Status";
            this.olvColumn5.Width = 100;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(884, 545);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel All Download";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(728, 545);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 25);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel Download";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Downloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 582);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.objectListView1);
            this.Name = "Downloader";
            this.Text = "Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Downloader_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        private BrightIdeasSoftware.OLVColumn olvColumn6;
    }
}