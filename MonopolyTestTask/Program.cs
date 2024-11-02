using MonopolyTestTask.Entities;
using System.Text;

namespace MonopolyTestTask
{
    internal class Program
    {
        static Random rnd = new Random(30);
        static int GlobalId = 0;
        static void Main(string[] args)
        {
            List<Pallet> pallets = new();
            for (int i = 0; i < 10; i++)
            {
                var p1 = new Pallet(800, 144, 1200) { Id = GlobalId++ };
                for (int j = 0; j < rnd.Next(3, 5); j++)
                {
                    if (!p1.AddBox(GetRandomBox()))
                        Console.WriteLine("Неудалось добавить коробку на паллет.");
                }
                pallets.Add(p1);
            }


            bool isContinue = true;
            StringBuilder menuSb = new StringBuilder();
            menuSb.AppendLine("Выберите команду:");
            menuSb.AppendLine("0 - выход;");
            //menuSb.AppendLine("1 - пользовательский ввод;");
            menuSb.AppendLine("2 - сгруппировать все паллеты по сроку годности, отсортировать по возрастанию "
                + "срока годности, в каждой группе отсортировать паллеты по весу;");
            menuSb.AppendLine("срока годности, в каждой группе отсортировать паллеты по весу;");
            menuSb.AppendLine("3 - 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема;");
            menuSb.AppendLine("7 - Вывести все паллеты;");
            menuSb.AppendLine("8 - Вывести все паллеты с коробками;");
            menuSb.AppendLine("9 - Вывести все коробки.");


            string str = "", menu = menuSb.ToString();
            while (isContinue)
            {
                Console.WriteLine(menu);
                Console.Write("Ввод: ");
                str = Console.ReadLine() ?? "";

                switch (str)
                {
                    case "0":
                        isContinue = false;
                        break;
                    case "2":
                        PalletsSortGroupByExpirationDateASCInGroupSortByWeigh(pallets);
                        break;
                    case "3":
                        var t3Pallets = Top3PalletsBiggestExpirationDateSortByVolumeASC(pallets);
                        DisplayPalletsWithBoxes(t3Pallets);
                        break;
                    case "7":
                        DisplayPallets(pallets);
                        break;
                    case "8":
                        DisplayPalletsWithBoxes(pallets);
                        break;
                    case "9":
                        DisplayBoxes(pallets);
                        break;
                    default:
                        Console.WriteLine($"Неподдерживаемая команда: {str}");
                        break;
                }

                Console.WriteLine();
            }

        }

        static Box GetRandomBox()
            => rnd.Next(0, 1) == 1
            ? new Box(rnd.Next(10, 800), rnd.Next(10, 10000), rnd.Next(10, 1200), rnd.Next(10, 10000), new DateOnly(2024, 11, 4).AddDays(rnd.Next(10, 100))) { Id = GlobalId++ }
            : new Box(rnd.Next(10, 800), rnd.Next(10, 10000), rnd.Next(10, 1200), rnd.Next(10, 10000), null, new DateOnly(2024, 7, 4).AddDays(rnd.Next(0, 90))) { Id = GlobalId++ };

        static void DisplayHeader()
        {
            Console.WriteLine("{0,-6} | {1,-7} | {2,-6} | {3,-6} | {4,-6} | {5,-8} | {6,-10} | {7}",
                "Тип", "Id", "Ширина", "Высота", "Глубина", "Вес (г)", "Срок годн.", "Объём");
        }


        static void DisplayPallets(IEnumerable<Pallet> pallets)
        {
            DisplayHeader();
            foreach (var pallet in pallets)
            {
                DisplayPallet(pallet);
            }

        }

        static void DisplayPalletsWithBoxes(IEnumerable<Pallet> pallets)
        {
            DisplayHeader();
            foreach (var pallet in pallets)
            {
                DisplayPalletWithBoxes(pallet);
            }

        }

        static void DisplayBoxes(IEnumerable<Pallet> pallets)
        {
            DisplayHeader();
            foreach (var pallet in pallets)
            {
                List<Box> boxes = pallet.GetBoxes();
                foreach (var box in boxes)
                {
                    DisplayBox(box);
                }
            }

        }

