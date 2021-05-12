using System;
using NUnit.Framework;
using RestSharp;
using System.IO;
using FluentAssertions;


namespace DropboxAPI
{
    public class Tests
    {
        private string testFile = @"C:\Users\Fedorov.toxa\Downloads\webapi-testing-master\REDHETMAN\Testfile.txt";
        private string token = "3LI85gRAvxgAAAAAAAAAAfbcW4--LGJq7moIZu6km_PwcN5d6pnsiJx0ZBXqqy4M";

        private IRestClient client;
        private IRestResponse response;
        private IRestRequest request;
        
        
        [SetUp]
        public void RequestInit()
        {
            request = new RestRequest(Method.POST);
        }
        
        
        [Test]
        public void UploadFileRequestIsSuccessful()
        {
            client = new RestClient("https://content.dropboxapi.com/2/files/upload")
            {
                Timeout = -1
            };
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Dropbox-API-Arg", 
            "{\"path\": \"/CZAR/testFile.txt\",\"mode\": \"add\",\"autorename\": true,\"mute\": false,\"strict_conflict\": false}");            
            request.AddHeader("Content-Type", "application/octet-stream");
            byte[] data = File.ReadAllBytes(testFile);
            request.AddParameter("application/octet-stream", data, ParameterType.RequestBody);
            response = client.Execute(request);
            
            response.IsSuccessful.Should().BeTrue();

        }
        
        [Test]
        public void GetMetadataRequestIsSuccessful()
        {
            client = new RestClient("https://api.dropboxapi.com/2/files/get_metadata")
            {
                Timeout = -1
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", 
                "{\"path\": \"/CZAR\",\"include_media_info\": false,\"include_deleted\": false,\"include_has_explicit_shared_members\": false}",
                ParameterType.RequestBody);
            response = client.Execute(request);
            
            Assert.AreEqual(200, (int) response.StatusCode);

        }

        [Test]
        public void WDeleteFileRequestIsSuccessful()
        {
            client = new RestClient("https://api.dropboxapi.com/2/files/delete_v2")
            {
                Timeout = -1
            };
            request.AddHeader("Content-type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", "{\r\n\"path\":\"/CZAR/testFile.txt\"\r\n}",
                ParameterType.RequestBody);
            response = client.Execute(request);
            
            response.IsSuccessful.Should().BeTrue();

        }
        
    }
}