using strava.personal.bests.api.Models;

namespace strava.personal.bests.api.tests.TestData
{
    public class AthleteTestData
    {
        public static Athlete GetAnAthlete()
        {
            return new Athlete
            {
                Id = 12345,
                FirstName = "Rich",
                LastName = "Froning",
                ProfileMedium = "http://profile.medium.com",
                Profile = "http://profile.com",
                City = "Cookeville",
                State = "Tennessee",
                Country = "United States",
                Sex = "M",
                Summit = "true",
                CreatedAt = "Z",
                UpdatedAt = "2020-05-08T06:03:08Z",
                FollowerCount = 250,
                FriendCount = 200,
                MeasurementPreference = "meters",
                FTP = 205,
                Weight = 80.5F,
            };
        }
    }
}
