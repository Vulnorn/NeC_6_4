using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace NewC_6_4
{
    class Program
    {
        static void Main(string[] args)
        {
            Dealer dealer = new Dealer();
            dealer.Work();
        }
    }

    class Dealer
    {
        private Deck _deck = new Deck();
        private Player _player = new Player();

        public void Work()
        {
            const string CommandAddCardsPlayer = "1";
            const string CommandShowCardsPlayer = "2";
            const string CommandExit = "3";

            bool isWork = true;

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"Выберите пункт в меню:");
                Console.WriteLine($"{CommandAddCardsPlayer} - Дать карты игроку.");
                Console.WriteLine($"{CommandShowCardsPlayer} - Показать карты игрока.");
                Console.WriteLine($"{CommandExit} - Выход");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandAddCardsPlayer:
                        TransferCard(_player);
                        break;

                    case CommandShowCardsPlayer:
                        _player.ShowCards();
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Ошибка ввода команды.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void TransferCard(Player player)
        {
            Console.WriteLine($"Сколько карт передать игроку?");

            int lowerLimit = 0;
            int quantityCards = Utilite.GetNumberInRange(lowerLimit);

            for (int i = 0; i < quantityCards; i++)
            {
                KneadCards();

                if (_deck.TryGetCard(out Card card))
                {
                    player.TakeCard(card);
                }
            }
        }

        private void KneadCards()
        {
            int quantityCardsDeck = _deck.GetNumbersCards();

            if (quantityCardsDeck == 0)
                _deck.FillCards();
        }
    }

    class Player
    {
        private List<Card> _cards = new List<Card>();

        public void TakeCard(Card card)
        {
            _cards.Add(card);
        }

        public void ShowCards()
        {
            Console.WriteLine("В руке");

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ShowInfo();
            }

            Console.ReadKey();
        }
    }

    class Deck
    {
        private static Random s_random = new Random();
        private Stack<Card> _cards = new Stack<Card>();

        public Deck()
        {
            FillCards();
        }

        public void FillCards()
        {
            List<Card> cards = CreateCards();
            ShuffleCards(cards);
            FoldCards(cards);
        }

        public bool TryGetCard(out Card card)
        {
            return _cards.TryPop(out card);
        }

        public int GetNumbersCards()
        {
            return _cards.Count;
        }

        private List<Card> CreateCards()
        {
            List<Card> cards = new List<Card>();
            string[] ranks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "В", "Д", "К", "Т" };
            string[] suits = new string[] { "♠", "♣", "♦", "♥" };

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = 0; j < ranks.Length; j++)
                {
                    cards.Add(new Card(ranks[j], suits[i]));
                }
            }

            return cards;
        }

        private void ShuffleCards(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int randomIndex = i;

                while (randomIndex == i)
                {
                    randomIndex = s_random.Next(cards.Count);
                }

                Card card = cards[randomIndex];
                cards[randomIndex] = cards[i];
                cards[i] = card;
            }
        }

        private void FoldCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                _cards.Push(card);
            }
        }
    }

    class Card
    {
        private string _rank;
        private string _suit;

        public Card(string rank, string suit)
        {
            _rank = rank;
            _suit = suit;
        }

        public void ShowInfo()
        {
            Console.Write($"{_rank}{_suit} ");
        }
    }

    class Utilite
    {
        public static int GetNumberInRange(int lowerLimitRangeNumbers = Int32.MinValue, int upperLimitRangeNumbers = Int32.MaxValue)
        {
            bool isEnterNumber = true;
            int enterNumber = 0;
            string userInput;

            while (isEnterNumber)
            {
                Console.WriteLine($"Введите число.");

                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out enterNumber) == false)
                    Console.WriteLine("Не корректный ввод.");
                else if (HasNumberInRange(enterNumber, lowerLimitRangeNumbers, upperLimitRangeNumbers))
                    isEnterNumber = false;
            }

            return enterNumber;
        }

        private static bool HasNumberInRange(int number, int lowerLimitRangeNumbers, int upperLimitRangeNumbers)
        {
            if (number < lowerLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за нижний предел допустимого значения.");
                return false;
            }
            else if (number > upperLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за верхний предел допустимого значения.");
                return false;
            }

            return true;
        }
    }
}