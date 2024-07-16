namespace PhoneChecker.Abstractions
{
    public interface IPhoneNormalizer
    {
        /// <summary>
        /// Нормализует номер телефона, оставляя лишь цифры, с кодом страны
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>Нормализованный номер телефона с кодом страны</returns>
        string Normalize(string phoneNumber);
    }
}
