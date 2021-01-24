using Newtonsoft.Json;
using Restpirators.Analyzer.Models;
using Restpirators.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Xunit;

namespace Tests.Endpoints
{
    public class UnitTest1
    {
        private string token { get; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIzZmE4NWY2NC01NzE3LTQ1NjItYjNmYy0yYzk2M2Y2NmFmYTYiLCJleHAiOjE2MTczOTU0Njl9.TkLMyhuLmhbhWfeyYTpGEWGXkvW4UY4BocN6Q2_9VaY";
        [Fact]
        public async void Login()
        {
            var user = new { username = "test", password = "test" };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "http://localhost:5003/login?d=frontend";
            using var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void GetStatistics()
        {
            var _client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "http://localhost:5003/statistics");
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void GetTimeStatistics()
        {
            var _client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), "http://localhost:5003/statistics/getTimeStatistics/0/0");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void All()
        {
            var user = new { username = "test", password = "test" };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "http://localhost:5003/login?d=frontend";
            var client = new HttpClient();
            var loginResponse = await client.PostAsync(url, data);
            string token = loginResponse.Content.ReadAsStringAsync().Result;
            var addRequest = new HttpRequestMessage(new HttpMethod("POST"), "http://localhost:5003/client/Emergencies");
            var emergency = new
            {
                EmergencyTypeId = 1,
                Location = "LokalizacjaTestowa",
                Description = "OpisTestowy",
                Status = EmergencyStatus.New,
                Identifier = "97052602212"
            };
            addRequest.Content = new StringContent(JsonConvert.SerializeObject(emergency), Encoding.UTF8, "application/json");
            var addResponse = await client.SendAsync(addRequest);
            var getEmergencyRequest = new HttpRequestMessage(new HttpMethod("GET"), "http://localhost:5003/emergency/" + emergency.Identifier);
            var getEmergencyResponse = await client.SendAsync(getEmergencyRequest);
            var getAllEmergenciesRequest = new HttpRequestMessage(new HttpMethod("GET"), "http://localhost:5003/statistics");
            getAllEmergenciesRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Thread.Sleep(1000);
            var getAllEmergenciesResponse = await client.SendAsync(getAllEmergenciesRequest);
            IEnumerable<Rootobject> emergencies = JsonConvert.DeserializeObject<Rootobject[]>(getAllEmergenciesResponse.Content.ReadAsStringAsync().Result);
            var addedEmergency = emergencies.OrderByDescending(x => x.ModDate).FirstOrDefault();
            var emergencyToUpdate = new
            {
                Id = addedEmergency.EmergencyId,
                EmergencyTypeId = 1,
                Location = "LokalizacjaTestowa",
                Description = "OpisTestowy",
                Status = EmergencyStatus.TeamSent,
                Identifier = "97052602212",
                AssignedToTeamId = 1
            };
            var updateEmergencyRequest = new HttpRequestMessage(new HttpMethod("PUT"), "http://localhost:5003/emergencies");
            updateEmergencyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            updateEmergencyRequest.Content = new StringContent(JsonConvert.SerializeObject(emergencyToUpdate), Encoding.UTF8, "application/json");
            var updateEmergencyResponse = await client.SendAsync(updateEmergencyRequest);
            var getAllEmergenciesAfterUpdateRequest = new HttpRequestMessage(new HttpMethod("GET"), "http://localhost:5003/statistics");
            getAllEmergenciesAfterUpdateRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Thread.Sleep(1000);
            var getAllEmergenciesAfterUpdateResponse = await client.SendAsync(getAllEmergenciesAfterUpdateRequest);
            IEnumerable<Rootobject> updatedEmergencies = JsonConvert.DeserializeObject<Rootobject[]>(getAllEmergenciesAfterUpdateResponse.Content.ReadAsStringAsync().Result);
            var updatedEmergency = updatedEmergencies.OrderByDescending(x => x.ModDate).FirstOrDefault();
            var secondUpdateEmergencyRequest = new HttpRequestMessage(new HttpMethod("PUT"), "http://localhost:5003/emergencies");
            secondUpdateEmergencyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var emergencyToSecondUpdate = new
            {
                Id = addedEmergency.EmergencyId,
                EmergencyTypeId = 1,
                Location = "LokalizacjaTestowa",
                Description = "OpisTestowy",
                Status = EmergencyStatus.Done,
                Identifier = "97052602212",
                AssignedToTeamId = 1
            };
            secondUpdateEmergencyRequest.Content = new StringContent(JsonConvert.SerializeObject(emergencyToSecondUpdate), Encoding.UTF8, "application/json");
            var secondUpdateEmergencyResponse = await client.SendAsync(secondUpdateEmergencyRequest);
            var getAllEmergenciesAfterSecondUpdateRequest = new HttpRequestMessage(new HttpMethod("GET"), "http://localhost:5003/statistics");
            getAllEmergenciesAfterSecondUpdateRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Thread.Sleep(1000);
            var getAllEmergenciesAfterSecondUpdateResponse = await client.SendAsync(getAllEmergenciesAfterSecondUpdateRequest);
            IEnumerable<Rootobject> secondUpdatedEmergencies = JsonConvert.DeserializeObject<Rootobject[]>(getAllEmergenciesAfterSecondUpdateResponse.Content.ReadAsStringAsync().Result);
            var secondUpdatedEmergency = secondUpdatedEmergencies.OrderByDescending(x => x.ModDate).FirstOrDefault();
            Assert.True(addedEmergency != null && addedEmergency.Status == EmergencyStatus.New && updatedEmergency != null && updatedEmergency.Status == EmergencyStatus.TeamSent
                && secondUpdatedEmergency != null && secondUpdatedEmergency.Status == EmergencyStatus.Done);
            Assert.Equal(
                new[] { HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK, HttpStatusCode.OK },
                new[] { loginResponse.StatusCode, addResponse.StatusCode, getEmergencyResponse.StatusCode, updateEmergencyResponse.StatusCode, getAllEmergenciesResponse.StatusCode,
                updateEmergencyResponse.StatusCode, getAllEmergenciesAfterUpdateResponse.StatusCode, secondUpdateEmergencyResponse.StatusCode, getAllEmergenciesAfterSecondUpdateResponse.StatusCode});
        }

        public class Rootobject
        {
            public _Id _id { get; set; }
            public int EmergencyId { get; set; }
            public int EmergencyTypeId { get; set; }
            public EmergencyStatus Status { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
            public DateTime ModDate { get; set; }
        }

        public class _Id
        {
            public string oid { get; set; }
        }

    }
}
