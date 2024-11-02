using MonopolyTestTask.Entities.Interfaces;

namespace MonopolyTestTask.Entities
{
    public class Box : GeometryObject, IMeasurable
    {
        private Box(int width, int height, int depth, int weigh) : base(width, height, depth)
        {
            _weigh = weigh;
        }

        public Box(int width, int height, int depth, int weigh, DateOnly expirationDate) : this(width, height, depth, weigh)
        {
            CreatedBy = null;
            ExpirationDate = expirationDate;
        }

        public Box(int width, int height, int depth, int weigh, DateOnly? expirationDate, DateOnly createdBy) : this(width, height, depth, weigh)
        {
            CreatedBy = createdBy;
            ExpirationDate = ((DateOnly)createdBy).AddDays(100);
        }

        private int _weigh;

        public int Weigh { get => _weigh; }

        public DateOnly? CreatedBy { get; private set; }

        public DateOnly ExpirationDate { get; }

        public override string TypeName => nameof(Box);
    }
}
