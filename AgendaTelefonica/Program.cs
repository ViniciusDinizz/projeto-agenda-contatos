using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using AgendaTelefonica;

internal class Program
{
    private static void Main(string[] args)
    {
        int op = 0;
        List<PhoneBook> contatos = new List<PhoneBook>();
        Console.Write("Nome da agenda: ");
        var _fileName = Console.ReadLine().ToUpper()+".txt";

        FileExists(_fileName);
        contatos = LoadList(contatos, _fileName);



        do
        {
            Console.Clear();
            op = menu();
            switch (op)
            {
                case 1:
                    AddContact(contatos);
                    List<PhoneBook> contatosorder = contatos.OrderBy(contatos => contatos.NameContact).ToList();
                    contatos = contatosorder;
                    break;
                case 2:
                    contatos = FinishedContact(contatos);
                    break;
                case 3:
                    contatos = DeleteContact(contatos);
                    break;
                case 4:
                    print(contatos);
                    Console.Write("\n\nPress enter continue...");
                    Console.ReadKey();
                    break;
                case 5:
                    Console.WriteLine("Até logo.");
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

        } while (op != 5);

        
        File.WriteAllText(_fileName,SaveToFile(contatos, _fileName));

        //Funções para execução do programa

        List<PhoneBook> DeleteContact(List<PhoneBook> contatos)
        {
            int operador = 0;
            Console.Clear();
            Console.WriteLine("-- EXCLUIR CONTATOS --\n");
            print(contatos);
            Console.Write("\n\nQual nome do contato: ");
            var nameexcluir = Console.ReadLine().ToUpper();
            for (int i = 0; i < contatos.Count; i++)
            {
                operador += contatos[i].CheckNameList(nameexcluir);
            }
            if (operador == 0)
            {
                Console.Write("Contato não existente...");
                Thread.Sleep(2000);
            }
            else
            {
                int contato = CheckForDuplicity(contatos, nameexcluir);
                if (contato == -1)
                {
                    return contatos;
                }
                else
                {
                    contatos.RemoveAt(contato);
                    Console.Write("\n\nRemovendo...");
                    Thread.Sleep(2000);
                }
            }
            return contatos;
        }

        List<PhoneBook> FinishedContact(List<PhoneBook> contatos)
        {
            int operador = 0;
            Console.Clear();
            Console.WriteLine("-- EDITAR CONTATOS --\n");
            print(contatos);
            Console.Write("\n\nQual nome do contato: ");
            var name = Console.ReadLine().ToUpper();
            for (int i = 0; i < contatos.Count; i++)
            {
                operador += contatos[i].CheckNameList(name);
            }
            if (operador == 0)
            {
                Console.Write("Contato não existente...");
                Thread.Sleep(2000);
            }
            else
            {
                int contato = CheckForDuplicity(contatos, name);
                if (contato == -1)
                {
                    return contatos;
                }
                EditContact(contatos, contato);
            }
            return contatos;
        }

        void EditContact(List<PhoneBook> list, int contato)
        {
            int option = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("-- EDITAR CONTATO --\n");
                Console.WriteLine(list[contato].ToString());

                Console.WriteLine("\n\nO que deseja editar-\n[1]- Nome\n[2]- Número\n[3]- Data Aniversário\n[4]- Sair");
                option = CheckNumber();
                switch (option)
                {
                    case 1:
                        Console.Write("Nome: ");
                        var name = Console.ReadLine().ToUpper();
                        list[contato].NameContact = name;
                        Console.Clear();
                        break;
                    case 2:
                        Console.Write("Qual número? ");
                        var numberphone = Console.ReadLine();
                        list[contato].NumberPhone = numberphone;
                        Console.Clear();
                        break;
                    case 3:
                        Console.Write("Dia: ");
                        var day = Console.ReadLine(); ;
                        Console.Write("Mês: ");
                        var month = Console.ReadLine(); ;
                        Console.Write("Ano: ");
                        var year = Console.ReadLine(); ;
                        list[contato].DayBirt = day;
                        list[contato].MonthBirt = month;
                        list[contato].YearBirt = year;
                        Console.Clear();
                        break;
                    case 4:
                        Console.WriteLine("\n\n### Saindo... ### \n");
                        Thread.Sleep(1000);
                        break;
                    default:
                        Console.WriteLine("Campo não existente...");
                        break;
                }
            } while (option != 4);
        }

        int CheckForDuplicity(List<PhoneBook> list, string name)
        {
            int cont = 0, contato = 0, retorno2 = 0;
            for (int i = 0; i < list.Count; i++)
            {
                cont += list[i].CheckNameList(name);
            }
            if (cont > 1)
            {
                Console.Clear();
                int contador = 0;
                Console.WriteLine($"Qual Contato:\n\n");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].NameContact == name)
                    {
                        contador++;
                        Console.WriteLine($"{contador}º Nome: {list[i].NameContact} | Numero: {list[i].NumberPhone}\n");
                    }
                }
                Console.WriteLine("Informe o numero do telefone do contato: ");
                var tel = Console.ReadLine();

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].NumberPhone == tel)
                    {
                        return i;
                    }
                }
                Console.Write("\n\nNúmero incorreto...");
                Thread.Sleep(2000);
                return -1;
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].NameContact == name)
                    {
                        retorno2 = i;
                    }
                }
            }
            return retorno2;
        }

        void AddContact(List<PhoneBook> list)
        {
            Console.Clear();
            Console.Write("### Novo Contato ###\n\nNome do contato: ");
            var name = Console.ReadLine();
            Console.Write("Numero: ");
            var phoneadd = Console.ReadLine();
            Console.Write("Data Nascimento - \nDia: ");
            var day = Console.ReadLine(); ;
            Console.Write("Mês: ");
            var month = Console.ReadLine(); ;
            Console.Write("Ano: ");
            var year = Console.ReadLine(); ;
            list.Add(new PhoneBook(name.ToUpper(), phoneadd, day, month, year));
            Console.Clear();
        }

        int CheckNumber()
        {
            int num = 0;
            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.Write("Digite uma valor válido: ");
            }
            return num;
        }

        int menu()
        {
            Console.WriteLine("### Opções ###\n\n1- Adicionar Contato\n2- Alterar contato\n3- Excluir Contato\n4- Contatos\n5- Sair");
            int op = 0;
            while (!int.TryParse(Console.ReadLine(), out op))
            {
                Console.Clear();
                Console.WriteLine("### Agenda ###\n\n1- Adicionar Contato\n2- Alterar contato\n3- Excluir Contato\n4- Contatos\n5- Sair");
            }
            return op;
        }

        void print(List<PhoneBook> list)
        {
            Console.Clear();
            Console.WriteLine("### AGENDA ###\n");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].ToString());
            }
        }

        List<PhoneBook> LoadList(List<PhoneBook> contatos, string fileName)
        {
            StreamReader srr = new StreamReader(fileName);
            while (!srr.EndOfStream)
            {
                string[] _var = srr.ReadLine().Split(";");
                contatos.Add(new PhoneBook(_var[0], _var[1], _var[2], _var[3], _var[4]));
            }
            srr.Close();
            return contatos;
        }

        void FileExists(string fileName)
        {
            if (!File.Exists(fileName))
            {
                StreamWriter agenda = new StreamWriter(fileName);
                agenda.Close();
            }
        }

        string SaveToFile(List<PhoneBook> agenda, string fileName)
        {
            string _arquiFinish = "";
            for (int i = 0; i < agenda.Count; i++)
            {
                _arquiFinish += agenda[i].GravarArquiSemNum()+"\n";
            }
            return _arquiFinish;
        }
    }

}