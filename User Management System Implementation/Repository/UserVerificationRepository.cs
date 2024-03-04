using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;

namespace User_Management_System_Implementation.Repository
{
    public class UserVerificationRepository : Repository<UserVerification>, IUserVerificationRepository
    {
        private readonly ApplicationDbContext _context;

        public UserVerificationRepository(ApplicationDbContext options) : base(options)
        {
            _context = options;
        }

        public bool IsOtpExpired(UserVerification User)
        {
            if (User.OtpTimeStamp.HasValue)
            {
                DateTime expirationTime = User.OtpTimeStamp.Value.Add(TimeSpan.FromMinutes(3));
                if (expirationTime < DateTime.UtcNow)
                {
                    User.Otp = null;
                    User.OtpTimeStamp = null;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> IsVerified(string Identity, string Otp)
        {
            var indb = await _context.UserVerifications.FirstOrDefaultAsync(x => x.Identity == Identity);

            if (indb.Otp == Otp)
                return true;
            else
            return false;
        }
    }
}
