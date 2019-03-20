using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Serialization;

namespace AmberMeet.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var a = new UserRole().GetDescriptions();
        }
    }
}