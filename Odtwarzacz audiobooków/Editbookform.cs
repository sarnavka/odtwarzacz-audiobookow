namespace OdtwarzaczAudiobookow
{
    public class EditBookForm : Form
    {
        private readonly AudioBook audioBook;
        private TextBox txtTitle = null!;
        private TextBox txtAuthor = null!;
        private CheckBox chkSaveToFile = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;

        public bool SaveToFile => chkSaveToFile.Checked;

        public EditBookForm(AudioBook book)
        {
            this.audioBook = book;
            InitializeComponents();
            LoadBookData();
        }

        private void InitializeComponents()
        {
            this.Text = "Edytuj audiobook";
            this.Size = new Size(450, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblTitle = new Label
            {
                Text = "Tytuł:",
                Location = new Point(20, 25),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            txtTitle = new TextBox
            {
                Location = new Point(80, 22),
                Size = new Size(340, 23)
            };
            this.Controls.Add(txtTitle);

            var lblAuthor = new Label
            {
                Text = "Autor:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            this.Controls.Add(lblAuthor);

            txtAuthor = new TextBox
            {
                Location = new Point(80, 57),
                Size = new Size(340, 23)
            };
            this.Controls.Add(txtAuthor);

            var lblDuration = new Label
            {
                Text = "Czas trwania:",
                Location = new Point(20, 95),
                AutoSize = true
            };
            this.Controls.Add(lblDuration);

            var lblDurationValue = new Label
            {
                Text = audioBook.DisplayDuration,
                Location = new Point(110, 95),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            this.Controls.Add(lblDurationValue);

            var lblPath = new Label
            {
                Text = "Ścieżka:",
                Location = new Point(20, 120),
                AutoSize = true
            };
            this.Controls.Add(lblPath);

            var txtPath = new TextBox
            {
                Location = new Point(80, 117),
                Size = new Size(340, 23),
                Text = audioBook.FilePath,
                ReadOnly = true,
                BackColor = SystemColors.Control
            };
            this.Controls.Add(txtPath);

            chkSaveToFile = new CheckBox
            {
                Text = "Zapisz również do tagów pliku (ID3/MP4)",
                Location = new Point(20, 155),
                AutoSize = true
            };
            this.Controls.Add(chkSaveToFile);

            btnOK = new Button
            {
                Text = "OK",
                Location = new Point(240, 185),
                Size = new Size(90, 30),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);

            btnCancel = new Button
            {
                Text = "Anuluj",
                Location = new Point(335, 185),
                Size = new Size(85, 30),
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        private void LoadBookData()
        {
            txtTitle.Text = audioBook.Title;
            txtAuthor.Text = audioBook.Author;
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Tytuł nie może być pusty", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            audioBook.Title = txtTitle.Text.Trim();
            audioBook.Author = txtAuthor.Text.Trim();
        }
    }
}