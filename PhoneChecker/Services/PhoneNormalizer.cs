using PhoneChecker.Abstractions;

namespace PhoneChecker.Services
{
    public class PhoneNormalizer : IPhoneNormalizer
    {
        const int PHONE_PREFIX_LEN = 11;

        const int PHONE_SUFFIX_LEN = 10;
        
        private ILogger _logger;

        public PhoneNormalizer(ILogger<PhoneNormalizer> logger) 
        {
            _logger = logger;
        }

        public string Normalize(string phoneNumber)
        {
            // Оставляем только цифры
            var normalizedPhone = new string(phoneNumber.Where(char.IsDigit).ToArray());
            // Если номер телефона не содержит код страны
            if (normalizedPhone.Length == PHONE_SUFFIX_LEN)
            {
                // Добавляем код страны
                normalizedPhone = normalizedPhone.Insert(0, "7");
            }
            // Если номер телефона не той длины кидаем ошибку
            if (normalizedPhone.Length != PHONE_PREFIX_LEN)
            {
                _logger.LogError($"Не удалось нормализовать {phoneNumber}: неверная длина");
                throw new InvalidDataException(phoneNumber);
            }
            _logger.LogInformation($"Нормализован {phoneNumber} -> {normalizedPhone}");
            return normalizedPhone;
        }
    }
}
