

using User_Management_System_Implementation.Models;

namespace User_Management_System_Implementation.Repository.IRepository
{
    public interface IUserVerificationRepository:IRepository<UserVerification>
    {
        public bool IsOtpExpired(UserVerification User);

        public Task<bool> IsVerified(string Identity, string Otp);
    }
}
