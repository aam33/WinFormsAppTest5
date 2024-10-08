using EO.Internal;
using EO.WebBrowser;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace WinFormsAppTest5;

public partial class MusicPlayer : Form
{
    // connect a list of items (albums) to grid control
    BindingSource topBindingSource = new BindingSource();
    BindingSource bottomBindingSource = new BindingSource();
    public MusicPlayer()
    {
        // enable console
        bool useConsole = true;
        if (useConsole)
        {
            AllocConsole();
        }

        InitializeComponent();

        webView.Source = new Uri("https://accounts.spotify.com/en/login?continue=https%3A%2F%2Fopen.spotify.com%2F");

        //RetrieveMusic("https://accounts.spotify.com/en/login?continue=https%3A%2F%2Fopen.spotify.com%2F");
        //MessageBox.Show("Log in to Spotify in order to play entire tracks.");

        // attempt to handle KeyDown ENTER event
        this.KeyDown += new KeyEventHandler(MusicPlayer_KeyDown);
        this.KeyPreview = true;
    }

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AllocConsole();

    // consider TODO: take in boolean, if true then autoclick play, if false don't (don't autoplay unless side play button clicked)
    private async void RetrieveMusic(string musicUrl)
    {
        await webView.EnsureCoreWebView2Async(null);
        // Set a standard User-Agent string
        //webView.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        //webView.CoreWebView2.Navigate("https://www.youtube.com/embed/9FqaMehDsUc");   // WebView2 tutorial


        //webView.CoreWebView2.Navigate("https://open.spotify.com/embed/track/4PTG3Z6ehGkBFwjybzWkR8"); // rick roll
        /*
        // test
        string fileName = $"{Environment.CurrentDirectory}\\youtube.html";

        if (File.Exists(fileName))
        {
            // Read the contents of the HTML file
            string htmlContent = File.ReadAllText(fileName);
            MessageBox.Show(htmlContent);

            // Replace the placeholder URL with the actual music URL
            string updatedHtmlContent = htmlContent.Replace("PLACEHOLDER", musicUrl);

            // Write the updated HTML content back to the file
            File.WriteAllText(fileName, updatedHtmlContent);
            MessageBox.Show(updatedHtmlContent);

            // Load the updated HTML file into the WebView2 control
            webView.Source = new Uri($"file://{fileName}");
        } */

        // BEGIN ChatGPT code
        webView.Source = new Uri(musicUrl);

        // Inject JavaScript to click the play button
        webView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        // END ChatGPT code
    }

    // begin ChatGPT code, typed
    private async void CoreWebView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
    {
        //await Task.Delay(1000);

        // Check if the navigation was successful
        if (e.IsSuccess)
        {
            //MessageBox.Show("Navigation successful. Starting script...");
            await Task.Delay(500);

            // JavaScript code to simulate a click on the play button with more detailed diagnostics
            string script = @"
            (function() {
                // Find the play-pause button using its data-testid attribute
                let playPauseButton = document.querySelector('button[data-testid=""play-pause-button""]');
                if (playPauseButton) {
                    // Simulate a click on the play-pause button
                    playPauseButton.click();
                    console.log('Play-pause button press simulated.');
                } else {
                    console.log('Play-pause button not found in embedded player.');
                }
            })();
        ";

            // Execute the script and show the result   // remove var?
            var result = await webView.CoreWebView2.ExecuteScriptAsync(script);
            //MessageBox.Show(result);  // returns null (worth looking into?)
            MessageBox.Show("Script successfully executed");

            // detach to handle if navigation occurs again
            webView.CoreWebView2.NavigationCompleted -= CoreWebView2_NavigationCompleted;
            MessageBox.Show("Music should be playing");
        }
        else
        {
            MessageBox.Show("Navigation to the URL failed.");
        }

    }
    // end ChatGPT code

    

