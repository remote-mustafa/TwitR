using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitR.Models.Concrete;

namespace UI.WindowsForm
{
    public partial class Form1 : Form
    {
        HubConnection _tweetHub;
        string baseUrl = "https://localhost:44366";
        HttpClient _client;

        public Form1()
        {
            InitializeComponent();
            //_tweetHub = new HubConnectionBuilder().WithUrl("http://localhost:44366/TweetHub").Build();
            _client = new HttpClient();          
            
        }

        private void btnUserLogin_Click(object sender, EventArgs e)
        {
            Connect();
            GetTweets();
        }

        public void Connect()
        {
             HubConnection connection =  new HubConnectionBuilder().WithUrl(new Uri("https://localhost:44366/WinTweetHub")).WithAutomaticReconnect().Build();
             connection.StartAsync().Wait();
        }

        private async void GetTweets()
        {          
            await _tweetHub.InvokeAsync("GetTweets");

            _tweetHub.On<Tweet>("ReceieveAllTweets", (tweetList) =>
            {
                lbxTweetList.Items.Add($"{tweetList.TweetText}, user:{tweetList.User.UserName}");
            });
        }
    }
}
