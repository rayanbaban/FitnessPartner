using FitnessPartner.Models.DTOs;
using FitnessPartnerAPI.UnitTests;
using Newtonsoft.Json;
using System.Net;

namespace FitnessPartnerAPI.IntergrationTests.FitnessGoals;

public class FitnessGoalsTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public FitnessGoalsTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }

    //[Fact] angir at denne metoden faktisk er en test som skal utføres av testrammeverket.
    [Fact]
    public async Task CreateFitnessGoalAsync()
    {
        //Arrange

        //Act
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IklkcmlzIiwibmFtZWlkIjoiODY5Y2ExODctMDg2MS00MzU2LTg5MTctNDllNmYxZTM2NGQzIiwianRpIjoiZmUyZjU5MmQtNGYxOC00NzQ4LTgyNmUtYjAxZmNmNmI5ZjhjIiwicm9sZSI6IlVTRVIiLCJuYmYiOjE3MTUxMDIzNzEsImV4cCI6MTcxNTEzODM3MSwiaWF0IjoxNzE1MTAyMzcxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTA3IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzEwNyJ9.FYN0u0NtibTiP_y9frqzXRo32chviPMOeBTz7WHDEpc");

        var response = await _client.GetAsync("api/v1/fitnessgoals/");

        var data = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
