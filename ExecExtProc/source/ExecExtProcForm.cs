// HDevEngine/.NET (C#) example for executing external 
// HDevelop procedures
//
// © 2007-2017 MVTec Software GmbH
//
// Purpose:
// This example program shows how the classes HDevEngine, HDevProcedureCall,
// and HDevOpMultiWindowImpl can be used in order to implement a fin detection
// application. Most of the application's functionality is contained in the
// HDevelop external procedure detect_fin(), which can be found in the
// procedures directory. The procedure takes an image as input object parameter
// and returns a region and its area as output parameters.
// When you click the button Load, the HDevelop procedure is loaded;
// when you click Execute, the following steps are executed:
// 1. An image is grabbed and passed as input parameter to the procedure call.
// 2. The procedure call is executed.
// 3. The output parameters of the procedure call are retrieved and
//    displayed in the graphics window.
// These steps are executed repeatedly for different images. The class
// HDevOpMultiWindowImpl implements HDevelop's internal operators.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using HalconDotNet;
using System.Threading;

namespace ExecExtProc
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class ExecExtProcForm : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.Button LoadBtn;
        internal System.Windows.Forms.Button StartBtn;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        // HDevEngine
        // instance of the engine
        private HDevEngine MyEngine = new HDevEngine();
        // implementation of the display operators
        // private HDevOpMultiWindowImpl MyHDevOperatorImpl;

        // procedure call
        private HDevProcedureCall ProcCall;
        private HSmartWindowControl WindowControl;
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBox shotComboBox;
        private string PathPJ = Environment.CurrentDirectory;
        String ProgramPathString;
        internal Button StopBtn;
        private TableLayoutPanel tableLayoutPanel2;
        private Button Savebtn;
        private HFramegrabber Framegrabber;

        // HALCON window
        private HWindow Window;

        public ExecExtProcForm()
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
            this.LoadBtn = new System.Windows.Forms.Button();
            this.StartBtn = new System.Windows.Forms.Button();
            this.WindowControl = new HalconDotNet.HSmartWindowControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.shotComboBox = new System.Windows.Forms.ComboBox();
            this.StopBtn = new System.Windows.Forms.Button();
            this.Savebtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoadBtn
            // 
            this.LoadBtn.Location = new System.Drawing.Point(3, 103);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(120, 40);
            this.LoadBtn.TabIndex = 3;
            this.LoadBtn.Text = "Load Procedure";
            this.LoadBtn.Visible = false;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(3, 303);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(138, 44);
            this.StartBtn.TabIndex = 6;
            this.StartBtn.Text = "Start";
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // WindowControl
            // 
            this.WindowControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowControl.HDoubleClickToFitContent = true;
            this.WindowControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl.HImagePart = new System.Drawing.Rectangle(0, 0, 768, 576);
            this.WindowControl.HKeepAspectRatio = true;
            this.WindowControl.HMoveContent = true;
            this.WindowControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl.Location = new System.Drawing.Point(2, 2);
            this.WindowControl.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl.Name = "WindowControl";
            this.WindowControl.Size = new System.Drawing.Size(598, 411);
            this.WindowControl.TabIndex = 7;
            this.WindowControl.WindowSize = new System.Drawing.Size(598, 411);
            this.WindowControl.Load += new System.EventHandler(this.WindowControl_Load);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Controls.Add(this.WindowControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(752, 415);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.LoadBtn, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.shotComboBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.StopBtn, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.StartBtn, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.Savebtn, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(605, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(144, 409);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // shotComboBox
            // 
            this.shotComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shotComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shotComboBox.FormattingEnabled = true;
            this.shotComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.shotComboBox.Location = new System.Drawing.Point(3, 53);
            this.shotComboBox.Name = "shotComboBox";
            this.shotComboBox.Size = new System.Drawing.Size(138, 33);
            this.shotComboBox.TabIndex = 8;
            // 
            // StopBtn
            // 
            this.StopBtn.Enabled = false;
            this.StopBtn.Location = new System.Drawing.Point(3, 353);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(138, 45);
            this.StopBtn.TabIndex = 9;
            this.StopBtn.Text = "Stop";
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // Savebtn
            // 
            this.Savebtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Savebtn.Enabled = false;
            this.Savebtn.Location = new System.Drawing.Point(3, 203);
            this.Savebtn.Name = "Savebtn";
            this.Savebtn.Size = new System.Drawing.Size(138, 44);
            this.Savebtn.TabIndex = 10;
            this.Savebtn.Text = "Save Master";
            this.Savebtn.UseVisualStyleBackColor = true;
            this.Savebtn.Click += new System.EventHandler(this.Savebtn_Click);
            // 
            // ExecExtProcForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(752, 415);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ExecExtProcForm";
            this.Text = "Execute External HDevelop Procedures via HDevEngine";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExecExtProcForm_FormClosing);
            this.Load += new System.EventHandler(this.ExecExtProcForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new ExecExtProcForm());
        }

        private void ExecExtProcForm_Load(object sender, System.EventArgs e)
        {
            // path of external procedures
            PathPJ = PathPJ.Replace("bin\\Debug", "");
            string ProcedurePath = PathPJ + "\\hdev\\teaching_hdev";
            ProgramPathString = PathPJ + "\\hdev\\test_teaching.hdev";
            shotComboBox.SelectedIndex = 0;
            if (!HalconAPI.isWindows)
            {
                // Unix-based systems (Mono)
                ProcedurePath = ProcedurePath.Replace('\\', '/');
            }
            MyEngine.SetProcedurePath(ProcedurePath);

            // disable Execute button
            StartBtn.Enabled = false;
            LoadBtn_Click(sender, e);
        }

        private void LoadBtn_Click(object sender, System.EventArgs e)
        {
            try
            {
                HDevProcedure Procedure = new HDevProcedure("Calibrate");
                ProcCall = new HDevProcedureCall(Procedure);
            }
            catch (HDevEngineException Ex)
            {
                MessageBox.Show(Ex.Message, "HDevEngine Exception");
                return;
            }


            // enable Execute button
            LoadBtn.Enabled = false;
            StartBtn.Enabled = true;

        }


        private void StartBtn_Click(object sender, System.EventArgs e)
        {


            Framegrabber = new HFramegrabber();
            // read images and process them
            try
            {

                //Framegrabber.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb", -1, "false", "default", "[0] USB2.0 HD UVC WebCam", 0, -1);

                
                Framegrabber.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1, 
                    "false", "default", "003053231ec3_basler_aca250014gm", 0, -1);
                Framegrabber.SetFramegrabberParam("AcquisitionMode","SingleFrame");
                HImage Image = new HImage();
                HRegion CirclesRegion;

                int shot;
                shot = Int32.Parse(shotComboBox.SelectedItem+"");

                // execute procedure
                ProcCall.SetInputCtrlParamTuple("shot", shot);
                ProcCall.Execute();
                // get output parameters from procedure call
                CirclesRegion = ProcCall.GetOutputIconicParamRegion("RegionOfInterest");
                //LightCheck = ProcCall.GetOutputIconicParamRegion("RegionOfInterest");



                StartBtn.Enabled = false;
                shotComboBox.Enabled = false;
                Savebtn.Enabled = true;
                StopBtn.Enabled = true;
                while (true)
                {


                    if (!StopBtn.Enabled)  
                    {
                        StartBtn.Enabled = true;
                        StopBtn.Enabled = false;
                        Framegrabber.Dispose();
                        Image.Dispose();
                        return;
                    }
                    //Framegrabber.GrabImageStart(-1);
                    Image = Framegrabber.GrabImageAsync(-1);
                    
                    // display results
                    Image.DispObj(Window);
                    Window.SetColor("red");
                    Window.DispObj(CirclesRegion);
                    Window.SetPart(0, 0, -2, -2);

                    HSystem.WaitSeconds(0.108);
                }
            }
            catch (HOperatorException Ex)
            {
                MessageBox.Show(Ex.Message, "HALCON Exception");
            }
            catch (HDevEngineException Ex)
            {
                MessageBox.Show(Ex.Message, "HDevEngine Exception");
            }

            Framegrabber.Dispose();
            
        }

        private void WindowControl_Load(object sender, EventArgs e)
        {
            Window = WindowControl.HalconWindow;
            // initialize display
            Window.SetDraw("margin");
            Window.SetLineWidth(2);

            // handler for display operators
            // to use handler, uncomment the following lines
            // MyHDevOperatorImpl = new HDevOpMultiWindowImpl(Window);
            // MyEngine.SetHDevOperators(MyHDevOperatorImpl);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            StopBtn.Enabled = false;
            shotComboBox.Enabled = true;
            Savebtn.Enabled = false;
            Framegrabber.Dispose();
        }

        private void LiveCam_thread()
        {

        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            HImage refImg = new HImage();
            string PathPJ = Environment.CurrentDirectory;
            PathPJ = PathPJ.Replace("bin\\Debug", "");
            string PathProgram = "C:\\Users\\Advantech\\Desktop\\CU-TNS_LightGuide\\ExecProcedures\\vs2005\\Picture\\PicRef\\";
            Framegrabber.GrabImageStart(-1);
            refImg = Framegrabber.GrabImageAsync(-1);
            refImg.WriteImage("tiff", 0, PathProgram + "Good_Shot" + shotComboBox.SelectedItem);
            refImg.Dispose();
            StopBtn_Click(sender, e);
        }

        private void ExecExtProcForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Framegrabber.Dispose();
        }
    }
}
