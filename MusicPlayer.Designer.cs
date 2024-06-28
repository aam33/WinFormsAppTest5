
namespace WinFormsAppTest5;

partial class MusicPlayer
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
        button1 = new Button();
        dataGridView1 = new DataGridView();
        dataGridView2 = new DataGridView();
        button2 = new Button();
        textBox1 = new TextBox();
        pictureBox1 = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // button1
        // 
        button1.BackColor = SystemColors.ActiveCaption;
        button1.Location = new Point(307, 12);
        button1.Name = "button1";
        button1.Size = new Size(193, 34);
        button1.TabIndex = 0;
        button1.Text = "Load Albums";
        button1.UseVisualStyleBackColor = false;
        button1.Click += button1_Click;
        // 
        // dataGridView1
        // 
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Location = new Point(307, 52);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.RowHeadersWidth = 62;
        dataGridView1.RowTemplate.Height = 33;
        dataGridView1.Size = new Size(891, 185);
        dataGridView1.TabIndex = 1;
        dataGridView1.CellClick += dataGridView1_CellClick;
        // 
        // dataGridView2
        // 
        dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView2.Location = new Point(307, 243);
        dataGridView2.Name = "dataGridView2";
        dataGridView2.RowHeadersWidth = 62;
        dataGridView2.RowTemplate.Height = 33;
        dataGridView2.Size = new Size(891, 238);
        dataGridView2.TabIndex = 2;
        dataGridView2.CellClick += dataGridView2_CellClick;
        // 
        // button2
        // 
        button2.BackColor = SystemColors.ActiveCaption;
        button2.Location = new Point(1086, 12);
        button2.Name = "button2";
        button2.Size = new Size(112, 34);
        button2.TabIndex = 3;
        button2.Text = "Search";
        button2.UseVisualStyleBackColor = false;
        button2.Click += button2_Click;
        // 
        // textBox1
        // 
        textBox1.Location = new Point(799, 14);
        textBox1.Name = "textBox1";
        textBox1.PlaceholderText = "Search";
        textBox1.Size = new Size(281, 31);
        textBox1.TabIndex = 4;
        // 
        // pictureBox1
        // 
        pictureBox1.Location = new Point(12, 14);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(257, 223);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 5;
        pictureBox1.TabStop = false;
        // 
        // MusicPlayer
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1210, 493);
        Controls.Add(pictureBox1);
        Controls.Add(textBox1);
        Controls.Add(button2);
        Controls.Add(dataGridView2);
        Controls.Add(dataGridView1);
        Controls.Add(button1);
        Name = "MusicPlayer";
        Text = "MusicPlayer";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button button1;
    private DataGridView dataGridView1;
    private DataGridView dataGridView2;
    private Button button2;
    private TextBox textBox1;
    private PictureBox pictureBox1;
}
