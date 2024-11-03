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
            menuSb.AppendLine("1 - сгруппировать все паллеты по сроку годности, отсортировать по возрастанию "
                + "срока годности, в каждой группе отсортировать паллеты по весу;");
            menuSb.AppendLine("срока годности, в каждой группе отсортировать паллеты по весу;");
            menuSb.AppendLine("2 - 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема;");
            menuSb.AppendLine("7 - вывести все паллеты;");
            menuSb.AppendLine("8 - вывести все паллеты с коробками;");
            menuSb.AppendLine("9 - вывести все коробки.");


            string str = "", menu = menuSb.ToString();

            menuSb.Clear();
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
                    case "1":
                        var t2Pallets = PalletsSortGroupByExpirationDateASCGroupSortByWeigh(pallets);
                        //DisplayPallets(t2Pallets);
                        DisplayPalletsGroupByExpirationDate(t2Pallets, MenuYesNo("Выводить в списке ли коробки?"));
                        break;
                    case "2":
                        var t3Pallets = Top3PalletsBiggestExpirationDateSortByVolumeASC(pallets);
                        DisplayPallets(t3Pallets, true);
                        break;
                    case "7":
                        DisplayPallets(pallets);
                        break;
                    case "8":
                        DisplayPallets(pallets, true);
                        break;
                    case "9":
                        DisplayBoxes(pallets);
                        break;
                    default:
                        Console.WriteLine($"Неподдерживаемая команда: {str}");
                        break;
                }

                Console.WriteLine();
                //pallets.Sort(ComparePalletsByIdASC);
            }

        }

        private static bool MenuYesNo(string question)
        {
            Console.Write($"{question} (Y/n) По умолчанию - n: ");
            string str = Console.ReadLine() ?? "n";
            switch (str)
            {
                case "Y":
                case "y":
                    return true;
                default:
                    return false;
            }
        }

        #region Generating

        private static Box GetRandomBox()
            => rnd.Next(0, 1) == 1
            ? new Box(rnd.Next(10, 800), rnd.Next(10, 10000), rnd.Next(10, 1200), rnd.Next(10, 10000), new DateOnly(2024, 11, 4).AddDays(rnd.Next(10, 100))) { Id = GlobalId++ }
            : new Box(rnd.Next(10, 800), rnd.Next(10, 10000), rnd.Next(10, 1200), rnd.Next(10, 10000), null, new DateOnly(2024, 7, 4).AddDays(rnd.Next(0, 90))) { Id = GlobalId++ };



        #endregion

        #region Console Display

        private static void DisplayHeader()
        {
            Console.WriteLine("{0,-6} | {1,-7} | {2,-6} | {3,-6} | {4,-6} | {5,-8} | {6,-10} | {7}",
                "Тип", "Id", "Ширина", "Высота", "Глубина", "Вес (г)", "Срок годн.", "Объём");
        }

        private static void DisplayBox(Box box)
        {
            Console.WriteLine("{0,6} | {1,7} | {2,6} | {3,6} | {4,7} | {5,8} | {6,10} | {7}",
                box.TypeName, box.Id, box.Width, box.Height, box.Depth, box.Weigh, box.ExpirationDate, box.Volume);
        }

        private static void DisplayBoxes(IEnumerable<Box> boxes)
        {
            foreach (var box in boxes)
            {
                DisplayBox(box);
            }
        }

        private static void DisplayBoxes(IEnumerable<Pallet> pallets)
        {
            DisplayHeader();
            foreach (var pallet in pallets)
            {
                List<Box> boxes = pallet.GetBoxes();
                DisplayBoxes(boxes);
            }

        }
        private static void DisplayPallet(Pallet pallet, bool isWithBoxes = false)
        {
            Console.WriteLine("{0,6} | {1,7} | {2,6} | {3,6} | {4,7} | {5,8} | {6,10} | {7}",
                pallet.TypeName, pallet.Id, pallet.Width, pallet.Height, pallet.Depth, pallet.Weigh, pallet.ExpirationDate, pallet.Volume);

            if (isWithBoxes)
            {
                DisplayBoxes(pallet.GetBoxes());
            }
        }

        private static void DisplayPallets(IEnumerable<Pallet> pallets, bool isWithBoxes = false)
        {
            DisplayHeader();
            foreach (var pallet in pallets)
            {
                DisplayPallet(pallet, isWithBoxes);
            }

        }



        #endregion


        private static int ComparePalletsByIdASC(Pallet x, Pallet y)
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
                    return x.Id.CompareTo(y.Id);
                }
            }
        }


        #region Task 1

        private static void DisplayPalletsGroupByExpirationDate(IEnumerable<Pallet> pallets, bool isWithBoxes = false)
        {
            DateOnly expirationDate = new();
            foreach (var pallet in pallets)
            {
                if (pallet.ExpirationDate != expirationDate)
                {
                    expirationDate = pallet.ExpirationDate;
                    Console.WriteLine($"\nСрок годности: {expirationDate}");
                    DisplayHeader();
                }
                DisplayPallet(pallet, isWithBoxes);
            }
        }


        private static IEnumerable<Pallet> PalletsSortGroupByExpirationDateASCGroupSortByWeigh(List<Pallet> pallets)
        {
            //  Сгруппировать все паллеты по сроку годности,
            //  отсортировать по возрастанию срока годности,
            //  в каждой группе отсортировать паллеты по весу.
            //Dictionary<DateOnly, List<Pallet>> palletsByDate = new();

            //foreach (var pallet in pallets)
            //{
            //    if (!palletsByDate.ContainsKey(pallet.ExpirationDate))
            //    {
            //        palletsByDate.Add(pallet.ExpirationDate, new List<Pallet>());
            //    }

            //    palletsByDate[pallet.ExpirationDate].Add(pallet);
            //}

            // Так как мы только выводим данные, принял решение используя
            // стаюильную сортировку поменять порядок элементов, а не 
            // группировать их в под массивы, а затем при выводе разделять на группы.

            return pallets.OrderBy(pallet => pallet.Weigh).OrderBy(pallet => pallet.ExpirationDate);

        }

        #endregion

        #region Task 2

        private static IEnumerable<Pallet> Top3PalletsBiggestExpirationDateSortByVolumeASC(List<Pallet> pallets)
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

            return topNPallets.Slice(0, 3).OrderBy(pallet => pallet.Volume);
        }

        #endregion

    }
}
