namespace Pokemon_3D_Server_Client_GUI
{
    partial class ApplicationSettings
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
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.ObjectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.OlvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.OlvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.OlvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.TableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel1.ColumnCount = 2;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Controls.Add(this.OK_Button, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.Cancel_Button, 1, 0);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(915, 533);
            this.TableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 1;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(195, 36);
            this.TableLayoutPanel1.TabIndex = 1;
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OK_Button.Location = new System.Drawing.Point(4, 4);
            this.OK_Button.Margin = new System.Windows.Forms.Padding(4);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(89, 28);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "OK";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(101, 4);
            this.Cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(89, 28);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // ObjectListView1
            // 
            this.ObjectListView1.AllColumns.Add(this.OlvColumn1);
            this.ObjectListView1.AllColumns.Add(this.OlvColumn2);
            this.ObjectListView1.AllColumns.Add(this.OlvColumn3);
            this.ObjectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.ObjectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.OlvColumn1,
            this.OlvColumn2,
            this.OlvColumn3});
            this.ObjectListView1.FullRowSelect = true;
            this.ObjectListView1.GridLines = true;
            this.ObjectListView1.Location = new System.Drawing.Point(12, 12);
            this.ObjectListView1.MultiSelect = false;
            this.ObjectListView1.Name = "ObjectListView1";
            this.ObjectListView1.RowHeight = 100;
            this.ObjectListView1.SelectAllOnControlA = false;
            this.ObjectListView1.SelectColumnsOnRightClick = false;
            this.ObjectListView1.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.ObjectListView1.ShowItemToolTips = true;
            this.ObjectListView1.Size = new System.Drawing.Size(1099, 514);
            this.ObjectListView1.TabIndex = 2;
            this.ObjectListView1.UseCompatibleStateImageBehavior = false;
            this.ObjectListView1.View = System.Windows.Forms.View.Details;
            // 
            // OlvColumn1
            // 
            this.OlvColumn1.AspectName = "Setting";
            this.OlvColumn1.Groupable = false;
            this.OlvColumn1.IsEditable = false;
            this.OlvColumn1.Text = "Property";
            this.OlvColumn1.Width = 150;
            // 
            // OlvColumn2
            // 
            this.OlvColumn2.AspectName = "Value";
            this.OlvColumn2.Groupable = false;
            this.OlvColumn2.Hideable = false;
            this.OlvColumn2.Text = "Values";
            this.OlvColumn2.Width = 200;
            // 
            // OlvColumn3
            // 
            this.OlvColumn3.AspectName = "Remark";
            this.OlvColumn3.Groupable = false;
            this.OlvColumn3.Hideable = false;
            this.OlvColumn3.IsEditable = false;
            this.OlvColumn3.Text = "Remark";
            this.OlvColumn3.Width = 400;
            // 
            // ApplicationSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1123, 582);
            this.Controls.Add(this.ObjectListView1);
            this.Controls.Add(this.TableLayoutPanel1);
            this.Name = "ApplicationSettings";
            this.Text = "ApplicationSettings";
            this.Load += new System.EventHandler(this.ApplicationSettings_Load);
            this.TableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.Button OK_Button;
        internal System.Windows.Forms.Button Cancel_Button;
        internal BrightIdeasSoftware.ObjectListView ObjectListView1;
        internal BrightIdeasSoftware.OLVColumn OlvColumn1;
        internal BrightIdeasSoftware.OLVColumn OlvColumn2;
        internal BrightIdeasSoftware.OLVColumn OlvColumn3;
    }
}