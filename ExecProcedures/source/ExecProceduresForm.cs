// HDevEngine/.NET (C#) example for executing local and external HDevelop procedures
//
//© 2007-2017 MVTec Software GmbH
//
// Purpose:
// This example program shows how the classes HDevEngine, HDevProcedureCall,
// and HDevOpMultiWindowImpl can be used in order to implement a fin detection application.
// It uses the local and external procedures contained and referenced in the
// HDevelop program fin_detection.hdev, which can be found in the
// directory hdevelop.
// When you click the button Load, the HDevelop program is loaded, the other buttons
// execute procedures that initialize image acquisition, grab and process images,
// and visualize details, respectively. For the latter, the class HDevOpMultiWindowImpl 
// is used, which implements HDevelop's internal display operators.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using HalconDotNet;
using Automation.BDaq;

namespace ExecProcedures
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class ExecProceduresForm : System.Windows.Forms.Form
    {
        private IContainer components;

        // HDevEngine
        // instance of the engine
        private HDevEngine MyEngine = new HDevEngine();
        // path of HDevelop program
        String ProgramPathString;
        // procedure calls
        private HDevProcedureCall disCirProcCall;
        private HDevProcedureCall inspctDefProcCall;

        private HDevProcedureCall inspectDefectProcCall; 
        private HDevProcedureCall getRegionMasterProcCall;

        // implementation of the display operators
        private HDevOpMultiWindowImpl MyHDevOperatorImpl;
        // HALCON window
        private HWindow Window1;
        private HWindow Window2;
        private HWindow Window3;
        private HWindow Window4;
        // image acquisition device and image size
        HFramegrabber Framegrabber;
        // image and extracted region
        private HImage Image = new HImage();
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label3;
        private Label label2;
        private ComboBox comboBox1;
        private Label checkStatusLabel;
        private TableLayoutPanel tableLayoutPanel5;
        internal Label PartNolabel;
        private InstantDiCtrl testDi1;

        private InstantDoCtrl testDo1;
        HRegion CirclesRegion = new HRegion();
        private Boolean checkReset = false;
        private TableLayoutPanel tableLayoutPanel8;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel10;
        private Label label6;
        private Label label7;
        private Label label10;
        private Label label11;
        internal Label OKlabel;
        internal Label ngLabel;
        private Label label1;
        internal Label TotalLabel;
        private Label label13;
        private TableLayoutPanel tableLayoutPanel6;
        private HSmartWindowControl WindowControl2;
        private HSmartWindowControl WindowControl3;
        private HSmartWindowControl WindowControl4;
        private HSmartWindowControl WindowControl1;
        private ArrayList imgRefList = new ArrayList();
        private ArrayList regionsRefList = new ArrayList();
        private ArrayList regionsRotList = new ArrayList();
        private ArrayList imgList = new ArrayList();
        private ArrayList windowsList = new ArrayList();
        private Boolean ngStatus = false;
        private int bubbleItem;
        private int bwItem;
        private int okItem;
        private int ngItem;
        private Timer timer1;
        private int totalItem;
        private string PathPJ = Environment.CurrentDirectory;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem stopToolStripMenuItem;
        private ToolStripMenuItem resumeToolStripMenuItem;
        private DateTime date = new DateTime();



        public ExecProceduresForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecProceduresForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.checkStatusLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.OKlabel = new System.Windows.Forms.Label();
            this.ngLabel = new System.Windows.Forms.Label();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PartNolabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.WindowControl2 = new HalconDotNet.HSmartWindowControl();
            this.WindowControl3 = new HalconDotNet.HSmartWindowControl();
            this.WindowControl4 = new HalconDotNet.HSmartWindowControl();
            this.WindowControl1 = new HalconDotNet.HSmartWindowControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.testDi1 = new Automation.BDaq.InstantDiCtrl(this.components);
            this.testDo1 = new Automation.BDaq.InstantDoCtrl(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.92308F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.07692F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(788, 541);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.checkStatusLabel, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel8, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(608, 6);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(174, 529);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // checkStatusLabel
            // 
            this.checkStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkStatusLabel.AutoSize = true;
            this.checkStatusLabel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.checkStatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.checkStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkStatusLabel.ForeColor = System.Drawing.Color.White;
            this.checkStatusLabel.Location = new System.Drawing.Point(3, 402);
            this.checkStatusLabel.Name = "checkStatusLabel";
            this.checkStatusLabel.Size = new System.Drawing.Size(168, 93);
            this.checkStatusLabel.TabIndex = 11;
            this.checkStatusLabel.Text = "...";
            this.checkStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 108);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 258F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(168, 258);
            this.tableLayoutPanel8.TabIndex = 13;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel10);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(162, 252);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Total check";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 3;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel10.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.label6, 2, 1);
            this.tableLayoutPanel10.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.OKlabel, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.ngLabel, 1, 1);
            this.tableLayoutPanel10.Controls.Add(this.TotalLabel, 1, 2);
            this.tableLayoutPanel10.Controls.Add(this.label13, 2, 2);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 3;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(156, 227);
            this.tableLayoutPanel10.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 36);
            this.label1.TabIndex = 10;
            this.label1.Text = "Total :";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(111, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "pcs.";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(111, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "pcs.";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 18);
            this.label10.TabIndex = 1;
            this.label10.Text = "OK  :";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 18);
            this.label11.TabIndex = 2;
            this.label11.Text = "NG :";
            // 
            // OKlabel
            // 
            this.OKlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.OKlabel.AutoSize = true;
            this.OKlabel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.OKlabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.OKlabel.Location = new System.Drawing.Point(55, 23);
            this.OKlabel.Name = "OKlabel";
            this.OKlabel.Padding = new System.Windows.Forms.Padding(3);
            this.OKlabel.Size = new System.Drawing.Size(46, 28);
            this.OKlabel.TabIndex = 8;
            this.OKlabel.Text = "0";
            this.OKlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ngLabel
            // 
            this.ngLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ngLabel.AutoSize = true;
            this.ngLabel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ngLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ngLabel.Location = new System.Drawing.Point(55, 98);
            this.ngLabel.Name = "ngLabel";
            this.ngLabel.Padding = new System.Windows.Forms.Padding(3);
            this.ngLabel.Size = new System.Drawing.Size(46, 28);
            this.ngLabel.TabIndex = 9;
            this.ngLabel.Text = "0";
            this.ngLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalLabel
            // 
            this.TotalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalLabel.AutoSize = true;
            this.TotalLabel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.TotalLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TotalLabel.Location = new System.Drawing.Point(55, 174);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Padding = new System.Windows.Forms.Padding(3);
            this.TotalLabel.Size = new System.Drawing.Size(46, 28);
            this.TotalLabel.TabIndex = 11;
            this.TotalLabel.Text = "0";
            this.TotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(111, 178);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "pcs.";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel5.Controls.Add(this.comboBox1, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.PartNolabel, 1, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(168, 99);
            this.tableLayoutPanel5.TabIndex = 12;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "TEAA",
            "TLAA",
            "TGHA(coming soon)",
            "T3VA(coming soon)"});
            this.comboBox1.Location = new System.Drawing.Point(55, 10);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(5, 5, 15, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(98, 28);
            this.comboBox1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 36);
            this.label3.TabIndex = 0;
            this.label3.Text = "Part No.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 36);
            this.label2.TabIndex = 0;
            this.label2.Text = "Model";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PartNolabel
            // 
            this.PartNolabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PartNolabel.AutoSize = true;
            this.PartNolabel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.PartNolabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PartNolabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PartNolabel.Location = new System.Drawing.Point(55, 60);
            this.PartNolabel.Margin = new System.Windows.Forms.Padding(5, 0, 15, 0);
            this.PartNolabel.Name = "PartNolabel";
            this.PartNolabel.Padding = new System.Windows.Forms.Padding(3);
            this.PartNolabel.Size = new System.Drawing.Size(98, 28);
            this.PartNolabel.TabIndex = 9;
            this.PartNolabel.Text = "01";
            this.PartNolabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 529F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(593, 529);
            this.tableLayoutPanel4.TabIndex = 13;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.WindowControl2, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.WindowControl3, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.WindowControl4, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.WindowControl1, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(587, 523);
            this.tableLayoutPanel6.TabIndex = 15;
            // 
            // WindowControl2
            // 
            this.WindowControl2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl2.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowControl2.Enabled = false;
            this.WindowControl2.HDoubleClickToFitContent = true;
            this.WindowControl2.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl2.HImagePart = new System.Drawing.Rectangle(0, 0, 850, 565);
            this.WindowControl2.HKeepAspectRatio = true;
            this.WindowControl2.HMoveContent = false;
            this.WindowControl2.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl2.Location = new System.Drawing.Point(295, 2);
            this.WindowControl2.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl2.Name = "WindowControl2";
            this.WindowControl2.Size = new System.Drawing.Size(290, 257);
            this.WindowControl2.TabIndex = 14;
            this.WindowControl2.WindowSize = new System.Drawing.Size(290, 257);
            // 
            // WindowControl3
            // 
            this.WindowControl3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl3.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowControl3.Enabled = false;
            this.WindowControl3.HDoubleClickToFitContent = true;
            this.WindowControl3.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl3.HImagePart = new System.Drawing.Rectangle(0, 0, 850, 565);
            this.WindowControl3.HKeepAspectRatio = true;
            this.WindowControl3.HMoveContent = false;
            this.WindowControl3.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl3.Location = new System.Drawing.Point(2, 263);
            this.WindowControl3.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl3.Name = "WindowControl3";
            this.WindowControl3.Size = new System.Drawing.Size(289, 258);
            this.WindowControl3.TabIndex = 13;
            this.WindowControl3.WindowSize = new System.Drawing.Size(289, 258);
            // 
            // WindowControl4
            // 
            this.WindowControl4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl4.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowControl4.Enabled = false;
            this.WindowControl4.HDoubleClickToFitContent = true;
            this.WindowControl4.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl4.HImagePart = new System.Drawing.Rectangle(0, 0, 850, 565);
            this.WindowControl4.HKeepAspectRatio = true;
            this.WindowControl4.HMoveContent = false;
            this.WindowControl4.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl4.Location = new System.Drawing.Point(295, 263);
            this.WindowControl4.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl4.Name = "WindowControl4";
            this.WindowControl4.Size = new System.Drawing.Size(290, 258);
            this.WindowControl4.TabIndex = 12;
            this.WindowControl4.WindowSize = new System.Drawing.Size(290, 258);
            // 
            // WindowControl1
            // 
            this.WindowControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowControl1.Enabled = false;
            this.WindowControl1.HDoubleClickToFitContent = true;
            this.WindowControl1.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl1.HImagePart = new System.Drawing.Rectangle(0, 0, 850, 565);
            this.WindowControl1.HKeepAspectRatio = true;
            this.WindowControl1.HMoveContent = false;
            this.WindowControl1.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl1.Location = new System.Drawing.Point(2, 2);
            this.WindowControl1.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl1.Name = "WindowControl1";
            this.WindowControl1.Size = new System.Drawing.Size(289, 257);
            this.WindowControl1.TabIndex = 11;
            this.WindowControl1.WindowSize = new System.Drawing.Size(289, 257);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // testDi1
            // 
            this.testDi1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("testDi1._StateStream")));
            // 
            // testDo1
            // 
            this.testDo1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("testDo1._StateStream")));
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(788, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stopToolStripMenuItem,
            this.resumeToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // resumeToolStripMenuItem
            // 
            this.resumeToolStripMenuItem.Name = "resumeToolStripMenuItem";
            this.resumeToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.resumeToolStripMenuItem.Text = "Resume";
            this.resumeToolStripMenuItem.Click += new System.EventHandler(this.resumeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // ExecProceduresForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(788, 565);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ExecProceduresForm";
            this.Text = "CU-TNS Light Guide Inspection";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExecProceduresForm_FormClosing);
            this.Load += new System.EventHandler(this.ExecProceduresForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private const int input_port = 0;
        private const int output_port = 0;
        private const byte input_ch = 0x1;
        private const byte g_ch = 1;
        private const byte ng_ch = 2;
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.Run(new ExecProceduresForm());
        }

        private void ExecProceduresForm_Load(object sender, System.EventArgs e)
        {
            WindowControl_Load(sender, e);
            // path of external procedures
            PathPJ = PathPJ.Replace("bin\\Debug", "");
            string ProcedurePath = PathPJ + "\\hdev\\inspectDefect";
            ProgramPathString = PathPJ + "\\hdev\\test_black_dot.hdev";
            comboBox1.SelectedIndex = 0;
            if (!HalconAPI.isWindows)
            {
                // Unix-based systems (Mono)
                ProcedurePath = ProcedurePath.Replace('\\', '/');
                ProgramPathString = ProgramPathString.Replace('\\', '/');
            }
            try
            {
                Framegrabber = new HFramegrabber();
                /*
                Framegrabber.OpenFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
                  -1, "default", "default", "default", "C:/Users/Admin/Desktop/CU-TNS_LightGuide/ExecProcedures/vs2005/Picture/Ngtest/NotGood_shot1.tif", "default", -1, -1);
                */

                //Framegrabber.OpenFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
                //   -1, "default", "default", "default", "?", "default", -1, -1);

                /*
                Framegrabber.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1, 
                    "false", "default", "003053231ec3_basler_aca250014gm", 0, -1);
                Framegrabber.SetFramegrabberParam("AcquisitionMode","SingleFrame");
                */

                //Framegrabber.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb", -1, "false", "default", "[0] FaceTime HD Camera (Build-in)", 0, -1);


                //Framegrabber.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb", -1, "false", "default", "[0] USB2.0 HD UVC WebCam", 0, -1);

                //HTuple height = Framegrabber.GetFramegrabberParam("Height");
                //HTuple width = Framegrabber.GetFramegrabberParam("Width");

                //Window.SetPart(0, 0, Convert.ToInt32(height.ToString()), Convert.ToInt32(width.ToString()));
                testDi1 = new InstantDiCtrl();
                testDo1 = new InstantDoCtrl();
                testDo1.SelectedDevice = new DeviceInformation(0);
                testDi1.SelectedDevice = new DeviceInformation(0);
                //Console.WriteLine(testDi1.BoardVersion);
                testDo1.Write(0, 0x00);

            }
            catch (HDevEngineException Ex)
            {
                MessageBox.Show(Ex.Message, "HDevEngine Exception");
                return;
            }
            MyEngine.SetProcedurePath(ProcedurePath);
            LoadProcedures(sender, e);
            getRegionsImgRef();
        }

        private void LoadProcedures(object sender, System.EventArgs e)
        {
            // load program and access procedure calls
            try
            {
                //HDevProgram Program = new HDevProgram(ProgramPathString);

                //HDevProcedure InitAcqProc = new HDevProcedure(Program, "init_acq");
                //HDevProcedure inspctDefProc = new HDevProcedure("InspectDefect");
                //HDevProcedure disCirProc = new HDevProcedure(Program, "DisplayCircle");
                //InitAcqProcCall = new HDevProcedureCall(InitAcqProc);
                //inspctDefProcCall = new HDevProcedureCall(inspctDefProc);
                //disCirProcCall = new HDevProcedureCall(disCirProc);

                HDevProcedure getRegionMasterProc = new HDevProcedure("get_region_master");
                getRegionMasterProcCall = new HDevProcedureCall(getRegionMasterProc);
                HDevProcedure inspectDefectProc = new HDevProcedure("inspect_defect");
                inspectDefectProcCall = new HDevProcedureCall(inspectDefectProc);



            }
            catch (HDevEngineException Ex)
            {
                MessageBox.Show(Ex.Message, "HDevEngine Exception");
                return;
            }

        }

        private void WindowControl_Load(object sender, EventArgs e)
        {
            Window1 = WindowControl1.HalconWindow;
            Window2 = WindowControl2.HalconWindow;
            Window3 = WindowControl3.HalconWindow;
            Window4 = WindowControl4.HalconWindow;
            // initialize display
            Window1.SetDraw("margin");
            Window1.SetLineWidth(1);
            Window2.SetDraw("margin");
            Window2.SetLineWidth(1);
            Window3.SetDraw("margin");
            Window3.SetLineWidth(1);
            Window4.SetDraw("margin");
            Window4.SetLineWidth(1);
            // handler for display operators
            MyHDevOperatorImpl = new HDevOpMultiWindowImpl(Window1);

            windowsList.Add(Window1);
            windowsList.Add(Window2);
            windowsList.Add(Window3);
            windowsList.Add(Window4);
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            byte outputData = 0;
            ErrorCode err = ErrorCode.Success;
            for (int i = 0; i < 4; i++)
            {
                HWindow window = (HWindow)windowsList[i];
                window.ClearWindow();
                runProcess((HImage)imgList[i], window, i + 1);
            }
            if (ngStatus)
            {

                checkStatusLabel.Text = "NG";
                checkStatusLabel.BackColor = System.Drawing.Color.Red;

                outputData = ng_ch;
                ngItem++;
            }
            else
            {

                checkStatusLabel.Text = "OK";
                checkStatusLabel.BackColor = System.Drawing.Color.Green;

                outputData = g_ch;
                okItem++;
            }
            saveImage(imgList, ngStatus);
            totalItem++;
            updateData();

            err = testDo1.Write(output_port, (byte)outputData);
            if (err != ErrorCode.Success)
            {
                HandleError(err);
            }


            ngStatus = false;
        }

        private void runProcess(HImage Image, HWindow window, int shot)
        {
            window.ClearWindow();

            window.DispObj(Image);
            window.SetPart(0, 0, -2, -2);
            checkStatusLabel.Text = "";


            HObjectVector setOfcircle;
            try
            {
                // execute procedure
                /*
                inspctDefProcCall.SetInputIconicParamObject("ImageChk", Image);
                inspctDefProcCall.SetInputCtrlParamTuple("shot", shot);
                inspctDefProcCall.SetInputCtrlParamTuple("typeChk", "All");
                inspctDefProcCall.Execute();
                Image = inspctDefProcCall.GetOutputIconicParamImage("ImageChkRot");
                */
                inspectDefectProcCall.SetInputIconicParamObject("ImageChk", Image);
                inspectDefectProcCall.SetInputIconicParamObject("ImageRef", (HObject)imgRefList[shot - 1]);
                inspectDefectProcCall.SetInputIconicParamObject("ROI_Rot", (HObject)regionsRotList[shot-1]);
                inspectDefectProcCall.SetInputIconicParamObject("RegionsRef", (HObject)regionsRefList[shot - 1]);
                inspectDefectProcCall.Execute();
                HImage imgChk = Image;
                Image = inspectDefectProcCall.GetOutputIconicParamImage("ImageChkRot");
                // drew circle
                setOfcircle = inspectDefectProcCall.GetOutputIconicParamVector("setOfcircle");
                //int typeNG = inspctDefProcCall.GetOutputCtrlParamTuple("typeNG");
                //Console.WriteLine("TypeNG :" + typeNG);
                Image.DispObj(window);
                Console.WriteLine(setOfcircle.Length);
                if (setOfcircle.Length > 0)
                {
                    window.SetColor("red");
                    for (int i = 0; i < setOfcircle.Length; i++)
                    {
                        window.DispObj(setOfcircle[i].O);
                    }
                    ngStatus = true;
                    //((HImage)imgWindows).WriteImage("tiff", 0, PathPJ + "\\Picture\\Ng\\NgImg" + (DateTime.Now).ToFileTime());
                    imgChk.WriteImage("tiff",0,PathPJ + "\\Picture\\Ng\\NgImg" + (DateTime.Now).ToFileTime());
                }




                window.SetPart(0, 0, -2, -2);

                // get output parameters from procedure call
                /*Framegrabber = 
                    new HFramegrabber(InitAcqProcCall.GetOutputCtrlParamTuple("AcqHandle"));*/
            }
            catch (HDevEngineException Ex)
            {
                MessageBox.Show(Ex.Message, "HDevEngine Exception");
                return;
            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            // read Di port state
            byte inputData = 0;
            ErrorCode err = ErrorCode.Success;
            err = testDi1.Read(input_port, out inputData);

            if (err != ErrorCode.Success)
            {
                timer1.Enabled = false;
                HandleError(err);
                return;
            }
            else if (inputData == input_ch)
            {
                Console.WriteLine("Ready");
                timer1.Enabled = false;
                if (checkReset == true)
                {
                    resetParam();

                    checkReset = false;
                }
                Framegrabber.GrabImageStart(-1);
                testImageAllShot(imgList.Count);
                imgList.Add(Framegrabber.GrabImageAsync(-1));
                HWindow window = (HWindow)windowsList[imgList.Count - 1];

                window.DispObj((HImage)imgList[imgList.Count - 1]);
                //saveImage((HImage)imgList[imgList.Count - 1]);
                window.SetPart(0, 0, -2, -2);
                System.Threading.Thread.Sleep(1000);
                timer1.Enabled = true;
                if (imgList.Count == 4)
                {
                    runBtn_Click(sender, e);
                    checkReset = true;
                }

            }
            // do something.....
        }

        private void HandleError(ErrorCode err)
        {
            if ((err >= ErrorCode.ErrorHandleNotValid) && (err != ErrorCode.Success))
            {
                MessageBox.Show("Sorry ! Some errors happened, the error code is: " + err.ToString(), "Static DI");
            }
        }

        private void ExecProceduresForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            testDo1.Write(0, 0x00);
        }

        private void update_Status()
        {
            OKlabel.Text = "" + okItem;
            ngLabel.Text = "" + ngItem;
            TotalLabel.Text = "" + totalItem;
        }

        private void clearAllWindows()
        {
            for (int i = 0; i < 4; i++)
            {
                HWindow window = (HWindow)windowsList[i];
                window.ClearWindow();
            }
        }

        private void resetParam()
        {
            clearAllWindows();
            imgList = new ArrayList();
            checkStatusLabel.Text = "";
            checkStatusLabel.BackColor = System.Drawing.Color.White;
            testDo1.Write(0, 0x00);
        }


        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkStatusLabel.Text = "";
            checkStatusLabel.BackColor = System.Drawing.Color.White;
            bubbleItem = 0;
            bwItem = 0;
            okItem = 0;
            ngItem = 0;
            totalItem = 0;
            updateData();
        }

        private void updateData()
        {
            OKlabel.Text = "" + okItem;
            ngLabel.Text = "" + ngItem;
            TotalLabel.Text = "" + totalItem;
        }
        private void saveImage(ArrayList imgList, Boolean ngStatus)
        {
            PathPJ = PathPJ.Replace("bin\\Debug", "");
            //Console.WriteLine((DateTime.Now).ToFileTime());
            String PathG = PathPJ + ("\\Picture\\pic_good");
            String PathNG = PathPJ + ("\\Picture\\pic_notgood");
            if(ngStatus)
            {
                for(int i = 0; i <= 3; i++)
                {
                    ((HImage)imgList[i]).WriteImage("tiff", 0, PathNG + "\\NgImg" + (DateTime.Now).ToFileTime());
                }
            }
            else
            {
                for (int i = 0; i <= 3; i++)
                {
                    ((HImage)imgList[i]).WriteImage("tiff", 0, PathG + "\\GImg" + (DateTime.Now).ToFileTime());
                }
            }
        }

        private void testImageAllShot(int i)
        {
            if (i == 0)
            {
                Framegrabber.OpenFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
                  -1, "default", "default", "default", "../../Picture/Ngtest/NotGood_shot1.tif", "default", -1, -1);
            }
            else if (i == 1)
            {
                Framegrabber.OpenFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
                  -1, "default", "default", "default", "../../Picture/Ngtest/NotGood_shot2.tif", "default", -1, -1);
            }
            else if (i == 2)
            {
                Framegrabber.OpenFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
                  -1, "default", "default", "default", "../../Picture/Ngtest/NotGood_shot3.tif", "default", -1, -1);
            }
            else
            {
                Framegrabber.OpenFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
                  -1, "default", "default", "default", "../../Picture/Ngtest/NotGood_shot4.tif", "default", -1, -1);
            }
        }

        private void getRegionsImgRef()
        {
            HImage imgRef = new HImage();
            HRegion regionsRef = new HRegion();
            HRegion regionsRot = new HRegion();
            imgRefList = new ArrayList();
            regionsRefList = new ArrayList();
            regionsRotList = new ArrayList();
            for (int i = 0; i<4; i++)
            {
                getRegionMasterProcCall.SetInputCtrlParamTuple("shot", i + 1);
                getRegionMasterProcCall.Execute();
                imgRef = getRegionMasterProcCall.GetOutputIconicParamImage("ImageRef");
                regionsRef = getRegionMasterProcCall.GetOutputIconicParamRegion("RegionsRef");
                regionsRot = getRegionMasterProcCall.GetOutputIconicParamRegion("ROI_Rot");
                imgRefList.Add(imgRef);
                regionsRefList.Add(regionsRef);
                regionsRotList.Add(regionsRot);
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}