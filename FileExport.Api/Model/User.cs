using Bogus;

namespace FileExport.Api.Model
{

    public class GenerateData
    {
        public List<User> GenerateRandomData()
        {
            var userFaker = new Faker<User>("tr")
                .RuleFor(i => i.id, i => i.Random.Guid())
                .RuleFor(i => i.FirstName, i => i.Person.FirstName)
                .RuleFor(i => i.LastName, i => i.Person.LastName)
                .RuleFor(i => i.EmailAddress, i => i.Person.Email)
                .RuleFor(i => i.Age, i => i.Random.Int(15, 65))
                .RuleFor(i => i.Gender, i => i.PickRandom<Gender>());

            var generatedData = userFaker.Generate(10000);
            return generatedData;
        }
    }
    public class User
    {
        public Guid id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
