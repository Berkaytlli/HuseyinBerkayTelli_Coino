using AppEnvironment;
using Context;
using Entity;
using Repository.OtpKeyRepository;
using System.Text;

namespace GenerateKey.OTP
{
    public class OtpHelper : IOtpHelper
    {
        private readonly IOtpKeyRepositoryService _keyService;
        private readonly ApplicationDbContext _dbContext;
        private OtpKeyOptions _otpKeyOptions;
        public OtpHelper(IOtpKeyRepositoryService keyService, ApplicationDbContext dbContext)
        {
            _keyService = keyService;
            _dbContext = dbContext;
            _otpKeyOptions = new OtpKeyOptions();
        }
        public string GenerateOtpKey(int numbs)
        {
            try
            {
                string otpKey = string.Empty;
                Random rnd = new Random();

                for (int i = 1; i <= numbs; i++)
                    otpKey += rnd.Next(0, 9);
                return otpKey;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public Result<GeneratedOtpKey> CreateOtpKey(int numbs, int userId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                string key = GenerateOtpKey(numbs);
                GeneratedOtpKey generatedOtpKeys = new GeneratedOtpKey();
                generatedOtpKeys.UserId = userId;
                generatedOtpKeys.OtpKeyExpiration = DateTime.Now.AddMinutes(_otpKeyOptions.OtpKeyExpiration);
                generatedOtpKeys.GeneratedKey = key;
                generatedOtpKeys.CreatedBy = userId;
                var resultGenerate = _keyService.Add(generatedOtpKeys, userId);
                if (!resultGenerate.IsSuccess)
                    return new Result<GeneratedOtpKey>(resultGenerate.MessageType ??
                                                                        MessageType.UpdateFailed);
                transaction.Commit();
                return resultGenerate;
            }
            catch (Exception e)
            {
                return new Result<GeneratedOtpKey>(e);
            }
        }

        public Result ValidateOtpKey(string key, int operatorUserId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var getUserKey = _keyService.GetFirst(x => x.OtpKeyExpiration > DateTime.Now && x.GeneratedKey == key);
                if (!getUserKey.IsSuccess)
                    return new Result<GeneratedOtpKey>(getUserKey.MessageType ?? MessageType.RecordNotFound);

                var resultVerify = _keyService.Delete(getUserKey.Data, operatorUserId);
                if (!resultVerify.IsSuccess)
                    return new Result<GeneratedOtpKey>(resultVerify.MessageType ?? MessageType.DeleteFailed);
                transaction.Commit();
                return resultVerify;
            }
            catch (Exception e)
            {
                return new Result<GeneratedOtpKey>(e);
            }
        }
        public string RandomString(int length)
        {
            try
            {
                if (length <= 0) throw new ArgumentOutOfRangeException("length", "length must be more than zero.");
                const int byteSize = 0x100;
                var allowedCharSet = new HashSet<char>(_otpKeyOptions.AllowedChars).ToArray();
                if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));
                using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
                {
                    var result = new StringBuilder();
                    var buf = new byte[128];
                    while (result.Length < length)
                    {
                        rng.GetBytes(buf);
                        for (var i = 0; i < buf.Length && result.Length < length; ++i)
                        {
                            var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                            if (outOfRangeStart <= buf[i]) continue;
                            result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                        }
                    }
                    return result.ToString();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}

