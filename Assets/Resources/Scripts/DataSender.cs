using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSender : MonoBehaviour
{
    [SerializeField] private string BASE_URL = "https://docs.google.com/spreadsheets/d/1kIP0JDDBBstvCxsHtpfC8OUhRe23gj6TBCMoP3TjsRA/edit?usp=sharing";

    public void SendData()
    {
        string questName = "Level1";
        float rating = 0.2f, achiever = 0.25f, explorer = 0.25f, socializer = 0.0f, killer = 0.0f;
        StartCoroutine(Post(questName, rating, achiever, explorer, socializer, killer));
    }

    private IEnumerator Post(string questName, float rating, float achiever, float explorer, float socializer, float killer)
    {
        WWWForm form = new WWWForm();
        form.AddField("Quest Name", questName);
        form.AddField("Rating"    , rating.ToString());
        form.AddField("Achiever"  , achiever.ToString());
        form.AddField("Explorer"  , explorer.ToString());
        form.AddField("Socializer", socializer.ToString());
        form.AddField("Killer"    , killer.ToString());

        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;
    }
}


/*
 using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace testGoogleSheets
{
    public class Attendance
    {
        public string AttendanceId { get; set; }
    }

    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Bartle Quest";
        static string SheetId = "1Sc_oXKICrxwWzugA4-JKhgU4HDMMWm04-yzS14EXr30";

        static void Main(string[] args)
        {
            var service = AuthorizeGoogleApp();
            
            //string newRange = GetRange(service);

            IList<IList<Object>> objNeRecords = GenerateData();
            UpdatGoogleSheetinBatch(objNeRecords, SheetId, "A1:A", service);
        }

        private static SheetsService AuthorizeGoogleApp()
        {
            UserCredential credential;

            ClientSecrets secrets = new ClientSecrets();
            secrets.ClientId = "600335507946-p7tu2i1b1tg3nraotghai04voc4oro3v.apps.googleusercontent.com";
            secrets.ClientSecret = "NUx7sbtlkKr0D2NTtLaU9_H1";

            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                Scopes,
                "user",
                CancellationToken.None).Result;

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        protected static string GetRange(SheetsService service)
        {
            // Define request parameters.
            String spreadsheetId = SheetId;
            String range = "A:A";

            SpreadsheetsResource.ValuesResource.GetRequest getRequest =
                       service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange getResponse = getRequest.Execute();
            IList<IList<Object>> getValues = getResponse.Values;

            int currentCount = getValues.Count() + 2;

            String newRange = "A" + currentCount + ":A";

            return newRange;
        }

        private static IList<IList<Object>> GenerateData()
        {
            List<IList<Object>> objNewRecords = new List<IList<Object>>();

            IList<Object> obj = new List<Object>();

            obj.Add("Level1");
            obj.Add("0,20");
            obj.Add("0,25");
            obj.Add("0,23434");
            obj.Add("0,286");
            obj.Add("1,0");


            objNewRecords.Add(obj);

            return objNewRecords;
        }

        private static void UpdatGoogleSheetinBatch(IList<IList<Object>> values, string spreadsheetId, string newRange, SheetsService service)
        {
            SpreadsheetsResource.ValuesResource.AppendRequest request =
               service.Spreadsheets.Values.Append(new ValueRange() { Values = values }, spreadsheetId, newRange);
            request.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var response = request.Execute();
        }
    }
}
     */
