using MonopolyTestTask.Entities;
using System.Text;

namespace MonopolyTestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            List<Pallet> pallets = new();

            bool isContinue = true;
            StringBuilder menuSb = new StringBuilder();
            menuSb.AppendLine("Выьерите команду:");
            menuSb.AppendLine("0 - выход;");
            menuSb.AppendLine("1 - пользовательский ввод;");
            menuSb.AppendLine("2 - сгруппировать все паллеты по сроку годности, отсортировать по возрастанию "
                + "срока годности, в каждой группе отсортировать паллеты по весу;");
            menuSb.AppendLine("срока годности, в каждой группе отсортировать паллеты по весу;");
            menuSb.AppendLine("3 - 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.");


            string str = "", menu = menuSb.ToString();
            while (isContinue)
            {
                Console.WriteLine(menu);
                str = Console.ReadLine() ?? "";

                switch (str)
                {
                    case "0":
                        isContinue = false;
                        break;
                    case "1":
                        PalletsSortGroupByExpirationDateASCInGroupSortByWeigh(pallets);
                        break;
                    default:
                        Console.WriteLine($"Неподдерживаемая команда: {str}");
                        break;
                }

                Console.WriteLine();
            }

        }

        static void PalletsSortGroupByExpirationDateASCInGroupSortByWeigh(List<Pallet> pallets)
        {

        }

        static void Top3PalletsBiggestExpirationDateSortByVolumeASC(List<Pallet> pallets)
        {

        }

    }
}
