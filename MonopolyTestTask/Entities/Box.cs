using MonopolyTestTask.Entities.Interfaces;

namespace MonopolyTestTask.Entities
{
    public class Box : GeometryObject, IMeasurable
    {
        private Box(int width, int height, int depth, int weigh) : base(width, height, depth)
        {
            _weigh = weigh;
        }


        /// <summary>
        /// Конструктор с использованием срока годности
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="depth">Глубина</param>
        /// <param name="weigh">Вес</param>
        /// <param name="expirationDate">Срок годности</param>
        public Box(int width, int height, int depth, int weigh, DateOnly expirationDate) : this(width, height, depth, weigh)
        {
            CreatedBy = null;
            ExpirationDate = expirationDate;
        }


        /// <summary>
        /// Конструктор с использованием даты производства коробки
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="depth">Глубина</param>
        /// <param name="weigh">Вес</param>
        /// <param name="expirationDate">Не будет присвоено, а будет вычислено через createdBy + 100 дней, подставляйте null</param>
        /// <param name="createdBy">Дата производство</param>
        public Box(int width, int height, int depth, int weigh, DateOnly? expirationDate, DateOnly createdBy) : this(width, height, depth, weigh)
        {
            CreatedBy = createdBy;
            ExpirationDate = createdBy.AddDays(100);
            //ExpirationDate = expirationDate ?? createdBy.AddDays(100); // Можно заменить, чтобы была возможность указывать оба параметра
        }

        private readonly int _weigh;

        public int Weigh { get => _weigh; }

        public DateOnly? CreatedBy { get; private set; } // Может быть удалено, так как кроме вычисления срока годности нигде не используется

        public DateOnly ExpirationDate { get; }

        public override string TypeName => nameof(Box);
    }
}
