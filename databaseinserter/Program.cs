using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace databaseinserter
{

    class Program
    {
        public static OracleConnection connection;


        public static OracleConnection getConnection()
        {
            if (connection == null)
            {
                connection = new OracleConnection();
                connection.ConnectionString = "User Id=s95313;Password=s95313;Data Source = (DESCRIPTION = " +
                                            " (ADDRESS = (PROTOCOL = TCP)(HOST = 217.173.198.135)(PORT = 1522    ))" +
                                            " (CONNECT_DATA =" +
                                            " (SERVER = DEDICATED)" +
                                             " (SERVICE_NAME = orcltp.iaii.local)" +
                                            ")" +
                                            ");";

                connection.Open();
            }
            return connection;
        }
        public static long GetNewIndex(String column, String table)
        {
            OracleCommand sel = new OracleCommand();
            String select = "Select max(" + column + ") AS ID FROM " + table;
            sel.Connection = getConnection();
            sel.CommandText = select;
            using (DbDataReader reader = sel.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int index = reader.GetOrdinal("ID");

                        if (!reader.IsDBNull(index))
                        {
                            long oldId = Convert.ToInt64(reader.GetValue(index));
                            return ++oldId;
                        }
                        else return 1;
                    }
                }
            }
            return 1;
        }
        public static List<ulong> ForeignKey(string kolumna, string tabela)
        {
            OracleCommand sel = new OracleCommand();
            List<ulong> indeks = new List<ulong>();
            String select = "select " + kolumna + " as ForeignKey from " + tabela;
            sel.Connection = getConnection();
            sel.CommandText = select;
            using (DbDataReader reader = sel.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int index = reader.GetOrdinal("ForeignKey");

                        if (!reader.IsDBNull(index))
                        {
                            indeks.Add(Convert.ToUInt64(reader.GetValue(index)));
                        }
                    }
                }
            }
            return indeks;
        }
        static void Main(string[] args)
        {
        Start:
            int ile = 1;
            string[] names = { "Leo", "Mike", "Boris", "Andres", "Pawel", "Jacob", "Cristiano", "Janusz", "Zinedine", "Maciek" };
            string[] lastnames = { "Kowalski", "Messi", "Iniesta", "Lewandowski", "Lampard", "Cocoa", "Dundersztyc", "Zidane", "Brzeczyszczykiewicz", "Iksde" };
            string[] tables = { "ZAWODNICY", "GRUPY_TRENINGOWE", "KONTRAKTY", "KONTUZJE", "OBIEKTY_SPORTOWE", "ROZGRYWKI", "SCOUTING", "SPECJALIZACJE", "SZTAB_SZKOLENIOWY", "TERMINARZ", "TRANSFERY", "ZESPOL" };
            string[] columns = { "NR_KART_ZAW", "NR_GRUPY", "ID", "ID_KONTUZJI", "ID_OBIEKTU", "ID_ROZGRYWEK", "ID_PODROZY", "ID_SPEC", "NR_KART_TREN", "NR_SPOTKANIA", "ID_TRANSFERU", "ID_ZESPOLU" };
            string[] nationality = { "Polska", "Argentyna", "Hiszpania", "Niemcy", "Wlochy", "Rosja", "Czachy","Maroko", "Brazylia", "Afganistan" };
            string[] kontuzja = { "glowa", "bark", "noga", "reka", "udo", "brzuch", "klatka", "stopa", "ucho", "nos" };
            string[] obiekty = { "boisko", "silowniaA", "basen", "stadion", "bieznia", "silowniaB", "silowniaC", "boisko2", "boisko3", "basen2" };
            string[] specjalizacja = { "trener bramkarzy", "trener obrony", "trener pomocy", "trener napadu", "trener silowy", "scout", "fizjoterapeuta", "menadzer", "asystent", "lekarz" };
            string[] position = { "Bramkarz", "Obronca", "Napastnik", "Pomocnik" };
            string[] adress = { "Wiejska 44", "La rambla", "Plaça Espanya", "Plaça de les Glòries Catalanes", "Poble Espanyol", "Port Vell", "Rambla de Catalunya", "Tibidabo", "Eixample", "Opolska 21" };
            string[] rozgrywki = { "Liga", "Puchar", "Liga mistrzów", "Klubowe MS" };
            string[] zespoly = { "Real Madryt", "Wisla Krakow", "Valencia", "Chelsea", "AC Milan", "Sevilla", "Bayern", "Man City", "FC Porto", "Karabach" };
            long[] tab = new long[100];
            int i, k = 0, rowCount = 0, p = 0, s = 1;
            int wybor1;
            int wyborAdd;
            int wyborEnd;
            long PrimaryKey = 0;
            string sql = "";
            Random rnd = new Random();
            OracleConnection con;
            con = new OracleConnection();
            con.ConnectionString = "User Id=s95313;Password=s95313;Data Source = (DESCRIPTION = " +
   " (ADDRESS = (PROTOCOL = TCP)(HOST = 217.173.198.135)(PORT = 1522    ))" +
   " (CONNECT_DATA =" +
     " (SERVER = DEDICATED)" +
     " (SERVICE_NAME = orcltp.iaii.local)" +
    ")" +
  ");";

            con.Open();
            List<ulong> ZespolList = ForeignKey("ID_ZESPOLU", "ZESPOL");
            List<ulong> ZawodnicyList = ForeignKey("NR_KART_ZAW", "ZAWODNICY");
            List<ulong> SztabList = ForeignKey("NR_KART_TREN", "SZTAB_SZKOLENIOWY");
            List<ulong> ObiektList = ForeignKey("ID_OBIEKTU", "OBIEKTY_SPORTOWE");
            List<ulong> SpecjalizacjeList = ForeignKey("ID_SPEC", "SPECJALIZACJE");
            List<ulong> RozgrywkiList = ForeignKey("ID_ROZGRYWEK", "ROZGRYWKI");
            try
            {
                Console.WriteLine("///////////////////////////////////////");
                Console.WriteLine("///  Connected to Oracle " + con.ServerVersion + " ///");
                Console.WriteLine("///////////////////////////////////////");
                Console.WriteLine("///Na jakiej tabeli chcesz operować?///");
                Console.WriteLine("///////////////////////////////////////");
                Console.WriteLine("/// 1.Zawodnicy                     ///");
                Console.WriteLine("/// 2.Grupy treningowe              ///");
                Console.WriteLine("/// 3.Kontrakty                     ///");
                Console.WriteLine("/// 4.Kontuzje                      ///");
                Console.WriteLine("/// 5.Obiekty sportowe              ///");
                Console.WriteLine("/// 6.Rozgrywki                     ///");
                Console.WriteLine("/// 7.Scouting                      ///");
                Console.WriteLine("/// 8.Specjalizacje                 ///");
                Console.WriteLine("/// 9.Sztab Szkoleniowy             ///");
                Console.WriteLine("/// 10.Terminarz                    ///");
                Console.WriteLine("/// 11.Transfery                    ///");
                Console.WriteLine("/// 12.Zespół                       ///");
                Console.WriteLine("/// 0.Wyjdz z programu              ///");
                Console.WriteLine("///////////////////////////////////////");
                wybor1 = int.Parse(Console.ReadLine());
                Console.Clear();
            }
            catch(FormatException exception)
            {
                Console.Clear();
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("///                        Musisz podać liczbę                         ///");
                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.ReadKey();
                Console.Clear();
                goto Start;
            }
            
            if (wybor1 > 12)
            {
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("///                       Nieprawidlowy wybor.                         ///");
                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.ReadKey();
                Console.Clear();
                goto Start;
            }
            if (wybor1 < 0)
            {
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("///                       Nieprawidlowy wybor.                         ///");
                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.ReadKey();
                Console.Clear();
                goto Start;
            }
            if (wybor1 == 0)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            try
            {
                Console.WriteLine("Ile wierszy chcesz dodać");
                 wyborAdd = int.Parse(Console.ReadLine());
                Console.Clear();
            }
            catch (FormatException exception)
            {
                Console.Clear();
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("///                        Musisz podać liczbę                         ///");
                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.ReadKey();
                Console.Clear();
                goto Start;
            }
            if (wyborAdd < 1)
            {
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("///                       Nieprawidlowy wybor.                         ///");
                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.ReadKey();
                Console.Clear();
                goto Start;
            }
            string tabela = tables[wybor1 - 1];
            string kolumna = columns[wybor1 - 1];
            PrimaryKey = GetNewIndex(kolumna, tabela);
            for (i = 0; i < wyborAdd; i++)
                {
                  if (wybor1 == 1)
                     {  
                            if (ZespolList.Count == 0)
                            {
                                Console.WriteLine("/////////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac zawodnikow, najpierw trzeba utworzyc conajmniej 1 zespol.///");
                                Console.WriteLine("///                      Kliknij aby kontynuowac.                     ///");
                                Console.WriteLine("/////////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                    goto Start;
                            }
                            sql = "Insert into " + tabela + " (NR_KART_ZAW,IMIE,NAZWISKO,NARODOWOSC,POZYCJA,DATA_URODZENIA,ZESPOL) values (" +PrimaryKey + ",'" + names[k] + "','" + lastnames[k] + "','" + nationality[k] + "','" + position[p] + "',to_date('" + rnd.Next(1, 99) + "/" + rnd.Next(1, 12) + "/" + rnd.Next(1, 28) + "','RR/MM/DD'),"  + ZespolList[rnd.Next(ZespolList.Count)] +  ")";
                            ZawodnicyList = ForeignKey("NR_KART_ZAW", "ZAWODNICY");
                            ++PrimaryKey;
                } else if (wybor1 == 2)
                        {
                            if (ZawodnicyList.Count == 0 )
                            {
                              Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////");
                              Console.WriteLine("///Aby dodac Grupy treningowe, najpierw trzeba utworzyc conajmniej 1 zawodnika, trenera oraz obiekt.///");
                              Console.WriteLine("///                                    Kliknij aby kontynuowac.                                     ///");
                              Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////");
                              Console.ReadKey();
                              Console.Clear();
                              goto Start;
                            } else if (SztabList.Count == 0)
                            {
                               Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////");
                               Console.WriteLine("///Aby dodac Grupy treningowe, najpierw trzeba utworzyc conajmniej 1 zawodnika, trenera oraz obiekt.///");
                               Console.WriteLine("///                                    Kliknij aby kontynuowac.                                     ///");
                               Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////");
                               Console.ReadKey();
                               Console.Clear();
                                 goto Start;
                    }
                            else if (ObiektList.Count == 0)
                            {
                               Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////");
                               Console.WriteLine("///Aby dodac Grupy treningowe, najpierw trzeba utworzyc conajmniej 1 zawodnika, trenera oraz obiekt.///");
                               Console.WriteLine("///                                    Kliknij aby kontynuowac.                                     ///");
                               Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////");
                               Console.ReadKey();
                               Console.Clear();
                               goto Start;
                    }
                    if (PrimaryKey == 100)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                          Utworzono maksymalna ilosc grup.                       ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }

                    sql = "Insert into " + tabela + " (NR_GRUPY,NR_KART_ZAW,NR_KART_TREN,ID_OBIEKTU,TERMIN) values (" + PrimaryKey + "," + ZawodnicyList[rnd.Next(ZawodnicyList.Count)] + "," + SztabList[rnd.Next(SztabList.Count)] + "," + ObiektList[rnd.Next(ObiektList.Count)] + ",to_date('" + rnd.Next(1, 99) + "/" + rnd.Next(1, 12) + "/" + rnd.Next(1, 28) + "','RR/MM/DD')) ";
                            ++PrimaryKey;
                }
                  else if (wybor1 == 3)
                        {
                            
                            if (ZawodnicyList.Count == 0)
                            {
                                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac kontrakt, najpierw trzeba utworzyc conajmniej 1 zawodnika.///");
                                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                goto Start;
                    }
                    if (PrimaryKey == 100)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc kontuzji.                       ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }
                    sql = "Insert into " + tabela + " (ID,NR_KART,PENSJA,WARTOSC,KONIEC_KONTRAKTU) values (" + PrimaryKey + "," + ZawodnicyList[rnd.Next(ZawodnicyList.Count)] + "," + rnd.Next(1, 9999999) + "," + rnd.Next(1, 999999999) + ",to_date('" + rnd.Next(1, 99) + "/" + rnd.Next(1, 12) + "/" + rnd.Next(1, 28) + "','RR/MM/DD'))";
                    ++PrimaryKey;
                }
                  else if (wybor1 == 4)
                        {
                            if (ZawodnicyList.Count == 0)
                            {
                                Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac Kontuzje, najpierw trzeba utworzyc conajmniej 1 zawodnika oraz trenera.///");
                                Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                                Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                goto Start;
                    }
                    else if (SztabList.Count == 0)
                            {
                                Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac Kontuzje, najpierw trzeba utworzyc conajmniej 1 zawodnika oraz trenera.///");
                                Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                                Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                goto Start;
                    }
                            if(PrimaryKey == 100)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc kontuzji.                       ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }
                            sql = "Insert into " + tabela + " (ID_KONTUZJI,NR_KART_ZAW,NR_KART_TREN,RODZAJ_KONTUZJI,KONIEC_KONTUZJI,KOSZT) values ( " + PrimaryKey + "," + ZawodnicyList[rnd.Next(ZawodnicyList.Count)] + "," + SztabList[rnd.Next(SztabList.Count)] + ",'" + kontuzja[k] + " boli',to_date('" + rnd.Next(1, 99) + "/" + rnd.Next(1, 12) + "/" + rnd.Next(1, 28) + "','RR/MM/DD')," + rnd.Next(1, 99999) + ")";
                    ++PrimaryKey;
                }
                  else if (wybor1 == 5)
                        {
                    if (PrimaryKey == 100)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc obiektow.                       ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }
                    sql = "Insert into " + tabela + " (ID_OBIEKTU,NAZWA,GRUPY_TRENUJACE,ADRES) values (" + PrimaryKey + ",'" + obiekty[k] + "'," + rnd.Next(1, 99) + ",'" + adress[k] + "')";
                    ObiektList = ForeignKey("ID_OBIEKTU", "OBIEKTY_SPORTOWE");
                    ++PrimaryKey;
                }
                  else if (wybor1 == 6)
                        {
                    if (PrimaryKey == 100)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc rozgrywek.                      ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }
                    sql = "Insert into " + tabela + " (ID_ROZGRYWEK,ROZGRYWKI) values (" + PrimaryKey + ",'" + rozgrywki[p] + "')";
                    RozgrywkiList = ForeignKey("ID_ROZGRYWEK", "ROZGRYWKI");
                    ++PrimaryKey;
                }
                  else if (wybor1 == 7)
                        {
                            if (SztabList.Count == 0)
                            {
                                Console.WriteLine("//////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac Obiekt, najpierw trzeba utworzyc conajmniej 1 trenera.///");
                                Console.WriteLine("///                    Kliknij aby kontynuowac.                    ///");
                                Console.WriteLine("//////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                goto Start;
                    }
                    if (PrimaryKey == 1003)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc scoutów.                        ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }
                    sql = "Insert into " + tabela + " (ID_PODROZY,NR_KART_TREN,GDZIE,DATA_WYJAZDU,DATA_POWROTU,KOSZT) values (" + PrimaryKey + "," + SztabList[rnd.Next(SztabList.Count)] + ",'" + nationality[k] + "',to_date('" + rnd.Next(10, 15) + "/" + rnd.Next(1, 6) + "/" + rnd.Next(1, 28) + "','RR/MM/DD'),to_date('" + rnd.Next(15, 20) + "/" + rnd.Next(6, 12) + "/" + rnd.Next(1, 28) + "','RR/MM/DD') ," + rnd.Next(1, 99999) + ")";
                    ++PrimaryKey;
                }
                  else if (wybor1 == 8)
                        {
                    if (PrimaryKey == 100)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc specjalizacji.                  ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }
                    sql = "Insert into " + tabela + " (ID_SPEC,SPECJALIZACJA) values (" + PrimaryKey + ",'" + specjalizacja[k] + "')";
                    SpecjalizacjeList = ForeignKey("ID_SPEC", "SPECJALIZACJE");
                    ++PrimaryKey;
                        }
                  else if (wybor1 == 9)
                        {
                            if (SpecjalizacjeList.Count == 0)
                            {
                                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac Trenera, najpierw trzeba utworzyc conajmniej 1 Specjalizacje.///");
                                Console.WriteLine("///                        Kliknij aby kontynuowac.                       ///");
                                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                goto Start;
                    }

                    sql = "Insert into " + tabela + " (NR_KART_TREN,IMIE,NAZWISKO,SPECJALIZACJA,DATA_URODZENIA) values (" + PrimaryKey + ",'" + names[s] + "','" + lastnames[k] + "'," + SpecjalizacjeList[rnd.Next(SpecjalizacjeList.Count)] + ",to_date('" + rnd.Next(1, 99) + "/" + rnd.Next(1, 12) + "/" + rnd.Next(1, 28) + "','RR/MM/DD'))";
                            SztabList = ForeignKey("NR_KART_TREN", "SZTAB_SZKOLENIOWY");
                    ++PrimaryKey;
                }
                  else if (wybor1 == 10)
                        {
                            if (ZespolList.Count == 0)
                            {
                                Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac Spotkanie, najpierw trzeba utworzyc conajmniej 1 zespol oraz rozgrywki.///");
                                Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                                Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                goto Start;
                    }
                    else if (RozgrywkiList.Count == 0)
                            {
                                Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac Spotkanie, najpierw trzeba utworzyc conajmniej 1 zespol oraz rozgrywki.///");
                                Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                                Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                goto Start; 
                    }
                    if (PrimaryKey == 1000)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc spotkan.                        ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }

                    sql = "Insert into " + tabela + " (NR_SPOTKANIA,DATA_SPOTKANIA,PRZECIWNIK,MIEJSCE,TYP_ROZGRYWEK,ZESPOL) values (" + PrimaryKey + ",to_date('" + rnd.Next(1, 99) + "/" + rnd.Next(1, 12) + "/" + rnd.Next(1, 28) + "','RR/MM/DD'),'" + zespoly[k] + "','Dom'," + RozgrywkiList[rnd.Next(RozgrywkiList.Count)] + "," + ZespolList[rnd.Next(ZespolList.Count)] + ")";
                    ++PrimaryKey;
                }
                  else if (wybor1 == 11)
                        {
                            if (ZawodnicyList.Count == 0)
                            {
                                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                                Console.WriteLine("///Aby dodac Transfer, najpierw trzeba utworzyc conajmniej 1 zawodnika.///");
                                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                                Console.ReadKey();
                                Console.Clear();
                                goto Start;
                    }
                    if (PrimaryKey == 1000)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc transferow.                     ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }
                    sql = "Insert into " + tabela + " (ID_TRANSFERU,NR_KART_ZAW,SKAD,DOKAD,RODZAJ_TRANSFERU,TERMIN,KWOTA) values (" + PrimaryKey + "," + ZawodnicyList[rnd.Next(ZawodnicyList.Count)] + ",'" + zespoly[k] + "','Barcelona','Transfer',to_date('" + rnd.Next(1, 99) + "/" + rnd.Next(1, 12) + "/" + rnd.Next(1, 28) + "','RR/MM/DD')," + rnd.Next(1, 99999999) + ")";
                    ++PrimaryKey;
                }
                  else if (wybor1 == 12)
                        {
                    if (PrimaryKey == 10)
                    {
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.WriteLine("///                      Utworzono maksymalna ilosc zespolow.                       ///");
                        Console.WriteLine("///                             Kliknij aby kontynuowac.                            ///");
                        Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////////");
                        Console.ReadKey();
                        Console.Clear();
                        goto Start;
                    }
                    sql = "Insert into " + tabela + " (ID_ZESPOLU,NAZWA) values (" + PrimaryKey + ",'Senior')";
                    ZespolList = ForeignKey("ID_ZESPOLU", "ZESPOL");
                    ++PrimaryKey;
                }
                StreamWriter SW;
                SW = File.AppendText("E:\\plik.txt");
                SW.WriteLine(sql);
                
                OracleCommand cmd = con.CreateCommand();
                            cmd.CommandText = sql;
                             rowCount = cmd.ExecuteNonQuery();
              
                        
                        Console.WriteLine("Dodano wiersz = " + ile);
                ++ile;
                        ++k;++p;
                        k = k % 10;
                        p = p % 4;
                SW.Close();
            }
            Console.Clear();
            Console.WriteLine("Co chcesz zrobic");
            Console.WriteLine("1. Pracuj dalej");
            Console.WriteLine("2. Koniec");
            try
            {
                 wyborEnd = int.Parse(Console.ReadLine());
                Console.Clear();
            }
            catch(FormatException exception)
            {
                Console.Clear();
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("///          Musisz podać liczbę, powrót go głównego menu.             ///");
                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.ReadKey();
                Console.Clear();
                goto Start;
            }
            if (wyborEnd == 1)
            {
                
                goto Start;
             
            }
            else if (wyborEnd == 2)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else if(wyborEnd < 1 || wyborEnd > 1)
            {
                Console.Clear();
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("///           Nieprawidlowy wybor, powrót go głównego menu.            ///");
                Console.WriteLine("///                      Kliknij aby kontynuowac.                      ///");
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
                Console.ReadKey();
                Console.Clear();
                goto Start;
            }

        }
    }
}