        static void DisplayPallet(Pallet pallet)
        {
            Console.WriteLine("{0,6} | {1,7} | {2,6} | {3,6} | {4,7} | {5,8} | {6,10} | {7}",
                pallet.TypeName, pallet.Id, pallet.Width, pallet.Height, pallet.Depth, pallet.Weigh, pallet.ExpirationDate, pallet.Volume);
        }

        static void DisplayPalletWithBoxes(Pallet pallet)
        {
            DisplayPallet(pallet);
            foreach (var box in pallet.GetBoxes())
            {
                DisplayBox(box);
            }
        }

        static void DisplayBox(Box box)
        {
            Console.WriteLine("{0,6} | {1,7} | {2,6} | {3,6} | {4,7} | {5,8} | {6,10} | {7}",
                box.TypeName, box.Id, box.Width, box.Height, box.Depth, box.Weigh, box.ExpirationDate, box.Volume);
        }



        static void PalletsSortGroupByExpirationDateASCInGroupSortByWeigh(List<Pallet> pallets)
        {

        }

        static List<Pallet> Top3PalletsBiggestExpirationDateSortByVolumeASC(List<Pallet> pallets)
        {
            Dictionary<Box, Pallet> newerBoxes = [];

            pallets.ForEach(pallet =>
            {
                List<Box> boxes = pallet.GetBoxes() ?? new List<Box>();

                if (boxes.Count <= 0)
                {
                    return;
                }

                Box newestBox = boxes.First();

                foreach (var box in boxes)
                {
                    if (newestBox.ExpirationDate < box.ExpirationDate)
                    {
                        newestBox = box;
                    }
                }
                newerBoxes.Add(newestBox, pallet);

            });

            int topN = 3;

            var keys = newerBoxes.Keys.ToArray();

            List<Box> newestBoxes = new(keys.Take(topN));
            // сортировка 3 первых паллетов по убыванию срока годности коробки
            for (int i = 1; i < topN; i++)
            {
                var tmpBox = newestBoxes[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (tmpBox.ExpirationDate > newestBoxes[j].ExpirationDate)
                    {
                        newestBoxes[j + 1] = newestBoxes[j];
                        newestBoxes[j] = tmpBox;
                    }
                    else
                    {
                        newestBoxes[j + 1] = tmpBox;
                        break;
                    }
                }
            }

            // Вставка оставшихся паллетов по убыванию срока годности коробки
            for (int i = topN; i < keys.Length; i++)
            {
                if (keys[i].ExpirationDate > newestBoxes[topN - 1].ExpirationDate)
                {
                    for (int j = topN - 2; j >= 0; j--)
                    {
                        if (keys[i].ExpirationDate > newestBoxes[j].ExpirationDate)
                        {
                            newestBoxes[j + 1] = newestBoxes[j];
                            newestBoxes[j] = keys[i];
                        }
                        else
                        {
                            newestBoxes[j + 1] = keys[i];
                            break;
                        }
                    }
                }
            }

            List<Pallet> topNPallets = [];

            foreach (var box in newestBoxes)
            {
                topNPallets.Add(newerBoxes[box]);
            }

            topNPallets.Sort(ComparePalletsByVolumeASC);

            return topNPallets.Slice(0, 3);
        }

        private static int ComparePalletsByExpirationDateASC(Pallet x, Pallet y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    return x.ExpirationDate.CompareTo(y.ExpirationDate);
                }
            }
        }

        private static int CompareBoxesByExpirationDateDESC(Box x, Box y)
            => -1 * CompareBoxesByExpirationDateASC(x, y);

        private static int CompareBoxesByExpirationDateASC(Box x, Box y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    return x.ExpirationDate.CompareTo(y.ExpirationDate);
                }
            }
        }

        private static int ComparePalletsByExpirationDateDESC(Pallet x, Pallet y)
            => -1 * ComparePalletsByExpirationDateASC(x, y);

        private static int ComparePalletsByVolumeASC(Pallet x, Pallet y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    return x.Volume.CompareTo(y.Volume);
                }
            }
        }
    }
}
