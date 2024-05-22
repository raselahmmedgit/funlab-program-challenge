using FunlabProgramChallenge.Model;

namespace FunlabProgramChallenge.Models
{
    public static class AppDbContextInitializer
    {
        public static bool CreateIfNotExists()
        {
            using (var context = new AppDbContext())
            {
                return context.Database.EnsureCreated();
            }
        }

        public static void SeedData()
        {
            // Create an instance and save the entity to the database
            var member = new Member() { 
                MemberId = 1, 
                FirstName = "Rasel", 
                LastName = "Ahmmed", 
                EmailAddress = "raselahmmed@mail.com", 
                PhoneNumber = "01911-045573",
                PresentAddress = "Dhaka",
                PermanentAddress = "Dhaka",
                CardName = "Rasel Ahmmed",
                CardNumber = "007",
                CardExpirationYear = "26",
                CardExpirationMonth = "12",
                CardCvc = "1234"
            };

            using (var context = new AppDbContext())
            {
                context.Member.Add(member);
                context.SaveChanges();
            }
        }
    }
}
