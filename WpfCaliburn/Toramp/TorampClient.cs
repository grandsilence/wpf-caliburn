using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Leaf.Core.Extensions.String;

namespace WpfCaliburn.Toramp
{
    public sealed class TorampClient : NetProvider
    {
        public TorampClient() : base("https://www.toramp.com")
        {

        }

        public bool Authenticated { get; private set; }

        public async Task Login(string login, string password)
        {
            Logout();

            // GET login page
            var resp = await Http.GetAsync("/login.php");
            resp.EnsureSuccessStatusCode();

            string html = await resp.Content.ReadAsStringAsync();
            if (!html.Contains("Вход на сайт") || !html.Contains("Toramp"))
                throw new ParsingException("Сайт видимо сменил код, нужно дорабатывать", html);

            // POST login data
            var req = new HttpRequestMessage(HttpMethod.Post, "/takelogin.php") {
                Content = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("username", login),
                    new KeyValuePair<string, string>("password", password),
                })
            };
            req.Headers.Referrer = new Uri("https://www.toramp.com/login.php");

            resp = await Http.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            // TODO: validation
            html = await resp.Content.ReadAsStringAsync();
            
            // TODO: check no redirection
            Authenticated = true;
        }

        public async Task<string> GetChart()
        {
            if (!Authenticated)
                return null;

            string resp = await Http.GetStringAsync("/my-chart.php");
            if (ShouldLogin(resp))
                return null;

            // TODO: change to chart message!!
            if (resp.Contains(">Пока вы&nbsp;ещё не&nbsp;добавили ни&nbsp;одного сериала"))
                return string.Empty;

            string content = resp.BetweenEx("<div id=\"content\">", "<div id=\"load\">");
            var tables = content.BetweensEx("<table", "</table>");

            foreach (string table in tables)
            {
                // Ignore Yandex metrics / ads
                if (table.Contains("yandex_rtb"))
                    continue;

                var rows = table.BetweensOrEmpty("<tr>", "</tr>");
                if (rows.Length < 2)
                    continue;

                // Parse Image and Title
                string rowTitle = rows[0];
                var cols = rowTitle.BetweensOrEmpty("<td", "</td>");
                if (cols.Length < 2)
                    continue;

                string image = cols[0]
                    .BetweenLast("class=\"p-shadow\"", "<img src=")
                    ?.Between("\"", "\"");

                string title = cols[1]
                    .BetweenLast("</a>", "\">");

                // Parse Details: Season, Episode, Release date
                string rowDetails = rows[1];
                cols = rowDetails.BetweensOrEmpty("<td", "</td>");
                if (cols.Length < 3)
                    continue;

                // RegExp not required because it's slower 5x
                // var regex = new Regex(@"Сезон\s+(\d+).*?эпизод\s+(\d+)", RegexOptions.CultureInvariant);

                // TODO: parse int
                string seasonTd = cols[0].Between("<em>", "<br />")?.Replace("Сезон", "").Trim();
                string episodeTd = cols[0].Between("<br />", "</em>")?.Replace("эпизод", "").Trim();

                string releaseDate = cols[2].Between("\">", "<br />")?.Trim();
                bool validDate = DateTime.TryParse(releaseDate, out var releaseDateTime);

                /*
                foreach (string td in tds)
                {
                    // Parsing Serial image
                    if (td.Contains("<img src=\"pic/my_chart"))
                    {
                        // Fast solution without RegExp
                        string serialImage = td
                            .BetweenLast("class=\"p-shadow\"", "<img src=")
                            ?.Between("\"", "\""); // also can be used string.Split
                    }
                    else if (td.Contains())


                }*/

            }


            // TODO: parsing serials
            return resp;
        }

        // /series.php


        public void Logout()
        {
            Authenticated = false;
        }

        private static bool ShouldLogin(string html) => html.Contains("<a href=\"login.php\">войдите</a>");
    }
}