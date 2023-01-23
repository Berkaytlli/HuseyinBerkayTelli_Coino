using AppEnvironment;
using Entity;

namespace GenerateKey.OTP
{
    public interface IOtpHelper
    {
        Result<GeneratedOtpKey> CreateOtpKey(int numbs, int operatorUserId);
        Result ValidateOtpKey(string key, int operatorUserId);
        string GenerateOtpKey(int numbs);
        string RandomString(int length);

    }
}