    // TODO: Remove this if no references and not needed
    private async void playAlbum(string albumUrl)
    {
        await webView.EnsureCoreWebView2Async(null);
        // Set a standard User-Agent string
        webView.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        //webView.CoreWebView2.Navigate("https://www.youtube.com/embed/9FqaMehDsUc");
        webView.CoreWebView2.Navigate(albumUrl);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Console.WriteLine("Button 1 clicked. Now retrieving albums...");
        // Version 1 temporary code. Notice private method and on Click event name
        // creates new list every time we click the button - I don't think we want this!

        //AlbumsDAO myAlbumsDAO = new AlbumsDAO();  // Version 1
        /*Album a1 = new Album
        {
            ID = 1,
            Artist = 1,
            AlbumTitle = "Test",
            ReleaseYear = 2024,
            ImageURL = "Nothing yet"
        };
        Album a2 = new Album
        {
            ID = 2,
            Artist = 2,
            AlbumTitle = "Test 2",
            ReleaseYear = 2022,
            ImageURL = "Gonna fail"
        };

        // albums list in AlbumsDAO class must be public
        myAlbumsDAO.albums.Add(a1);
        myAlbumsDAO.albums.Add(a2);*/

        // Version 2 actually connects to the database
        AlbumsDAO myAlbumsDAO = new AlbumsDAO();    // Version 2

        // connect the list to the grid view control
        //top.DataSource = myAlbumsDAO.albums;   // Version 1
        topBindingSource.DataSource = myAlbumsDAO.getAllAlbums(); // Version 2

        // tell the grid view that the binding source is associated with it
        dataGridView1.DataSource = topBindingSource;
    }

    // search button click
    private void button2_Click(object sender, EventArgs e)
    {
        // TODO: same functionality if ENTER key pressed and search box is not empty
        Console.WriteLine("Button 2 clicked. Now searching...");
        AlbumsDAO myAlbumsDAO = new AlbumsDAO();

        // connect the list to the grid view control
        topBindingSource.DataSource = myAlbumsDAO.searchAlbumTitles(textBox1.Text);
        //topBindingSource.DataSource = myAlbumsDAO.searchAlbumTitles(contents);

        // tell the grid view that the binding source is associated with it
        dataGridView1.DataSource = topBindingSource;
    }

