using PhoneChecker.Abstractions;
using PhoneChecker.Models;

namespace PhoneChecker.Repositories
{
    public class PhoneNumberChecker : IPhoneNumberChecker
    {
        // Для блокировки
        private readonly object _lock;

        private readonly ILogger _logger;

        private readonly IPhoneNormalizer _phoneNormalizer;

        // Бинарное дерево
        private Node _root;

        public PhoneNumberChecker(ILogger<PhoneNumberChecker> logger, 
            IPhoneNormalizer phoneNormalizer)
        {
            _root = null;
            _lock = new object();
            _phoneNormalizer = phoneNormalizer;
            _logger = logger;
        }   

        public void Insert(string phoneNumber)
        {
            var normalizedPhone = _phoneNormalizer.Normalize(phoneNumber);
            // Потокобезопасность при вставке
            lock (_lock)
            {
                _root = Insert(_root, normalizedPhone);
                _logger.LogInformation($"Вставлено {normalizedPhone}");
            }
        }

        // Рекурсивное вставка
        private Node Insert(Node root, string phoneNumber)
        {
            // Достигнут последний элемент узла
            if (root == null)
            {
                root = new Node(phoneNumber);
                return root;
            }

            // Если номер меньше текущего узла, идем влево
            if (string.Compare(phoneNumber, root.Phone) < 0)
            {
                root.Left = Insert(root.Left, phoneNumber);
            }
            // Если номер больше текущего узла, идем вправо
            else if (string.Compare(phoneNumber, root.Phone) > 0)
            {
                root.Right = Insert(root.Right, phoneNumber);
            }

            return root;
        }

        public bool IsBlocked(string phoneNumber)
        {
            var normalizedPhone = _phoneNormalizer.Normalize(phoneNumber);
            // Потокобезопасность при поиске
            lock (_lock)
            {
                return Search(_root, normalizedPhone);
            }
        }

        // Рекурсивный поиск
        private bool Search(Node root, string phoneNumber)
        {
            if (root == null)
            {
                return false;
            }

            // Сравниваем номер с текущим узлом
            int comparison = string.Compare(phoneNumber, root.Phone);

            // Если номер найден в текущем узле
            if (comparison == 0)
            {
                return true;
            }
            else if (comparison < 0) // Если номер меньше текущего узла, идем влево
            {
                return Search(root.Left, phoneNumber);
            }
            else // Если номер больше текущего узла, идем вправо
            {
                return Search(root.Right, phoneNumber);
            }
        }

    }
}
