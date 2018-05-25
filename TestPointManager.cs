/*///////////////////////////////////
Copyright BAE SYSTEMS 2017
//////////////////////////////////*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using LibUutComs;
using BaeSystems.TestStations.Databases;
using BaeSystems.TestStations.Properties;
using TabPageExtensions;
using UutComsControls;

namespace BaeSystems.TestStations
{
    public partial class TestPointManager : Form, IConfigurable
    {
        /* Properties */

        // List of all available testpoints
        [ImportMany(typeof(ITestPoint), AllowRecomposition = true)]
        List<ITestPoint> allTestPoints { get; set; }
        
        // List of all log options
        [ImportMany(typeof(ILogger))]
        List<ILogger> loggers { get; set; }
        
        /// <summary>
        /// String containing the full path to the TMS log file.
        /// </summary>
        //public static string LogFileName { get; set; }


        /* Class Variables */

        // - TODO - use MEF to find / select the database
        // right now, only paperless is supported, so don't bother
        IDatabase paperlessDb;

        // token for task cancellation
        CancellationTokenSource requestTestCancel;

        ITestPoint previousTp = null;
        bool portTabsActive = false;
        // variable for tracking if the testpoint controls are hidden
        bool controlsHidden = false;

        // Checks if form has an update
        Progress<StatusInfo> progressHandler;

        ConfigManagerXml configManager;

        static bool verifyTestPointVersion = false;
        static bool verifyEquipmentInCal = false;
        static bool clearOperatorId = false;
        static bool clearSerialNumber = true;
        bool errorSelectingIndex = false;
        
        public void Configure(IConfigManager configManager)
        {
            loggerSelect.Text = configManager.GetValue(ConfigFile.TEST_STATION, "/General/Logging", "Excel");
            Boolean.TryParse(configManager.GetValue(ConfigFile.TEST_STATION, "/General/VerifyTpVersion", "False"), out verifyTestPointVersion);
            Boolean.TryParse(configManager.GetValue(ConfigFile.TEST_STATION, "/General/VerifyInCal", "False"), out verifyEquipmentInCal);
            Boolean.TryParse(configManager.GetValue(ConfigFile.TEST_STATION, "/General/ClearOperator", "False"), out clearOperatorId);
            Boolean.TryParse(configManager.GetValue(ConfigFile.TEST_STATION, "/General/ClearSerialNum", "False"), out clearSerialNumber);

            bool value = false;
            Boolean.TryParse(configManager.GetValue(ConfigFile.TEST_STATION, "/General/TestMode", "False"), out value);
            testMode.Checked = value;
            Boolean.TryParse(configManager.GetValue(ConfigFile.TEST_STATION, "/General/EngMode", "False"), out value);
            engineeringMode.Checked = value;
            Boolean.TryParse(configManager.GetValue(ConfigFile.TEST_STATION, "/General/NoDB", "False"), out value);
            noDatabase.Checked = value;

            configManager.IgnoreStartupError = false;
        }

        public void UpdateConfiguration(string[] newValues)
        {
            if (newValues.Length > 0) loggerSelect.Text = newValues[0];
            if (newValues.Length > 1) Boolean.TryParse(newValues[1], out verifyTestPointVersion);
            if (newValues.Length > 2) Boolean.TryParse(newValues[2], out verifyEquipmentInCal);
            if (newValues.Length > 3) Boolean.TryParse(newValues[3], out clearOperatorId);
            if (newValues.Length > 4) Boolean.TryParse(newValues[4], out clearSerialNumber);

            bool value = false;
            if (newValues.Length > 5) Boolean.TryParse(newValues[5], out value);
            testMode.Checked = value;
        }

        private string titleText;

        Dictionary<string, UutCompact> portControls = new Dictionary<string, UutCompact>();

        //-----------------------------------------------------------------
        public TestPointManager()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            // Add the plugin directory(s) to the catalog
            List<string> dirList = Settings.Default.PluginDir.Cast<string>().ToList();
            foreach (var dir in dirList)
            {
                DirectoryCatalog pluginDir = new DirectoryCatalog(dir);
                catalog.Catalogs.Add(pluginDir);
            }
            CompositionContainer container = new CompositionContainer(catalog);
            try
            {
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("LoaderException"))
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    if (typeLoadException.LoaderExceptions[0].ToString().Contains("NationalInstruments.Common."))
                    {
                        MessageBox.Show("Driver is missing. Try installing this:\r\n" +
                                         @"S:\SHARED\IR\Engineering\Software\COTS\NI Drivers\NI Vision_2015",
                                         "Error loading the TMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    MessageBox.Show(typeLoadException.LoaderExceptions[0].ToString(),
                        "Error loading the TMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.ToString(), "Error loading the TMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Environment.Exit(0);
            }

            InitializeComponent();

            titleText = this.ProductName + ", " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = titleText;

            // Populate the logger menu
            if (loggers.Count > 0)
            {
                loggerSelect.ComboBox.BindingContext = this.BindingContext;
                loggerSelect.ComboBox.DataSource = new BindingSource(loggers, null);
                loggerSelect.ComboBox.DisplayMember = "DisplayName";
            }

            // Create the progress reporting object
            progressHandler = new Progress<StatusInfo>();
            progressHandler.ProgressChanged += ProgressHandler_UpdateGuiState;

            // Set the initial GUI configuration
            configManager = new ConfigManagerXml(Settings.Default.ConfigDir, progressHandler);

            // Setup the database connection
            paperlessDb = new PaperlessDBFile();
        }

        //-----------------------------------------------------------------
        // Read the configuration files
        private async Task InitialConfig()
        {
            this.Configure(configManager);
            foreach (var tp in allTestPoints)
            {
                configManager.SetTestPoint(tp.TestPointName);
                await Task.Run(() =>
                {
                    IConfigurable configurableObject = tp as IConfigurable;
                    if (configurableObject != null)
                    {
                        configurableObject.Configure(configManager);
                    }
                    if (tp.Tasks != null)
                    {
                        foreach (var tt in tp.Tasks)
                        {
                            configurableObject = tt as IConfigurable;
                            if (configurableObject != null)
                            {
                                configurableObject.Configure(configManager);
                            }
                            tt.StatusHandle = progressHandler;
                        }
                    }
                });
            }

            if(engineeringMode.Checked)
            {
                string id = ConfigurationManager.AppSettings["TestID"];
                if(!string.IsNullOrEmpty(id))
                {
                    operatorId.Text = id;
                }
                string sn = ConfigurationManager.AppSettings["TestSN"];
                if(!string.IsNullOrEmpty(sn))
                {
                    uutSerialNumber.Text = sn;
                }
            }
            operatorId.BackColor = Color.Yellow;
        }

        //-----------------------------------------------------------------
        // When the GUI loads, start checking for updates
        private async void TestPointManager_Load(object sender, EventArgs e)
        {
            logWindow.UpdateSize();

            await InitialConfig();

            uutSerialNumber.Enabled = true;
        }

        //-----------------------------------------------------------------
        // If the user tries to close the form while a test is in progress, ask if they are sure
        private async void TestPointManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (runTestPoint.Text.Contains("Cancel"))
            {
                DialogResult dr = MessageBox.Show("Test in progress!\nAre you sure?", "Test In Progress", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.Yes)
                {
                    if (requestTestCancel != null)
                    {
                        requestTestCancel.Cancel();
                    }
                    // Close the previous ports if there were any
                    if (previousTp != null)
                    {
                        IHoldResources prevTask = previousTp as IHoldResources;
                        if (prevTask != null)
                        {
                            await Task.Run(() =>
                            {
                                prevTask.UnSelect();
                            });
                        }
                        IHazAPort prevPort = previousTp as IHazAPort;
                        if (prevPort != null)
                        {
                            int i = tabControl1.TabPages.Count - 1;
                            foreach (var port in prevPort.Ports)
                            {
                                port.Cancel();
                            }
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }


        }

        private async void uutSerialNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == '\r') && (!string.IsNullOrEmpty(uutSerialNumber.Text)))
            {
                // Populate the testpoint menu
                if (allTestPoints.Count > 0)
                {
                    List<ITestPoint> tpList;
                    if (noDatabase.Checked)
                    {
                        tpList = allTestPoints;
                    }
                    else
                    {
                        List<int> allowed = new List<int>();
                        AllowFormControls(false);
                        await Task.Run(() =>
                        {
                            allowed = paperlessDb.GetAllowedTestPoints(uutSerialNumber.Text);   //Get available list of testpoints for this serial
                            if (allowed == null)
                            {
                                MessageBox.Show("This serial number is not at a testpoint. Check the serial number\r\n" +
                                                "or disposition it in Paperless.",
                                                "Serial Number Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                allowed = allowed.Distinct().ToList();  //Trim out duplicated testpoints
                            }
                        });
                        AllowFormControls(true);

                        /* Break out if there are no testpoints available */
                        if (allowed == null)
                        {
                            return;
                        }

                        /* Only show testpoints in the dropdown that are both available on this unit's router
                         * and are in the executable folder as a .dll */
                        List<ITestPoint> intersection = new List<ITestPoint>();
                        foreach (ITestPoint itp in allTestPoints)
                        {
                            if (itp.Id.Intersect(allowed).Count() > 0)
                            {
                                intersection.Add(itp);
                            }
                        }
                        tpList = intersection;

                    }
                    if (tpList.Count > 0)
                    {
                        uutSerialNumber.BackColor = default(Color);
                        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/ca1ffbce-0452-4f0a-9b84-e080e3aa51f8/how-do-i-stop-the-selectedindexchanged-event-from-firing-twice?forum=csharpgeneral
                        testPointSelect.SelectedIndexChanged -= testPointSelect_SelectedIndexChanged;
                        testPointSelect.DataSource = new BindingSource(tpList, null);
                        testPointSelect.DisplayMember = "DisplayName";
                        // If only one testpoint is available, go ahead and select it
                        if (tpList.Count == 1)
                        {
                            testPointSelect.SelectedIndex = 0;
                            runTestPoint.BackColor = Color.Yellow;
                        }
                        // Otherwise, make the operator choose
                        else
                        {
                            testPointSelect.SelectedItem = null;
                            testPointInfo.ForeColor = Color.Coral;
                        }
                        testPointSelect.SelectedIndexChanged += testPointSelect_SelectedIndexChanged;
                        if(tpList.Count == 1)
                        {
                            testPointSelect_SelectedIndexChanged(testPointSelect, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The scanned serial number is not at the correct testpoint.",
                            "Serial Number Disposition", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        testPointSelect.SelectedIndexChanged -= testPointSelect_SelectedIndexChanged;
                        testPointSelect.DataSource = null;
                        testPointSelect.Items.Clear();
                        testPointSelect.SelectedIndexChanged += testPointSelect_SelectedIndexChanged;
                    }
                }
            }
        }

        private Dictionary<string, object> GetFlags()
        {
            Dictionary<string, object> flags = new Dictionary<string, object>();
            flags.Add("EngMode", engineeringMode.Checked);
            flags.Add("TestMode", testMode.Checked);
            flags.Add("NoDatabase", noDatabase.Checked);
            flags.Add("Employee", operatorId.Text);
            flags.Add("SerialNum", uutSerialNumber.Text);
            flags.Add("SpecPath", Environment.CurrentDirectory + "\\DbSpec\\");
            flags.Add("Handle", this.Handle);

            return flags;
        }

        //-----------------------------------------------------------------
        // Set menu and controls when the testpoint is changed
        private async void testPointSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            AllowFormControls(false);
            ITestPoint selectedTp = testPointSelect.SelectedValue as ITestPoint;
            testPointInfo.ForeColor = default(Color);

            try
            {
                if (previousTp != selectedTp || errorSelectingIndex)
                {
                    testProgressBar.Value = 10;
                    testProgressBar.BarText = "Clearing Previous Testpoint";

                    // Close the previous ports if there were any
                    if (previousTp != null)
                    {
                        IHoldResources prevTask = previousTp as IHoldResources;
                        if (prevTask != null)
                        {
                            await prevTask.UnSelect();
                        }
                        IHazAPort prevPort = previousTp as IHazAPort;
                        if (prevPort != null)
                        {
                            int i = tabControl1.TabPages.Count - 1;
                            foreach (var port in prevPort.Ports)
                            {
                                //port.Cancel();

                                if (portTabsActive)
                                {
                                    TabPage tp = tabControl1.TabPages[i];
                                    tp.Controls.Clear();
                                    tabControl1.TabPages.RemoveAt(i);
                                    tp.Dispose();
                                    i--;
                                }
                            }
                        }
                        portTabsActive = false;
                    }
                    previousTp = selectedTp;

                    // Set the status interface
                    selectedTp.StatusHandle = progressHandler;
                    var flags = GetFlags();
                    selectedTp.Flags = flags;

                    testProgressBar.Value = 80;
                    testProgressBar.BarText = "Creating Controls";

                    // Update the window to pick which tasks to run
                    if (selectedTp.Tasks != null)
                    {
                        testPicker.TaskList = selectedTp.Tasks.Select(s => s.DisplayName).ToList();
                    }
                    else
                    {
                        testPicker.TaskList = null;
                    }

                    // Update the testpoint control (or hide the tab if the TP doesn't have one
                    // Size to grow the GUI by is there is a large test point control
                    int addX = 0;
                    int addY = 0;

                    // Clear any existing test point control
                    testPointControls.Controls.Clear();

                    UserControl testControl = selectedTp.TestControl;
                    // If the testpoint doesn't have a control, hide the control tab
                    if (testControl == null)
                    {
                        if (!controlsHidden)
                        {
                            testPointControls.SetInvisible();
                            controlsHidden = true;
                        }
                        this.Size = this.MinimumSize;
                    }

                    // Otherwise configure to show the test point control
                    else
                    {
                        // Show the tab if it isn't already
                        if (controlsHidden)
                        {
                            testPointControls.SetVisible(tabControl1);
                            controlsHidden = false;
                        }
                        // Add the control
                        testPointControls.Controls.Add(testControl);
                        testControl.Location = new Point(0, 0);
                        tabControl1.SelectedTab = testPointControls;
                        Size controlSize = testControl.Size;
                        // Calculate form size adjustment
                        addX = Math.Max(0, controlSize.Width - 700);
                        addY = Math.Max(0, controlSize.Height - 380);
                        // clear anchor styles first to avoid problems
                        testControl.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    }

                    // Set the form size
                    this.Size = new Size(this.MinimumSize.Width + addX, this.MinimumSize.Height + addY);
                    // Set the anchor after resize so it looks right
                    if (testControl != null)
                    {
                        testControl.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
                    }

                    // Get the version information
                    string tpDllName = Assembly.GetAssembly(selectedTp.GetType()).GetName().Name;
                    string tpVersion = Assembly.GetAssembly(selectedTp.GetType()).GetName().Version.ToString();
                    this.Text = titleText + " : " + tpDllName + " " + tpVersion;

                    IHoldResources thisTask = selectedTp as IHoldResources;
                    if (thisTask != null)
                    {
                        await thisTask.Select();
                    }
                    // Open the new ports if there are any
                    IHazAPort portObject = selectedTp as IHazAPort;
                    if (portObject != null)
                    {
                        int i = 0;
                        foreach (var port in portObject.Ports)
                        {
                            port.Start();

                            if (engineeringMode.Checked)
                            {
                                TabPage tp = new TabPage();
                                //tp.Name = "Port " + i.ToString();
                                tp.Text = "Port " + i.ToString();
                                UutCompact ctrl = null;
                                if (portControls.ContainsKey(selectedTp.TestPointName + i.ToString()))
                                {
                                    ctrl = portControls[selectedTp.TestPointName + i.ToString()];
                                }
                                else
                                {
                                    ctrl = new UutCompact();
                                    ctrl.Port = port;
                                    portControls.Add(selectedTp.TestPointName + i.ToString(), ctrl);
                                }
                                tp.Controls.Add(ctrl);
                                ctrl.Location = new Point(0, 0);
                                ctrl.Size = new Size(tabControl1.Size.Width - 10, tabControl1.Size.Height - 30);
                                tabControl1.TabPages.Add(tp);
                                i++;

                                portTabsActive = true;
                            }
                        }
                    }
                }
                testProgressBar.Value = 0;
                testProgressBar.BarText = "Ready";
                errorSelectingIndex = false;
                runTestPoint.Enabled = true;
                runTestPoint.BackColor = Color.Yellow;
            }
            catch (Exception ex)
            {
                errorSelectingIndex = true;
                runTestPoint.Enabled = false;

                if (selectedTp.StatusHandle != null)
                {
                    StatusInfo.ReportException(selectedTp.StatusHandle, "SelectedIndexChanged", ex);
                }
                // Clear any existing test point control
                testPointControls.Controls.Clear();

                MessageBox.Show(ex.ToString(), "Error Selecting Testpoint", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            AllowFormControls(true);
        }

        //-----------------------------------------------------------------
        // Run a testpoint (or cancel a running one
        private async void runTestPoint_Click(object sender, EventArgs e)
        {
            runTestPoint.BackColor = default(Color);
            // If starting a new testpoint
            if (runTestPoint.Text.Contains("Run"))
            {
                // Clear previous progress information
                testProgressBar.Value = 0;
                testProgressBar.BarText = "";

                TaskInfo.GlobalAborted = false;

                if (String.IsNullOrEmpty(operatorId.Text))
                {
                    MessageBox.Show("Please enter operator ID");
                    operatorId.Select();
                }
                else if (testPointSelect.SelectedValue == null)
                {
                    /* - TODO - need to do anything here? this should be an odd special case */
                }
                else
                {
                    ITestPoint tp = testPointSelect.SelectedValue as ITestPoint;

                    bool allowTpToRun = true;
                    if (verifyTestPointVersion)
                    {
                        // https://stackoverflow.com/q/11848785
                        string tpDllName = Assembly.GetAssembly(tp.GetType()).GetName().Name;
                        string tpVersion = Assembly.GetAssembly(tp.GetType()).GetName().Version.ToString();
                        string tpInfo = tpDllName + " " + tpVersion;

                        if (MessageBox.Show("Selected test point is version:\n\n" + tpInfo + "\n\n" + "Does this match the test station certification sticker?", "Verify Test Point", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            allowTpToRun = false;
                        }
                    }
                    if (verifyEquipmentInCal)
                    {
                        if (MessageBox.Show("Is all test equipment within their cal dates?", "Verify In Cal", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            allowTpToRun = false;
                        }
                    }
                    if(allowTpToRun)
                    {
                        requestTestCancel = new CancellationTokenSource();

                        tp.Database = paperlessDb;
                        // Set the selected tasks to run
                        tp.TasksToRun = testPicker.SelectedTasks;
                        Dictionary<string, object> flags = GetFlags();
                        tp.Flags = flags;
                        try
                        {
                            // Disable controls
                            AllowFormControls(false);

                            // Start the log
                            testProgressBar.BarText = "Enabling Logging";
                            await Task.Run(() => {
                                StartLogging(tp.DisplayName);
                            });

                            // Set the flags
                            if (tp.Tasks != null)
                            {
                                foreach (var tt in tp.Tasks)
                                {
                                    tt.Flags = flags;
                                    tt.Database = paperlessDb;
                                }
                            }

                            // Run the test
                            testProgressBar.BarText = "Starting Test Point " + tp.DisplayName;
                            await tp.Run(requestTestCancel.Token);
                            testProgressBar.BarText = "Finished";
                        }
                        // If it was cancelled
                        catch (TaskCanceledException)
                        {
                            testProgressBar.BarText = "Cancelled";
                        }
                        // If the test had an error it was expecting
                        catch (ApplicationException ex)
                        {
                            MessageBox.Show("Error during test: " + ex.Message);
                            testProgressBar.BarText = "Finished: Error";
                        }
                        // If there was a different error
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unexpected exception: " + ex.Message);
                            testProgressBar.BarText = "Finished: Error";
                        }
                        finally
                        {
                        }

                        if (clearOperatorId)
                        {
                            // I don't think that the ID should be cleared
                            // If the test wasn't cancelled, clear the operator ID so another test can be run
                            if (!requestTestCancel.IsCancellationRequested)
                            {
                                operatorId.Text = "";
                            }
                        }

                        requestTestCancel.Dispose();

                        // The Progress event that is reporting the state of the last task has not been handled
                        // Use DoEvents to make sure it is (and disable the form around it to avoid side effects)
                        // http://stackoverflow.com/a/5183623
                        this.Enabled = false;
                        await Task.Run(() => { Application.DoEvents(); });
                        this.Enabled = true;

                        // Finish test
                        testProgressBar.BarText = "Finished";
                        testProgressBar.Value = testProgressBar.Maximum;
                        FinishLogging();
                        AllowFormControls(true);
                        if (clearSerialNumber)
                        {
                            uutSerialNumber.Clear();
                        }
                        uutSerialNumber.Select();
                    }
                }
            }

            // A test is already running, so cancel it
            else
            {
                // Cancel the test
                requestTestCancel.Cancel();
                TaskInfo.GlobalAborted = true;
            }
        }

        /// <summary>
        /// Disable and change control test based on if a test is running
        /// </summary>
        /// <param name="state">false to disable controls, true to enable</param>
        private void AllowFormControls(bool state)
        {
            mainMenuBar.Enabled = state;
            runTestPoint.Text = state ? "Run Test Point" : "Cancel";
            testPointSelect.Enabled = state;
            testPicker.AllowSelection = state;
            uutSerialNumber.Enabled = state;
            operatorId.Enabled = state;
            //tabControl1.Enabled = state;
        }

        /// <summary>
        /// Create the log file that a test uses
        /// </summary>
        /// <param name="test">Test name</param>
        private void StartLogging(string test)
        {
            LoggerProperties.SelectedLogger = null;
            // Get the current logger type
            try
            {
                LoggerProperties.SelectedLogger = loggerSelect.ComboBox.SelectedValue as ILogger;
            }
            catch (Exception)
            {
            }
            // If there is a logger selected
            if (LoggerProperties.SelectedLogger != null)
            {
                //  Make sure the directory exists that we are logging to
                if(!Directory.Exists(Settings.Default.LoggingDir))
                {
                    Directory.CreateDirectory(Settings.Default.LoggingDir);
                }

                // Get the filename for the log (date + testpoint)
                DateTime dt = DateTime.Now;
                string timeStr = String.Format("{0,4:00}{1,2:00}{2,2:00}_{3,2:00}{4,2:00}{5,2:00}",
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                string logFileName = Settings.Default.LoggingDir + "\\" + timeStr + "_" + test;
                bool usingLogFile = LoggerProperties.SelectedLogger.Open(logFileName);
                // If it didn't open successfully, ask if they want to continue
                if (!usingLogFile)
                {
                    if (MessageBox.Show("Unable to open log, continue without?", "Log Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        throw new ApplicationException("Unable to open log file: " + logFileName);
                    }
                }
                // If it did open, connect the write function to the progress handler
                else
                {
                    progressHandler.ProgressChanged += LoggerProperties.SelectedLogger.Write;
                }
            }
        }

        /// <summary>
        /// Finish testpoint logging
        /// </summary>
        private void FinishLogging()
        {
            ILogger selectedLogger = null;
            // Get the current logger
            try
            {
                selectedLogger = loggerSelect.ComboBox.SelectedValue as ILogger;
            }
            catch (Exception)
            {
            }
            // If it exists and is open, close it
            if ((selectedLogger != null) && !String.IsNullOrEmpty(selectedLogger.LogFileName))
            {
                progressHandler.ProgressChanged -= selectedLogger.Write;
                selectedLogger.Close();
            }
        }

        // Handle progress events from a testpoint
        private void ProgressHandler_UpdateGuiState(object sender, StatusInfo e)
        {
            // If its a progress bar update, set percent and text
            if (e.Severity == StatusType.Progress)
            {
                testProgressBar.BarText = e.Message;
                testProgressBar.Value = e.Id;
            }
            // If its a task state update, set TreeView
            if (e.Severity == StatusType.Task)
            {
                testPicker.ChangeTaskStatus(e.SourceTag, (TaskState)e.Id);
            }
            else
            {
                logWindow.AddEntry(e);
            }
        }

        // Save GUI configuration to last file
        private void saveConfig_Click(object sender, EventArgs e)
        {
        }

        // Save GUI configuration to a specific file
        private void saveAsConfig_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
            }
            sfd.Dispose();
        }

        // open an existing configuration
        private void openConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
            }
        }

        // Open the logging directory in Windows Explorer
        private void openLoggingDir_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", Settings.Default.LoggingDir);
        }

        private void exitApplication_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Displays the Test Station XML file in notepad.
        /// </summary>
        private void config_openTestStationXML_Click(object sender, EventArgs e)
        {
            Process.Start("Notepad.exe", configManager.GetFilePath(ConfigFile.TEST_STATION));
        }

        /// <summary>
        /// Pressing Enter in the Emp ID box moves the cursor down to the SN box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void operatorId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == '\r') && (!string.IsNullOrEmpty(operatorId.Text)))
            {
                operatorId.BackColor = default(Color);
                uutSerialNumber.Select();
                uutSerialNumber.BackColor = Color.Yellow;
            }
        }
    }
}