    private void MusicPlayer_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            // this is the sender, new EventArgs is the EventArgs
            button2_Click(this, new EventArgs());
        }
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        // add explicit cast
        DataGridView dataGridView = (DataGridView)sender;

        generalCellClick(dataGridView, e);

        /*// Check if the click was on a header cell
        if (e.RowIndex == -1)
        {
            // handle header click
            MessageBox.Show("Column header clicked");
            // if column index is valid
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView.Columns.Count)
            {
                // Gets or sets the caption text on the column's header cell
                String headerText = dataGridView.Columns[e.ColumnIndex].HeaderText;
                MessageBox.Show("Header contents: " + headerText);
            }
            // TODO: Sort by that column by calling method in AlbumsDAO???
            return;
        }

        // trying to handle case if someone clicks on that bottom row
        if (e.RowIndex >= dataGridView.RowCount - 1 || dataGridView.CurrentCell.Value == null)
        {
            return; // Do nothing if an invalid cell or the new row placeholder is clicked
        }

        // get the row number clicked
        int rowClicked = dataGridView.CurrentRow.Index;
        String contents = dataGridView.CurrentCell.Value.ToString();
        MessageBox.Show("Clicked row " + rowClicked);
        // cell contents works!! :D
        MessageBox.Show("Cell contents: " + contents);



        // TEST CODE FOR CLICKING THE ALBUM PLAY BUTTON
    
        String forNewQuery = "";
        String toBePlayed = "";

        if (e.ColumnIndex == -1)
        {
            sidePlayButton(dataGridView, forNewQuery, toBePlayed, rowClicked);
            /*String imageURL = dataGridView.Rows[rowClicked].Cells[3].Value?.ToString();

            // test code starts here -- appears to be working as intended
            if (imageURL == null || imageURL.Length == 0)
            {
                pictureBox1.Hide();
                //return;
            }
            else
            {
                MessageBox.Show("Image URL=" + imageURL);
                //pictureBox1.Load(imageURL);   // had to handle according to Wikipedia's User-Agent policy: https://meta.wikimedia.org/wiki/User-Agent_policy
                LoadImageFromUrlAsync(imageURL, pictureBox1);
                pictureBox1.Show();
            }

            MessageBox.Show("Side play button clicked");
            toBePlayed = dataGridView.Rows[(int)rowClicked].Cells[4].Value?.ToString();
            forNewQuery = dataGridView.Rows[(int)rowClicked].Cells[1].Value?.ToString();
            MessageBox.Show("for bottom grid view: " + forNewQuery);
            MessageBox.Show("album URL: " + toBePlayed);

            AlbumsDAO myAlbumsDAOTest = new AlbumsDAO();

            bottomBindingSource.DataSource = myAlbumsDAOTest.retrieveTracksFromAlbum(forNewQuery);
            //playAlbum(toBePlayed);
            RetrieveMusic(toBePlayed);

            // tell the grid view that the binding source is associated with it
            dataGridView2.DataSource = bottomBindingSource;
        }

        // END TEST CODE FOR CLICKING THE ALBUM PLAY BUTTON

        // TODO: Get rid of message boxes
        // TODO: what if cells(column) 2 isn't always the URL?

        // Check if column index is valid
        if (e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView.Columns.Count)
        {
            String imageURL = dataGridView.Rows[rowClicked].Cells[3].Value?.ToString();

            // test code starts here -- appears to be working as intended
            if (imageURL == null || imageURL.Length == 0)
            {
                pictureBox1.Hide();
                //return;
            }
            else
            {
                MessageBox.Show("Image URL=" + imageURL);
                //pictureBox1.Load(imageURL);   // had to handle according to Wikipedia's User-Agent policy: https://meta.wikimedia.org/wiki/User-Agent_policy
                LoadImageFromUrlAsync(imageURL, pictureBox1);
                pictureBox1.Show();
            }

            // TODO: Get rid of message boxes
            // TODO: what if cells(column) 4 isn't always the URL?

            // set headerText if there is any
            String headerText2 = "";
            if (dataGridView.Columns[e.ColumnIndex].HeaderText != null)
            {
                headerText2 = dataGridView.Columns[(int)e.ColumnIndex].HeaderText;
            }

            if ((headerText2 == "AlbumTitle") || (headerText2 == "AlbumID") || (headerText2 == "ReleaseYear") || (headerText2 == "ImageURL"))
            {
                // forNewQuery needs to be AlbumTitle unless you click in Artist column
                forNewQuery = dataGridView.Rows[(int)rowClicked].Cells[1].Value?.ToString();
                MessageBox.Show(forNewQuery);

                AlbumsDAO myAlbumsDAO2 = new AlbumsDAO();

                bottomBindingSource.DataSource = myAlbumsDAO2.retrieveTracksFromAlbum(forNewQuery);

                // tell the grid view that the binding source is associated with it
                dataGridView2.DataSource = bottomBindingSource;
            }
            else if (headerText2 == "Artist")
            {
                // forNewQuery needs to be AlbumTitle unless you click in Artist column
                forNewQuery = contents;
                MessageBox.Show(forNewQuery);

                AlbumsDAO myAlbumsDAO2 = new AlbumsDAO();

                bottomBindingSource.DataSource = myAlbumsDAO2.searchAlbumTitles(forNewQuery);

                // tell the grid view that the binding source is associated with it
                // different binding source for different gridviews
                dataGridView2.DataSource = bottomBindingSource;
            }
            else if (headerText2 == "AlbumURL")
            {
                // forNewQuery needs to be AlbumTitle unless you click in Artist column
                toBePlayed = dataGridView.Rows[(int)rowClicked].Cells[4].Value?.ToString();
                forNewQuery = dataGridView.Rows[(int)rowClicked].Cells[1].Value?.ToString();
                MessageBox.Show("for bottom grid view: " + forNewQuery);
                MessageBox.Show("album URL: " + toBePlayed);

                AlbumsDAO myAlbumsDAO4 = new AlbumsDAO();

                bottomBindingSource.DataSource = myAlbumsDAO4.retrieveTracksFromAlbum(forNewQuery);
                //playAlbum(toBePlayed);
                RetrieveMusic(toBePlayed);

                // tell the grid view that the binding source is associated with it
                dataGridView2.DataSource = bottomBindingSource;
            }
        }*/
    }

    private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        // add explicit cast
        DataGridView dataGridView2 = (DataGridView)sender;

        generalCellClick(dataGridView2, e);
    }

    private void generalCellClick(DataGridView inputDataGridView, DataGridViewCellEventArgs e)
    {
        String headerText = "";
        // Check if the click was on a header cell
        if (e.RowIndex == -1)
        {
            MessageBox.Show("Column header clicked");
            if (e.ColumnIndex >= 0 && e.ColumnIndex < inputDataGridView.Columns.Count)
            {
                // Gets or sets the caption text on the column's header cell
                headerText = inputDataGridView.Columns[e.ColumnIndex].HeaderText;
                MessageBox.Show("Header contents: " + headerText);
            }
            // TODO: Sort by that column by calling method in AlbumsDAO???
            return;
        }

        // trying to handle case if someone clicks on that bottom row
        if (e.RowIndex >= inputDataGridView.RowCount - 1 || inputDataGridView.CurrentCell.Value == null)
        {
            return;     // Do nothing if an invalid cell or the new row placeholder is clicked
        }

        // get the row number clicked
        int rowClicked = inputDataGridView.CurrentRow.Index;
        String contents = inputDataGridView.CurrentCell.Value?.ToString();
        MessageBox.Show("Clicked row " + rowClicked);
        // cell contents works!! :D
        MessageBox.Show("Cell contents: " + contents);

        // TEST CODE FOR CLICKING THE ALBUM PLAY BUTTON

        String forNewQuery = "";
        String albumToBePlayed = "";

        if (e.ColumnIndex == -1)
        {
            sidePlayButton(inputDataGridView, forNewQuery, albumToBePlayed, rowClicked);
        }

        // END TEST CODE FOR CLICKING THE ALBUM PLAY BUTTON

        // TODO: Get rid of message boxes
        // TODO: what if cells(column) 2 isn't always the URL?

        // Check if column index is valid
        if (e.ColumnIndex >= 0 && e.ColumnIndex < inputDataGridView.Columns.Count)
        {
            // if on the filtered album view, switch to track view when user clicks on album
            // else (on the tracklist), pull up video player for given URL

            if (inputDataGridView.Columns[e.ColumnIndex].HeaderText != null)
            {
                headerText = inputDataGridView.Columns[(int)e.ColumnIndex].HeaderText;
            }

            MessageBox.Show("check if column index is valid/headerText: " + headerText);
            if (inputDataGridView.Columns.Count == 5)   // if on album view for particular artist (refactoring)
            //if ((headerText == "AlbumTitle") || (headerText == "AlbumID") || (headerText == "Artist") || (headerText == "ReleaseYear") || (headerText == "ImageURL") || (headerText == "AlbumURL"))    // on album view for particular artist
            {

                // always update image
                String imageURL = inputDataGridView.Rows[rowClicked].Cells[3].Value?.ToString();

                // test code starts here -- appears to be working as intended
                if (imageURL == null || imageURL.Length == 0)
                {
                    pictureBox1.Hide();
                    //return;
                }
                else
                {
                    MessageBox.Show("Image URL=" + imageURL);
                    //pictureBox1.Load(imageURL);   // had to handle according to Wikipedia's User-Agent policy: https://meta.wikimedia.org/wiki/User-Agent_policy
                    LoadImageFromUrlAsync(imageURL, pictureBox1);
                    pictureBox1.Show();
                }


                // switch screen no matter what if already on album view (if you get to this part of code, you are on album view)
                if ((headerText == "AlbumTitle") || (headerText == "AlbumID") || (headerText == "ReleaseYear") || (headerText == "ImageURL"))
                {
                    // forNewQuery needs to be AlbumTitle unless you click in Artist column
                    forNewQuery = inputDataGridView.Rows[(int)rowClicked].Cells[1].Value?.ToString();
                    MessageBox.Show(forNewQuery);

                    AlbumsDAO myAlbumsDAO2 = new AlbumsDAO();

                    bottomBindingSource.DataSource = myAlbumsDAO2.retrieveTracksFromAlbum(forNewQuery);

                    // tell the grid view that the binding source is associated with it
                    dataGridView2.DataSource = bottomBindingSource;
                }
                else if (headerText == "Artist")
                {
                    // forNewQuery needs to be AlbumTitle unless you click in Artist column
                    forNewQuery = contents;
                    MessageBox.Show(forNewQuery);

                    AlbumsDAO myAlbumsDAO2 = new AlbumsDAO();
                    bottomBindingSource.DataSource = myAlbumsDAO2.searchAlbumTitles(forNewQuery);

                    // tell the grid view that the binding source is associated with it
                    // different binding source for different gridviews
                    dataGridView2.DataSource = bottomBindingSource;
                }
                else if (headerText == "AlbumURL")
                {
                    // forNewQuery needs to be AlbumTitle unless you click in Artist column
                    albumToBePlayed = inputDataGridView.Rows[(int)rowClicked].Cells[4].Value?.ToString();
                    forNewQuery = inputDataGridView.Rows[(int)rowClicked].Cells[1].Value?.ToString();
                    MessageBox.Show("for bottom grid view: " + forNewQuery);
                    MessageBox.Show("album URL: " + albumToBePlayed);

                    AlbumsDAO myAlbumsDAO2 = new AlbumsDAO();

                    bottomBindingSource.DataSource = myAlbumsDAO2.retrieveTracksFromAlbum(forNewQuery);
                    //RetrieveMusic(albumToBePlayed);

                    // tell the grid view that the binding source is associated with it
                    dataGridView2.DataSource = bottomBindingSource;
                }
            }
            else if (inputDataGridView.Columns.Count == 3)  // else if showing tracklist (refactoring)
            //else if ((headerText == "TrackTitle") || (headerText == "TrackNumber") || (headerText == "MusicURL"))    // showing tracklist
            {
                String musicURL = inputDataGridView.Rows[rowClicked].Cells[2].Value?.ToString();

                if (string.IsNullOrEmpty(musicURL))
                {
                    MessageBox.Show("headerText: " + headerText);
                    MessageBox.Show("<ERROR: TRACK NOT FOUND>");
                    return;
                }
                else
                {
                    MessageBox.Show("headerText: " + headerText);
                    MessageBox.Show("Music URL=" + musicURL);
                    // new code to pull up video player
                    RetrieveMusic(musicURL);
                }
            }
        }
    }

    private void sidePlayButton(DataGridView inputDataGridView, string inputForNewQuery, string toBePlayed, int rowClicked)
    {
        // if on filtered album view
        if (inputDataGridView.Columns.Count == 5)
        {
            String imageURL = inputDataGridView.Rows[rowClicked].Cells[3].Value?.ToString();

            // test code starts here -- appears to be working as intended
            if (imageURL == null || imageURL.Length == 0)
            {
                pictureBox1.Hide();
                //return;
            }
            else
            {
                MessageBox.Show("Image URL=" + imageURL);
                //pictureBox1.Load(imageURL);   // had to handle according to Wikipedia's User-Agent policy: https://meta.wikimedia.org/wiki/User-Agent_policy
                LoadImageFromUrlAsync(imageURL, pictureBox1);
                pictureBox1.Show();
            }

            MessageBox.Show("Side play button clicked");
            toBePlayed = inputDataGridView.Rows[(int)rowClicked].Cells[4].Value?.ToString();
            inputForNewQuery = inputDataGridView.Rows[(int)rowClicked].Cells[1].Value?.ToString();
            MessageBox.Show("for bottom grid view: " + inputForNewQuery);
            MessageBox.Show("album URL: " + toBePlayed);

            AlbumsDAO myAlbumsDAOTest = new AlbumsDAO();

            bottomBindingSource.DataSource = myAlbumsDAOTest.retrieveTracksFromAlbum(inputForNewQuery);
            RetrieveMusic(toBePlayed);

            // tell the grid view that the binding source is associated with it
            // do not change this from dataGridView2 because we always want that tracklist on the bottom
            dataGridView2.DataSource = bottomBindingSource;
        }
        // else if on track view
        else if (inputDataGridView.Columns.Count == 3)
        {
            // this string could be toBePlayed to reduce code complexity
            String musicURL = inputDataGridView.Rows[rowClicked].Cells[2].Value?.ToString();

            if (string.IsNullOrEmpty(musicURL))
            {
                MessageBox.Show("<ERROR: TRACK NOT FOUND>");
                return;
            }
            else
            {
                MessageBox.Show("Music URL=" + musicURL);
                // new code to pull up video player
                RetrieveMusic(musicURL);
            }
        }
    }

    // beginning of ChatGPT code (handles Wikipedia User-Agent policy)
    private async void LoadImageFromUrlAsync(string imageUrl, PictureBox pictureBox)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the User-Agent header
                client.DefaultRequestHeaders.UserAgent.ParseAdd("WinFormsAppTest5/1.0 (yourname@example.com)");

                HttpResponseMessage response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    pictureBox.Image = Image.FromStream(stream);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading image: " + ex.Message);
        }
    }   // end LoadImageFromUrlAsync

    // end of ChatGPT code

}   // end public partial class MusicPlayer : Form

// TODO: (MOVE THIS BACK INSIDE NAMESPACE) Make new form, connect forms, make "Add new album" box and button
/*private void button3_Click(object sender, EventArgs e)
{
    // TODO: Error checking
    // add a new item to the database
    Album albumFromForm = new Album
    {
        // check to see if artist exists in bands table, and if not add and assign new id
        Artist = txt_albumArtist.Text,
        AlbumTitle = txt_albumName.Text,
        ReleaseYear = Int32.Parse(txt_albumYear.Text),
        ImageURL = txt_ImageURL.Text
    };

    AlbumsDAO albumsDAO = new AlbumsDAO();
    int result = albumsDAO.addOneAlbum(albumFromForm);
    MessageBox.Show(result + " new row(s) inserted");
}*/
