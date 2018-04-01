using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;

public class GoogleDataSender : MonoBehaviour
{
    static string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static string ApplicationName = "Bartle Quest";
    static string SheetId = "1Sc_oXKICrxwWzugA4-JKhgU4HDMMWm04-yzS14EXr30";

    void Awake()
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    }

    public void SendData()
    {
        var service = AuthorizeGoogleApp();
        IList<IList<object>> objNeRecords = GenerateData();
        UpdatGoogleSheetinBatch(objNeRecords, SheetId, "A1:A", service);
    }

    private static SheetsService AuthorizeGoogleApp()
    {
        UserCredential credential;

        ClientSecrets secrets = new ClientSecrets();
        secrets.ClientId = "600335507946-p7tu2i1b1tg3nraotghai04voc4oro3v.apps.googleusercontent.com";
        secrets.ClientSecret = "NUx7sbtlkKr0D2NTtLaU9_H1";
        
        /*credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            secrets,
            Scopes,
            "user",
            CancellationToken.None).Result;
            */

        var token = new TokenResponse { RefreshToken = "1/1omuKKwslccR8r7Ak5MtkzqF7SV4wGtA446fP7rHsUM" };

        credential = new UserCredential(
        new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer { ClientSecrets = secrets }),
        "user",
        token);

        // Create Google Sheets API service.
        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        return service;
    }

    private static IList<IList<object>> GenerateData()
    {
        List<IList<object>> objNewRecords = new List<IList<object>>();

        IList<object> obj = new List<object>();

        obj.Add("Level2");
        obj.Add("0,0");
        obj.Add("0,0");
        obj.Add("0,2");
        obj.Add("0,1");
        obj.Add("1,0");

        objNewRecords.Add(obj);

        return objNewRecords;
    }

    private static void UpdatGoogleSheetinBatch(IList<IList<object>> values, string spreadsheetId, string newRange, SheetsService service)
    {
        SpreadsheetsResource.ValuesResource.AppendRequest request =
           service.Spreadsheets.Values.Append(new ValueRange() { Values = values }, spreadsheetId, newRange);
        request.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
        request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        var response = request.Execute();
    }

    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }
}
