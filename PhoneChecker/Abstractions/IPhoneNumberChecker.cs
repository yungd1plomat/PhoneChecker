namespace PhoneChecker.Abstractions
{
    public interface IPhoneNumberChecker
    {
        /// <summary>
        /// Вставить заблокированный номер телефона в бинарное дерево
        /// </summary>
        /// <param name="phoneNumber"></param>
        void Insert(string phoneNumber);

        /// <summary>
        /// Проверяет заблокирован ли номер телефона 
        /// (выполняет поиск по бинарному дереву)
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>Заблокирован ли номер телефона</returns>
        bool IsBlocked(string phoneNumber);
    }
}
