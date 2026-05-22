using System.Numerics;

namespace OdtwarzaczAudiobookow
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // Main containers
            this.panelMain = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();

            // Library panel
            this.panelLibrary = new System.Windows.Forms.Panel();
            this.listViewBooks = new System.Windows.Forms.ListView();
            this.columnTitle = new System.Windows.Forms.ColumnHeader();
            this.columnAuthor = new System.Windows.Forms.ColumnHeader();
            this.columnDuration = new System.Windows.Forms.ColumnHeader();
            this.columnPosition = new System.Windows.Forms.ColumnHeader();
            this.columnProgress = new System.Windows.Forms.ColumnHeader();

            // Search panel
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cmbSortBy = new System.Windows.Forms.ComboBox();
            this.lblSortBy = new System.Windows.Forms.Label();
            this.chkSortAscending = new System.Windows.Forms.CheckBox();

            // Buttons panel
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnImportFolder = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();

            // Player panel
            this.panelPlayer = new System.Windows.Forms.Panel();
            this.groupBoxPlayer = new System.Windows.Forms.GroupBox();
            this.lblNowPlaying = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.trackBarPosition = new System.Windows.Forms.TrackBar();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.lblVolumeValue = new System.Windows.Forms.Label();
            this.pictureBoxCover = new System.Windows.Forms.PictureBox();

            // Chapters panel
            this.groupBoxChapters = new System.Windows.Forms.GroupBox();
            this.listViewChapters = new System.Windows.Forms.ListView();
            this.columnChapterTitle = new System.Windows.Forms.ColumnHeader();
            this.columnChapterTime = new System.Windows.Forms.ColumnHeader();

            // Speed panel
            this.groupBoxSpeed = new System.Windows.Forms.GroupBox();
            this.trackBarSpeed = new System.Windows.Forms.TrackBar();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblSpeedMin = new System.Windows.Forms.Label();
            this.lblSpeedMax = new System.Windows.Forms.Label();

            // Sync panel
            this.groupBoxSync = new System.Windows.Forms.GroupBox();
            this.chkSyncEnabled = new System.Windows.Forms.CheckBox();
            this.chkServerMode = new System.Windows.Forms.CheckBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnApplySync = new System.Windows.Forms.Button();
            this.lblSyncStatus = new System.Windows.Forms.Label();

            // Background music panel
            this.groupBoxBackground = new System.Windows.Forms.GroupBox();
            this.chkBackgroundMusic = new System.Windows.Forms.CheckBox();
            this.btnSelectMusic = new System.Windows.Forms.Button();
            this.lblBgVolume = new System.Windows.Forms.Label();
            this.trackBarBackgroundVolume = new System.Windows.Forms.TrackBar();
            this.lblBgVolumeValue = new System.Windows.Forms.Label();
            this.cmbAmbientSound = new System.Windows.Forms.ComboBox();
            this.lblAmbient = new System.Windows.Forms.Label();
            this.lblCurrentBgMusic = new System.Windows.Forms.Label();
            this.lblCurrentBgMusicLabel = new System.Windows.Forms.Label();

            // Settings panel
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.chkAutoResume = new System.Windows.Forms.CheckBox();
            this.lblSeekSeconds = new System.Windows.Forms.Label();
            this.numSeekSeconds = new System.Windows.Forms.NumericUpDown();

            // Keyboard shortcuts panel
            this.groupBoxShortcuts = new System.Windows.Forms.GroupBox();
            this.lblShortcuts = new System.Windows.Forms.Label();

            // Initialize
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelLibrary.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelPlayer.SuspendLayout();
            this.groupBoxPlayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).BeginInit();
            this.groupBoxChapters.SuspendLayout();
            this.groupBoxSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            this.groupBoxSync.SuspendLayout();
            this.groupBoxBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBackgroundVolume)).BeginInit();
            this.groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSeekSeconds)).BeginInit();
            this.groupBoxShortcuts.SuspendLayout();
            this.SuspendLayout();

            // panelMain
            this.panelMain.Controls.Add(this.splitContainer);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(10);
            this.panelMain.Size = new System.Drawing.Size(1084, 761);
            this.panelMain.TabIndex = 0;

            // splitContainer
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(10, 10);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer.Size = new System.Drawing.Size(1064, 741);
            this.splitContainer.SplitterDistance = 300;
            this.splitContainer.TabIndex = 0;

            // splitContainer.Panel1
            this.splitContainer.Panel1.Controls.Add(this.panelLibrary);
            this.splitContainer.Panel1.Controls.Add(this.panelButtons);
            this.splitContainer.Panel1.Controls.Add(this.panelSearch);

            // splitContainer.Panel2
            this.splitContainer.Panel2.Controls.Add(this.panelPlayer);

            // panelSearch
            this.panelSearch.Controls.Add(this.chkSortAscending);
            this.panelSearch.Controls.Add(this.cmbSortBy);
            this.panelSearch.Controls.Add(this.lblSortBy);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1064, 40);
            this.panelSearch.TabIndex = 0;

            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(5, 12);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(46, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Szukaj:";

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(57, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Wpisz tytuł lub autora...";
            this.txtSearch.Size = new System.Drawing.Size(400, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // lblSortBy
            this.lblSortBy.AutoSize = true;
            this.lblSortBy.Location = new System.Drawing.Point(480, 12);
            this.lblSortBy.Name = "lblSortBy";
            this.lblSortBy.Size = new System.Drawing.Size(56, 15);
            this.lblSortBy.TabIndex = 2;
            this.lblSortBy.Text = "Sortuj po:";

            // cmbSortBy
            this.cmbSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortBy.Items.AddRange(new object[] { "Tytuł", "Autor", "Ostatnio odtwarzane", "Czas trwania" });
            this.cmbSortBy.Location = new System.Drawing.Point(542, 8);
            this.cmbSortBy.Name = "cmbSortBy";
            this.cmbSortBy.Size = new System.Drawing.Size(150, 23);
            this.cmbSortBy.TabIndex = 3;
            this.cmbSortBy.SelectedIndexChanged += new System.EventHandler(this.cmbSortBy_SelectedIndexChanged);

            // chkSortAscending
            this.chkSortAscending.AutoSize = true;
            this.chkSortAscending.Checked = true;
            this.chkSortAscending.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSortAscending.Location = new System.Drawing.Point(700, 10);
            this.chkSortAscending.Name = "chkSortAscending";
            this.chkSortAscending.Size = new System.Drawing.Size(79, 19);
            this.chkSortAscending.TabIndex = 4;
            this.chkSortAscending.Text = "Rosnąco";
            this.chkSortAscending.UseVisualStyleBackColor = true;
            this.chkSortAscending.CheckedChanged += new System.EventHandler(this.chkSortAscending_CheckedChanged);

            // panelLibrary
            this.panelLibrary.Controls.Add(this.listViewBooks);
            this.panelLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLibrary.Location = new System.Drawing.Point(0, 40);
            this.panelLibrary.Name = "panelLibrary";
            this.panelLibrary.Size = new System.Drawing.Size(1064, 220);
            this.panelLibrary.TabIndex = 1;

            // listViewBooks
            this.listViewBooks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.columnTitle,
                this.columnAuthor,
                this.columnDuration,
                this.columnPosition,
                this.columnProgress});
            this.listViewBooks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBooks.FullRowSelect = true;
            this.listViewBooks.GridLines = true;
            this.listViewBooks.Location = new System.Drawing.Point(0, 0);
            this.listViewBooks.Name = "listViewBooks";
            this.listViewBooks.Size = new System.Drawing.Size(1064, 220);
            this.listViewBooks.TabIndex = 0;
            this.listViewBooks.UseCompatibleStateImageBehavior = false;
            this.listViewBooks.View = System.Windows.Forms.View.Details;
            this.listViewBooks.DoubleClick += new System.EventHandler(this.listViewBooks_DoubleClick);

            // Column Headers
            this.columnTitle.Text = "Tytuł";
            this.columnTitle.Width = 350;
            this.columnAuthor.Text = "Autor";
            this.columnAuthor.Width = 250;
            this.columnDuration.Text = "Czas trwania";
            this.columnDuration.Width = 100;
            this.columnPosition.Text = "Pozycja";
            this.columnPosition.Width = 100;
            this.columnProgress.Text = "Postęp";
            this.columnProgress.Width = 80;

            // panelButtons
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnImportFolder);
            this.panelButtons.Controls.Add(this.btnImport);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 260);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1064, 40);
            this.panelButtons.TabIndex = 2;

            // btnImport
            this.btnImport.Location = new System.Drawing.Point(5, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Importuj pliki";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);

            // btnImportFolder
            this.btnImportFolder.Location = new System.Drawing.Point(131, 5);
            this.btnImportFolder.Name = "btnImportFolder";
            this.btnImportFolder.Size = new System.Drawing.Size(130, 30);
            this.btnImportFolder.TabIndex = 1;
            this.btnImportFolder.Text = "Importuj folder";
            this.btnImportFolder.UseVisualStyleBackColor = true;
            this.btnImportFolder.Click += new System.EventHandler(this.btnImportFolder_Click);

            // btnEdit
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(813, 5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(120, 30);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edytuj";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(939, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Usuń";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // panelPlayer
            this.panelPlayer.Controls.Add(this.groupBoxShortcuts);
            this.panelPlayer.Controls.Add(this.groupBoxSettings);
            this.panelPlayer.Controls.Add(this.groupBoxBackground);
            this.panelPlayer.Controls.Add(this.groupBoxSync);
            this.panelPlayer.Controls.Add(this.groupBoxChapters);
            this.panelPlayer.Controls.Add(this.groupBoxSpeed);
            this.panelPlayer.Controls.Add(this.groupBoxPlayer);
            this.panelPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayer.Location = new System.Drawing.Point(0, 0);
            this.panelPlayer.Name = "panelPlayer";
            this.panelPlayer.Size = new System.Drawing.Size(1064, 437);
            this.panelPlayer.TabIndex = 0;

            // groupBoxPlayer
            this.groupBoxPlayer.Controls.Add(this.pictureBoxCover);
            this.groupBoxPlayer.Controls.Add(this.lblVolumeValue);
            this.groupBoxPlayer.Controls.Add(this.trackBarVolume);
            this.groupBoxPlayer.Controls.Add(this.lblVolume);
            this.groupBoxPlayer.Controls.Add(this.lblTotalTime);
            this.groupBoxPlayer.Controls.Add(this.lblCurrentTime);
            this.groupBoxPlayer.Controls.Add(this.trackBarPosition);
            this.groupBoxPlayer.Controls.Add(this.btnNext);
            this.groupBoxPlayer.Controls.Add(this.btnStop);
            this.groupBoxPlayer.Controls.Add(this.btnPlay);
            this.groupBoxPlayer.Controls.Add(this.btnPrev);
            this.groupBoxPlayer.Controls.Add(this.lblNowPlaying);
            this.groupBoxPlayer.Location = new System.Drawing.Point(5, 5);
            this.groupBoxPlayer.Name = "groupBoxPlayer";
            this.groupBoxPlayer.Size = new System.Drawing.Size(700, 180);  
            this.groupBoxPlayer.TabIndex = 0;
            this.groupBoxPlayer.TabStop = false;
            this.groupBoxPlayer.Text = "Odtwarzacz";


            // lblNowPlaying - tytuł audiobooka 
            this.lblNowPlaying.AutoSize = true;
            this.lblNowPlaying.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblNowPlaying.Location = new System.Drawing.Point(10, 25);
            this.lblNowPlaying.Name = "lblNowPlaying";
            this.lblNowPlaying.Size = new System.Drawing.Size(450, 20);  // Zwiększono szerokość
            this.lblNowPlaying.TabIndex = 0;
            this.lblNowPlaying.Text = "Brak odtwarzania";


            // pictureBoxCover - okładka 
            this.pictureBoxCover.Location = new System.Drawing.Point(580, 20);
            this.pictureBoxCover.Name = "pictureBoxCover";
            this.pictureBoxCover.Size = new System.Drawing.Size(80, 80);  
            this.pictureBoxCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCover.TabIndex = 11;
            this.pictureBoxCover.TabStop = false;


            // btnPrev
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnPrev.Location = new System.Drawing.Point(10, 60);  // Przesunięto w dół
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(55, 40);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "⏪";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);

            // btnPlay
            this.btnPlay.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btnPlay.Location = new System.Drawing.Point(71, 60);  // Przesunięto w dół
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(65, 40);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "▶";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);

            // btnStop
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnStop.Location = new System.Drawing.Point(142, 60);  // Przesunięto w dół
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(55, 40);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "⏹";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);

            // btnNext
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnNext.Location = new System.Drawing.Point(203, 60);  // Przesunięto w dół
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(55, 40);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "⏩";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);

            // trackBarPosition
            this.trackBarPosition.Location = new System.Drawing.Point(70, 110);  // Przesunięto w dół
            this.trackBarPosition.Maximum = 100;
            this.trackBarPosition.Name = "trackBarPosition";
            this.trackBarPosition.Size = new System.Drawing.Size(300, 45);
            this.trackBarPosition.TabIndex = 5;
            this.trackBarPosition.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarPosition.Scroll += new System.EventHandler(this.trackBarPosition_Scroll);
            this.trackBarPosition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarPosition_MouseDown);
            this.trackBarPosition.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarPosition_MouseUp);

            // lblCurrentTime
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(10, 115);  // Przesunięto w dół
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(55, 15);
            this.lblCurrentTime.TabIndex = 6;
            this.lblCurrentTime.Text = "00:00:00";

            // lblTotalTime
            this.lblTotalTime.AutoSize = true;
            this.lblTotalTime.Location = new System.Drawing.Point(376, 115);  // Przesunięto w dół
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(55, 15);
            this.lblTotalTime.TabIndex = 7;
            this.lblTotalTime.Text = "00:00:00";

            // lblVolume - etykieta "Głośność:" (obok czasu)
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(440, 115);  // Po prawej stronie czasu
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(58, 15);
            this.lblVolume.TabIndex = 8;
            this.lblVolume.Text = "Głośność:";

            // trackBarVolume - pasek głośności (obok czasu, pod tytułem)
            this.trackBarVolume.Location = new System.Drawing.Point(504, 110);  // Obok czasu
            this.trackBarVolume.Maximum = 100;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(80, 45);  // Wąski pasek
            this.trackBarVolume.TabIndex = 9;
            this.trackBarVolume.TickFrequency = 10;
            this.trackBarVolume.Value = 100;
            this.trackBarVolume.Scroll += new System.EventHandler(this.trackBarVolume_Scroll);

            // lblVolumeValue - wartość procentowa "100%" (obok paska)
            this.lblVolumeValue.AutoSize = true;
            this.lblVolumeValue.Location = new System.Drawing.Point(590, 115);  // Po prawej stronie paska
            this.lblVolumeValue.Name = "lblVolumeValue";
            this.lblVolumeValue.Size = new System.Drawing.Size(35, 15);
            this.lblVolumeValue.TabIndex = 10;
            this.lblVolumeValue.Text = "100%";

            // groupBoxChapters
            this.groupBoxChapters.Controls.Add(this.listViewChapters);
            this.groupBoxChapters.Location = new System.Drawing.Point(711, 5);
            this.groupBoxChapters.Name = "groupBoxChapters";
            this.groupBoxChapters.Size = new System.Drawing.Size(348, 160);
            this.groupBoxChapters.TabIndex = 1;
            this.groupBoxChapters.TabStop = false;
            this.groupBoxChapters.Text = "Rozdziały";

            // listViewChapters
            this.listViewChapters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.columnChapterTitle,
                this.columnChapterTime});
            this.listViewChapters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewChapters.FullRowSelect = true;
            this.listViewChapters.Location = new System.Drawing.Point(3, 19);
            this.listViewChapters.Name = "listViewChapters";
            this.listViewChapters.Size = new System.Drawing.Size(342, 138);
            this.listViewChapters.TabIndex = 0;
            this.listViewChapters.UseCompatibleStateImageBehavior = false;
            this.listViewChapters.View = System.Windows.Forms.View.Details;
            this.listViewChapters.DoubleClick += new System.EventHandler(this.listViewChapters_DoubleClick);

            // columnChapterTitle
            this.columnChapterTitle.Text = "Rozdział";
            this.columnChapterTitle.Width = 220;

            // columnChapterTime
            this.columnChapterTime.Text = "Czas";
            this.columnChapterTime.Width = 80;

            // groupBoxSpeed
            this.groupBoxSpeed.Controls.Add(this.lblSpeedMax);
            this.groupBoxSpeed.Controls.Add(this.lblSpeedMin);
            this.groupBoxSpeed.Controls.Add(this.lblSpeed);
            this.groupBoxSpeed.Controls.Add(this.trackBarSpeed);
            this.groupBoxSpeed.Location = new System.Drawing.Point(5, 171);
            this.groupBoxSpeed.Name = "groupBoxSpeed";
            this.groupBoxSpeed.Size = new System.Drawing.Size(350, 80);
            this.groupBoxSpeed.TabIndex = 2;
            this.groupBoxSpeed.TabStop = false;
            this.groupBoxSpeed.Text = "Prędkość odtwarzania (zachowuje ton głosu)";

            // trackBarSpeed
            this.trackBarSpeed.LargeChange = 10;
            this.trackBarSpeed.Location = new System.Drawing.Point(10, 25);
            this.trackBarSpeed.Maximum = 200;
            this.trackBarSpeed.Minimum = 50;
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new System.Drawing.Size(280, 45);
            this.trackBarSpeed.SmallChange = 5;
            this.trackBarSpeed.TabIndex = 0;
            this.trackBarSpeed.TickFrequency = 25;
            this.trackBarSpeed.Value = 100;
            this.trackBarSpeed.Scroll += new System.EventHandler(this.trackBarSpeed_Scroll);

            // lblSpeed
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSpeed.Location = new System.Drawing.Point(296, 30);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(45, 19);
            this.lblSpeed.TabIndex = 1;
            this.lblSpeed.Text = "1.00x";

            // lblSpeedMin
            this.lblSpeedMin.AutoSize = true;
            this.lblSpeedMin.Location = new System.Drawing.Point(10, 65);
            this.lblSpeedMin.Name = "lblSpeedMin";
            this.lblSpeedMin.Size = new System.Drawing.Size(33, 15);
            this.lblSpeedMin.TabIndex = 2;
            this.lblSpeedMin.Text = "0.5x";

            // lblSpeedMax
            this.lblSpeedMax.AutoSize = true;
            this.lblSpeedMax.Location = new System.Drawing.Point(256, 65);
            this.lblSpeedMax.Name = "lblSpeedMax";
            this.lblSpeedMax.Size = new System.Drawing.Size(30, 15);
            this.lblSpeedMax.TabIndex = 3;
            this.lblSpeedMax.Text = "2.0x";

            // groupBoxSync
            this.groupBoxSync.Controls.Add(this.lblSyncStatus);
            this.groupBoxSync.Controls.Add(this.btnApplySync);
            this.groupBoxSync.Controls.Add(this.txtPort);
            this.groupBoxSync.Controls.Add(this.lblPort);
            this.groupBoxSync.Controls.Add(this.txtServerIP);
            this.groupBoxSync.Controls.Add(this.lblIP);
            this.groupBoxSync.Controls.Add(this.chkServerMode);
            this.groupBoxSync.Controls.Add(this.chkSyncEnabled);
            this.groupBoxSync.Location = new System.Drawing.Point(361, 171);
            this.groupBoxSync.Name = "groupBoxSync";
            this.groupBoxSync.Size = new System.Drawing.Size(344, 160);
            this.groupBoxSync.TabIndex = 3;
            this.groupBoxSync.TabStop = false;
            this.groupBoxSync.Text = "Synchronizacja sieciowa (NetMQ/TCP)";

            // chkSyncEnabled
            this.chkSyncEnabled.AutoSize = true;
            this.chkSyncEnabled.Location = new System.Drawing.Point(10, 25);
            this.chkSyncEnabled.Name = "chkSyncEnabled";
            this.chkSyncEnabled.Size = new System.Drawing.Size(146, 19);
            this.chkSyncEnabled.TabIndex = 0;
            this.chkSyncEnabled.Text = "Włącz synchronizację";
            this.chkSyncEnabled.UseVisualStyleBackColor = true;
            this.chkSyncEnabled.CheckedChanged += new System.EventHandler(this.chkSyncEnabled_CheckedChanged);

            // chkServerMode
            this.chkServerMode.AutoSize = true;
            this.chkServerMode.Location = new System.Drawing.Point(170, 25);
            this.chkServerMode.Name = "chkServerMode";
            this.chkServerMode.Size = new System.Drawing.Size(93, 19);
            this.chkServerMode.TabIndex = 1;
            this.chkServerMode.Text = "Tryb serwera";
            this.chkServerMode.UseVisualStyleBackColor = true;
            this.chkServerMode.CheckedChanged += new System.EventHandler(this.chkServerMode_CheckedChanged);

            // lblIP
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(10, 55);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(63, 15);
            this.lblIP.TabIndex = 2;
            this.lblIP.Text = "IP serwera:";

            // txtServerIP
            this.txtServerIP.Location = new System.Drawing.Point(80, 52);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(120, 23);
            this.txtServerIP.TabIndex = 3;
            this.txtServerIP.Text = "127.0.0.1";

            // lblPort
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(210, 55);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(32, 15);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port:";

            // txtPort
            this.txtPort.Location = new System.Drawing.Point(248, 52);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(80, 23);
            this.txtPort.TabIndex = 5;
            this.txtPort.Text = "5555";

            // btnApplySync
            this.btnApplySync.Location = new System.Drawing.Point(10, 85);
            this.btnApplySync.Name = "btnApplySync";
            this.btnApplySync.Size = new System.Drawing.Size(100, 30);
            this.btnApplySync.TabIndex = 6;
            this.btnApplySync.Text = "Zastosuj";
            this.btnApplySync.UseVisualStyleBackColor = true;
            this.btnApplySync.Click += new System.EventHandler(this.btnApplySync_Click);

            // lblSyncStatus
            this.lblSyncStatus.AutoSize = true;
            this.lblSyncStatus.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblSyncStatus.Location = new System.Drawing.Point(120, 92);
            this.lblSyncStatus.Name = "lblSyncStatus";
            this.lblSyncStatus.Size = new System.Drawing.Size(139, 15);
            this.lblSyncStatus.TabIndex = 7;
            this.lblSyncStatus.Text = "Synchronizacja wyłączona";

            // groupBoxBackground
            this.groupBoxBackground.Controls.Add(this.lblCurrentBgMusic);
            this.groupBoxBackground.Controls.Add(this.lblCurrentBgMusicLabel);
            this.groupBoxBackground.Controls.Add(this.lblAmbient);
            this.groupBoxBackground.Controls.Add(this.cmbAmbientSound);
            this.groupBoxBackground.Controls.Add(this.lblBgVolumeValue);
            this.groupBoxBackground.Controls.Add(this.trackBarBackgroundVolume);
            this.groupBoxBackground.Controls.Add(this.lblBgVolume);
            this.groupBoxBackground.Controls.Add(this.btnSelectMusic);
            this.groupBoxBackground.Controls.Add(this.chkBackgroundMusic);
            this.groupBoxBackground.Location = new System.Drawing.Point(711, 171);
            this.groupBoxBackground.Name = "groupBoxBackground";
            this.groupBoxBackground.Size = new System.Drawing.Size(348, 160);
            this.groupBoxBackground.TabIndex = 4;
            this.groupBoxBackground.TabStop = false;
            this.groupBoxBackground.Text = "Podkład muzyczny (tło)";

            // chkBackgroundMusic
            this.chkBackgroundMusic.AutoSize = true;
            this.chkBackgroundMusic.Location = new System.Drawing.Point(10, 25);
            this.chkBackgroundMusic.Name = "chkBackgroundMusic";
            this.chkBackgroundMusic.Size = new System.Drawing.Size(155, 19);
            this.chkBackgroundMusic.TabIndex = 0;
            this.chkBackgroundMusic.Text = "Włącz podkład muzyczny";
            this.chkBackgroundMusic.UseVisualStyleBackColor = true;
            this.chkBackgroundMusic.CheckedChanged += new System.EventHandler(this.chkBackgroundMusic_CheckedChanged);

            // btnSelectMusic
            this.btnSelectMusic.Location = new System.Drawing.Point(175, 21);
            this.btnSelectMusic.Name = "btnSelectMusic";
            this.btnSelectMusic.Size = new System.Drawing.Size(160, 26);
            this.btnSelectMusic.TabIndex = 1;
            this.btnSelectMusic.Text = "Wybierz własny plik";
            this.btnSelectMusic.UseVisualStyleBackColor = true;
            this.btnSelectMusic.Click += new System.EventHandler(this.btnSelectMusic_Click);

            // lblAmbient
            this.lblAmbient.AutoSize = true;
            this.lblAmbient.Location = new System.Drawing.Point(10, 55);
            this.lblAmbient.Name = "lblAmbient";
            this.lblAmbient.Size = new System.Drawing.Size(52, 15);
            this.lblAmbient.TabIndex = 2;
            this.lblAmbient.Text = "Ambient:";

            // cmbAmbientSound
            this.cmbAmbientSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAmbientSound.Location = new System.Drawing.Point(68, 52);
            this.cmbAmbientSound.Name = "cmbAmbientSound";
            this.cmbAmbientSound.Size = new System.Drawing.Size(267, 23);
            this.cmbAmbientSound.TabIndex = 3;
            this.cmbAmbientSound.SelectedIndexChanged += new System.EventHandler(this.cmbAmbientSound_SelectedIndexChanged);

            // lblBgVolume
            this.lblBgVolume.AutoSize = true;
            this.lblBgVolume.Location = new System.Drawing.Point(10, 85);
            this.lblBgVolume.Name = "lblBgVolume";
            this.lblBgVolume.Size = new System.Drawing.Size(76, 15);
            this.lblBgVolume.TabIndex = 4;
            this.lblBgVolume.Text = "Głośność tła:";

            // trackBarBackgroundVolume
            this.trackBarBackgroundVolume.Location = new System.Drawing.Point(92, 80);
            this.trackBarBackgroundVolume.Maximum = 100;
            this.trackBarBackgroundVolume.Name = "trackBarBackgroundVolume";
            this.trackBarBackgroundVolume.Size = new System.Drawing.Size(200, 45);
            this.trackBarBackgroundVolume.TabIndex = 5;
            this.trackBarBackgroundVolume.TickFrequency = 10;
            this.trackBarBackgroundVolume.Value = 20;
            this.trackBarBackgroundVolume.Scroll += new System.EventHandler(this.trackBarBackgroundVolume_Scroll);

            // lblBgVolumeValue
            this.lblBgVolumeValue.AutoSize = true;
            this.lblBgVolumeValue.Location = new System.Drawing.Point(298, 85);
            this.lblBgVolumeValue.Name = "lblBgVolumeValue";
            this.lblBgVolumeValue.Size = new System.Drawing.Size(32, 15);
            this.lblBgVolumeValue.TabIndex = 6;
            this.lblBgVolumeValue.Text = "20%";

            // lblCurrentBgMusicLabel
            this.lblCurrentBgMusicLabel.AutoSize = true;
            this.lblCurrentBgMusicLabel.Location = new System.Drawing.Point(10, 110);
            this.lblCurrentBgMusicLabel.Name = "lblCurrentBgMusicLabel";
            this.lblCurrentBgMusicLabel.Size = new System.Drawing.Size(56, 15);
            this.lblCurrentBgMusicLabel.TabIndex = 7;
            this.lblCurrentBgMusicLabel.Text = "Aktualny:";

            // lblCurrentBgMusic
            this.lblCurrentBgMusic.AutoSize = true;
            this.lblCurrentBgMusic.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblCurrentBgMusic.Location = new System.Drawing.Point(72, 110);
            this.lblCurrentBgMusic.Name = "lblCurrentBgMusic";
            this.lblCurrentBgMusic.Size = new System.Drawing.Size(32, 15);
            this.lblCurrentBgMusic.TabIndex = 8;
            this.lblCurrentBgMusic.Text = "Brak";

            // groupBoxSettings
            this.groupBoxSettings.Controls.Add(this.numSeekSeconds);
            this.groupBoxSettings.Controls.Add(this.lblSeekSeconds);
            this.groupBoxSettings.Controls.Add(this.chkAutoResume);
            this.groupBoxSettings.Location = new System.Drawing.Point(5, 257);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(350, 85);
            this.groupBoxSettings.TabIndex = 5;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Ustawienia";

            // chkAutoResume
            this.chkAutoResume.AutoSize = true;
            this.chkAutoResume.Checked = true;
            this.chkAutoResume.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoResume.Location = new System.Drawing.Point(10, 25);
            this.chkAutoResume.Name = "chkAutoResume";
            this.chkAutoResume.Size = new System.Drawing.Size(268, 19);
            this.chkAutoResume.TabIndex = 0;
            this.chkAutoResume.Text = "Automatyczne wznawianie ostatniej pozycji";
            this.chkAutoResume.UseVisualStyleBackColor = true;
            this.chkAutoResume.CheckedChanged += new System.EventHandler(this.chkAutoResume_CheckedChanged);

            // lblSeekSeconds
            this.lblSeekSeconds.AutoSize = true;
            this.lblSeekSeconds.Location = new System.Drawing.Point(10, 55);
            this.lblSeekSeconds.Name = "lblSeekSeconds";
            this.lblSeekSeconds.Size = new System.Drawing.Size(130, 15);
            this.lblSeekSeconds.TabIndex = 1;
            this.lblSeekSeconds.Text = "Skok przewijania (sek):";

            // numSeekSeconds
            this.numSeekSeconds.Location = new System.Drawing.Point(145, 53);
            this.numSeekSeconds.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            this.numSeekSeconds.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numSeekSeconds.Name = "numSeekSeconds";
            this.numSeekSeconds.Size = new System.Drawing.Size(60, 23);
            this.numSeekSeconds.TabIndex = 2;
            this.numSeekSeconds.Value = new decimal(new int[] { 30, 0, 0, 0 });
            this.numSeekSeconds.ValueChanged += new System.EventHandler(this.numSeekSeconds_ValueChanged);

            // groupBoxShortcuts
            this.groupBoxShortcuts.Controls.Add(this.lblShortcuts);
            this.groupBoxShortcuts.Location = new System.Drawing.Point(5, 348);
            this.groupBoxShortcuts.Name = "groupBoxShortcuts";
            this.groupBoxShortcuts.Size = new System.Drawing.Size(350, 155);
            this.groupBoxShortcuts.TabIndex = 6;
            this.groupBoxShortcuts.TabStop = false;
            this.groupBoxShortcuts.Text = "Skróty klawiszowe";

            // lblShortcuts
            this.lblShortcuts.AutoSize = false;
            this.lblShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShortcuts.Location = new System.Drawing.Point(3, 19);
            this.lblShortcuts.Name = "lblShortcuts";
            this.lblShortcuts.Padding = new System.Windows.Forms.Padding(5);
            this.lblShortcuts.Size = new System.Drawing.Size(344, 133);
            this.lblShortcuts.TabIndex = 0;
            this.lblShortcuts.Text =
                "Space - Odtwarzaj/Wstrzymaj\r\n" +
                "Escape - Zatrzymaj\r\n" +
                "← / → - Przewiń do tyłu/przodu\r\n" +
                "↑ / ↓ - Zwiększ/zmniejsz prędkość\r\n" +
                "Ctrl+↑ / Ctrl+↓ - Głośniej/Ciszej\r\n" +
                "M - Włącz/wyłącz muzykę tła";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 761);
            this.Controls.Add(this.panelMain);
            this.MinimumSize = new System.Drawing.Size(1100, 800);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Odtwarzacz Audiobooków - z synchronizacją sieciową";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);

            // Resume layout
            this.panelMain.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelLibrary.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelPlayer.ResumeLayout(false);
            this.groupBoxPlayer.ResumeLayout(false);
            this.groupBoxPlayer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).EndInit();
            this.groupBoxChapters.ResumeLayout(false);
            this.groupBoxSpeed.ResumeLayout(false);
            this.groupBoxSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
            this.groupBoxSync.ResumeLayout(false);
            this.groupBoxSync.PerformLayout();
            this.groupBoxBackground.ResumeLayout(false);
            this.groupBoxBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBackgroundVolume)).EndInit();
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSeekSeconds)).EndInit();
            this.groupBoxShortcuts.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelLibrary;
        private System.Windows.Forms.ListView listViewBooks;
        private System.Windows.Forms.ColumnHeader columnTitle;
        private System.Windows.Forms.ColumnHeader columnAuthor;
        private System.Windows.Forms.ColumnHeader columnDuration;
        private System.Windows.Forms.ColumnHeader columnPosition;
        private System.Windows.Forms.ColumnHeader columnProgress;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.Label lblSortBy;
        private System.Windows.Forms.CheckBox chkSortAscending;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnImportFolder;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Panel panelPlayer;
        private System.Windows.Forms.GroupBox groupBoxPlayer;
        private System.Windows.Forms.Label lblNowPlaying;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TrackBar trackBarPosition;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblTotalTime;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.Label lblVolumeValue;
        private System.Windows.Forms.PictureBox pictureBoxCover;
        private System.Windows.Forms.GroupBox groupBoxChapters;
        private System.Windows.Forms.ListView listViewChapters;
        private System.Windows.Forms.ColumnHeader columnChapterTitle;
        private System.Windows.Forms.ColumnHeader columnChapterTime;
        private System.Windows.Forms.GroupBox groupBoxSpeed;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblSpeedMin;
        private System.Windows.Forms.Label lblSpeedMax;
        private System.Windows.Forms.GroupBox groupBoxSync;
        private System.Windows.Forms.CheckBox chkSyncEnabled;
        private System.Windows.Forms.CheckBox chkServerMode;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnApplySync;
        private System.Windows.Forms.Label lblSyncStatus;
        private System.Windows.Forms.GroupBox groupBoxBackground;
        private System.Windows.Forms.CheckBox chkBackgroundMusic;
        private System.Windows.Forms.Button btnSelectMusic;
        private System.Windows.Forms.Label lblBgVolume;
        private System.Windows.Forms.TrackBar trackBarBackgroundVolume;
        private System.Windows.Forms.Label lblBgVolumeValue;
        private System.Windows.Forms.ComboBox cmbAmbientSound;
        private System.Windows.Forms.Label lblAmbient;
        private System.Windows.Forms.Label lblCurrentBgMusic;
        private System.Windows.Forms.Label lblCurrentBgMusicLabel;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.CheckBox chkAutoResume;
        private System.Windows.Forms.Label lblSeekSeconds;
        private System.Windows.Forms.NumericUpDown numSeekSeconds;
        private System.Windows.Forms.GroupBox groupBoxShortcuts;
        private System.Windows.Forms.Label lblShortcuts;
    }
}