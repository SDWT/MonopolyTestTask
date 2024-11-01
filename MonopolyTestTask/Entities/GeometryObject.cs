using MonopolyTestTask.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyTestTask.Entities
{
    public abstract class GeometryObject(int width, int height, int depth) : BaseEntity, IVolumable
    {
        /// <summary>
        /// Ширина объекта
        /// </summary>
        public int Width { get; } = width;

        /// <summary>
        /// Высота объекта
        /// </summary>
        public int Height { get; } = height;

        /// <summary>
        /// Глубина объекта
        /// </summary>
        public int Depth { get; } = depth;

        public virtual int Volume => Depth * Width * Height;
    }

    public interface IWeighable
    {
        /// <summary>
        /// Вес
        /// </summary>
        public int Weigh { get; }
    }

    public interface IVolumable
    {
        /// <summary>
        /// Объём
        /// </summary>
        public int Volume { get; }
    }

    public interface IExpirable
    {
        /// <summary>
        /// Объём
        /// </summary>
        public DateOnly ExpirationDate { get; }
    }

    public interface IMeasurable : IWeighable, IVolumable, IExpirable
    {

    }

    public class Box : GeometryObject, IMeasurable
    {
        private Box(int width, int height, int depth, int weigh) : base(width, height, depth)
        {
            _weigh = weigh;
        }

        private Box(int width, int height, int depth, int weigh, DateOnly expirationDate) : this(width, height, depth, weigh)
        {
            CreatedBy = null;
            ExpirationDate = expirationDate;
        }

        private Box(int width, int height, int depth, int weigh, DateOnly? expirationDate, DateOnly createdBy) : this(width, height, depth, weigh)
        {
            CreatedBy = createdBy;
            ExpirationDate = ((DateOnly)createdBy).AddDays(100);
        }

        private int _weigh;

        public int Weigh { get => _weigh; }

        public DateOnly? CreatedBy { get; private set; }

        public DateOnly ExpirationDate { get; }

    }

    public class Pallet : GeometryObject, IMeasurable
    {
        public Pallet(int width, int height, int depth) : base(width, height, depth)
        {

        }

        private List<Box> Boxes { get; set; }

        public bool AddBox(Box box)
        {
            if (box.Width <= Width &&
                box.Depth <= Depth)
            {
                Boxes.Add(box);
                return true;
            }

            return false;
        }

        public int Weigh => 30000 + Boxes.Sum(b => b.Weigh);

        public override int Volume => base.Volume + Boxes.Sum(b => b.Volume);

        public DateOnly ExpirationDate => Boxes.Count <= 0 ? new DateOnly() : Boxes.Min(b => b.ExpirationDate);
    }
}
